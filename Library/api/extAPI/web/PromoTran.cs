using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using web;
using web.protocol;

namespace BU.data
{
    public abstract class PromoTranProtocol : TranProtocol<PromoTranRow>
    {
        [JsonProperty]
        public virtual string Memo1 { get; set; }
        [JsonProperty]
        public virtual string Memo2 { get; set; }

        static BU.LogType[] promos1 = new BU.LogType[]
            {
                BU.LogType.首存優惠, 
                BU.LogType.存款優惠, 
                BU.LogType.洗碼優惠, 
                BU.LogType.全勤優惠,
            };
        static BU.LogType[] promos2 = new BU.LogType[]
            {
                BU.LogType.首存優惠_前置單, 
                BU.LogType.存款優惠_前置單, 
                BU.LogType.洗碼優惠_前置單, 
                BU.LogType.全勤優惠_前置單
            };

        protected override string _tableName { get { return "PromoTran1"; } }

        public override jgrid.RowResponse update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                PromoTranRow row = null;
                if (this.ID.HasValue)
                    row = sqlcmd.ToObject<PromoTranRow>("select * from PromoTran1 nolock where ID='{0}'", this.ID);
                if (row == null)
                    return jgrid.RowResponse.UpdateMissing();
                sqltool s = new sqltool();
                s["N", "Memo1", "", row.Memo1, ""] = this.Memo1;
                s["N", "Memo2", "", row.Memo2, ""] = this.Memo2;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["ID"] = row.ID;
                    string sqlstr = s.SqlExport(s.Build("update PromoTran1 set ", sqltool._FieldValue, " where ID={ID} select * from PromoTran1 nolock where ID={ID}"));
                    jgrid.RowResponse res;
                    if (!sqlcmd.Execute(out row, out res, sqlstr))
                        return res;
                }
                return this.process_tran(sqlcmd, row);
            }
        }

        public override jgrid.RowResponse insert(string json_s, params object[] args)
        {
            if (this.Amount1 <= 0) this.Amount1 = null;
            sqltool s = new sqltool();
            if ((s.Values["prefix"] = this.LogType.GetPrefix(promos1) ?? this.LogType.GetPrefix(promos2)) == null)
                this.LogType = null;
            s["*", "LogType", "     "] = this.LogType;
            s["*", "GameID", "      "] = 0;
            s["*", "MemberID", "    "] = (StringEx.sql_str)"ID";
            s["*", "CorpID", "      "] = this.CorpID;
            s["*", "MemberACNT", "  "] = this.MemberACNT *= text.ValidAsACNT;
            s["*", "Amount1", "     "] = this.Amount1;
            s[" ", "Amount2", "     "] = 0;
            s[" ", "State", "       "] = TranState.Initial;
            s[" ", "CurrencyA", "   "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyB", "   "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyX", "   "] = 1;
            s[" ", "SerialNumber", ""] = (StringEx.sql_str)"@SerialNumber";
            s[" ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
            s["N", "Memo1", "       "] = this.Memo1;
            s["N", "Memo2", "       "] = this.Memo2;
            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
            if (s.needs != null)
                return jgrid.RowResponse.FieldNeeds(s.needs);
            s.Values["CorpID"] = (StringEx.sql_str)"CorpID";
            s.Values["MemberACNT"] = (StringEx.sql_str)"ACNT";
            s.Values["CorpID_"] = this.CorpID;
            s.Values["ACNT_"] = this.MemberACNT;

            string sqlstr = s.SqlExport(s.Build(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into PromoTran1 (ID, ", sqltool._Fields, @")
select @ID,", sqltool._Values, @"n from Member nolock where CorpID={CorpID_} and ACNT={ACNT_}
select * from PromoTran1 nolock where ID=@ID"));
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                jgrid.RowResponse res;
                PromoTranRow row;
                if (!sqlcmd.Execute(out row, out res, sqlstr))
                    return res;
                return this.process_tran(sqlcmd, row);
            }
        }

        jgrid.RowResponse process_tran(SqlCmd sqlcmd, PromoTranRow row)
        {
            this._TranValidOp();
            try
            {
                if (this.accept.HasValue && promos2.Conatins(row.LogType))
                {
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (this.accept.Value == AcceptOrReject.Accept)
                        {
                            this._TranGetAmount(sqlcmd, row, null);
                            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update PromoTran1 set LogType=LogType-10,ModifyTime=getdate(),ModifyUser={2} where ID='{0}' and LogType={1}
select * from PromoTran1 nolock where ID='{0}'", row.ID, (int)row.LogType.Value, row.ModifyUser))
                                r.FillObject(row);
                        }
                        else
                        {
                            row.State = TranState.Rejected;
                            this._TranFinish(sqlcmd, row, "Amount2");
                        }
                        sqlcmd.Commit();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                if (this.commit.HasValue && promos1.Conatins(row.LogType))
                    this._TranIn(sqlcmd, row);
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
                jgrid.RowResponse res2 = jgrid.RowResponse.Error(ex);
            }
            return jgrid.RowResponse.Success(row);
        }

        protected override void _TranFinish(SqlCmd sqlcmd, PromoTranRow row, string amount2)
        {
            sqlcmd.ExecuteNonQuery(@"
insert into PromoTran2 (ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,Amount2,State,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,FinishTime,CreateUser, ModifyUser,Memo1,Memo2)
select                  ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,{3}    ,{1}  ,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,getdate() ,CreateUser,{2}        ,Memo1,Memo2
from PromoTran1 nolock where ID='{0}' delete GameTran1 where ID='{0}'", row.ID, (int)row.State.Value, row.ModifyUser, amount2);
        }
    }
}