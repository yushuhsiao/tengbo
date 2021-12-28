using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PromoTranRow : TranRow
    {
        [DbImport, JsonProperty]
        public string Memo1;
        [DbImport, JsonProperty]
        public string Memo2;
    }

    public abstract class PromoTranRowCommand : TranRowCommand<PromoTranRow>
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
                BU.LogType.彩金贈送,
            };
        static BU.LogType[] promos2 = new BU.LogType[]
            {
                //BU.LogType.首存優惠_前置單, 
                //BU.LogType.存款優惠_前置單, 
                //BU.LogType.洗碼優惠_前置單, 
                //BU.LogType.全勤優惠_前置單
            };

        protected override string TableName { get { return "PromoTran"; } }

        //        public PromoTranRow update(string json_s, params object[] args)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //            {
        //                PromoTranRow row = sqlcmd.GetRowEx<PromoTranRow>(RowErrorCode.PromoTranNotFount, "select * from PromoTran1 nolock where ID='{0}'", this.ID);
        //                sqltool s = new sqltool();
        //                s["N", "Memo1", "", row.Memo1, ""] = this.Memo1;
        //                s["N", "Memo2", "", row.Memo2, ""] = this.Memo2;
        //                if (s.fields.Count > 0)
        //                {
        //                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
        //                    s.Values["ID"] = row.ID;
        //                    string sqlstr = s.BuildEx("update PromoTran1 set ", sqltool._FieldValue, " where ID={ID} select * from PromoTran1 nolock where ID={ID}");
        //                    row = sqlcmd.ExecuteEx<PromoTranRow>(sqlstr);
        //                }
        //                return this.process_tran(sqlcmd, row);
        //            }
        //        }

        //        public PromoTranRow insert(string json_s, params object[] args)
        //        {
        //            if (this.Amount1 <= 0) this.Amount1 = null;
        //            sqltool s = new sqltool();
        //            if ((s.Values["prefix"] = this.LogType.GetPrefix(promos1) ?? this.LogType.GetPrefix(promos2)) == null)
        //                this.LogType = null;
        //            s["*", "LogType", "     "] = this.LogType;
        //            s["*", "GameID", "      "] = 0;
        //            s["*", "MemberID", "    "] = (StringEx.sql_str)"ID";
        //            s["*", "CorpID", "      "] = this.CorpID;
        //            s["*", "MemberACNT", "  "] = this.MemberACNT *= text.ValidAsACNT;
        //            s["*", "Amount1", "     "] = this.Amount1;
        //            s[" ", "Amount2", "     "] = 0;
        //            s[" ", "State", "       "] = TranState.Initial;
        //            s[" ", "CurrencyA", "   "] = (StringEx.sql_str)"Currency";
        //            s[" ", "CurrencyB", "   "] = (StringEx.sql_str)"Currency";
        //            s[" ", "CurrencyX", "   "] = 1;
        //            s[" ", "SerialNumber", ""] = (StringEx.sql_str)"@SerialNumber";
        //            s[" ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
        //            s["N", "Memo1", "       "] = this.Memo1;
        //            s["N", "Memo2", "       "] = this.Memo2;
        //            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
        //            s.TestFieldNeeds();
        //            s.Values["CorpID"] = (StringEx.sql_str)"CorpID";
        //            s.Values["MemberACNT"] = (StringEx.sql_str)"ACNT";
        //            s.Values["CorpID_"] = this.CorpID;
        //            s.Values["ACNT_"] = this.MemberACNT;

        //            string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
        //insert into PromoTran1 (ID, ", sqltool._Fields, @")
        //select @ID,", sqltool._Values, @"n from Member nolock where CorpID={CorpID_} and ACNT={ACNT_}
        //select * from PromoTran1 nolock where ID=@ID");
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //            {
        //                PromoTranRow row = sqlcmd.ExecuteEx<PromoTranRow>(sqlstr);
        //                return this.process_tran(sqlcmd, row);
        //            }
        //        }

        //        PromoTranRow process_tran(SqlCmd sqlcmd, PromoTranRow row)
        //        {
        //            this._TranValidOp();
        //            try
        //            {
        //                if (this.accept.HasValue && promos2.Conatins(row.LogType))
        //                {
        //                    try
        //                    {
        //                        sqlcmd.BeginTransaction();
        //                        if (this.accept.Value == AcceptOrReject.Accept)
        //                        {
        //                            this._TranGetAmount(sqlcmd, row, null);
        //                            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update PromoTran1 set LogType=LogType-10,ModifyTime=getdate(),ModifyUser={2} where ID='{0}' and LogType={1}
        //select * from PromoTran1 nolock where ID='{0}'", row.ID, (int)row.LogType.Value, row.ModifyUser))
        //                                r.FillObject(row);
        //                        }
        //                        else
        //                        {
        //                            row.State = TranState.Rejected;
        //                            this._TranFinish(sqlcmd, row, "Amount2");
        //                        }
        //                        sqlcmd.Commit();
        //                    }
        //                    catch
        //                    {
        //                        sqlcmd.Rollback();
        //                        throw;
        //                    }
        //                }

        //                if (this.commit.HasValue && promos1.Conatins(row.LogType))
        //                    this._TranIn(sqlcmd, row);
        //            }
        //            catch (Exception ex)
        //            {
        //                log.error_msg(ex);
        //                //jgrid.RowResponse res2 = jgrid.RowResponse.Error(ex);
        //            }
        //            return row;
        //        }

        //        protected override void _TranFinish(SqlCmd sqlcmd, PromoTranRow row, string amount2)
        //        {
        //            sqlcmd.ExecuteNonQuery(@"
        //insert into PromoTran2 (ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,Amount2,State,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,FinishTime,CreateUser, ModifyUser,Memo1,Memo2)
        //select                  ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,{3}    ,{1}  ,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,getdate() ,CreateUser,{2}        ,Memo1,Memo2
        //from PromoTran1 nolock where ID='{0}' delete GameTran1 where ID='{0}'", row.ID, (int)row.State.Value, row.ModifyUser, amount2);
        //        }

        public override PromoTranRow Insert(string json_s, params object[] args) { return this.Insert(null, json_s, args); }
        public PromoTranRow Insert(SqlCmd sqlcmd, string json_s, params object[] args)
        {
            if (this.Amount1 <= 0) this.Amount1 = null;
            sqltool s = new sqltool();
            if (this.LogType.IsPromos())
                s.Values["prefix"] = this.LogType.GetPrefix();
            else
                this.LogType = null;
            //if ((s.Values["prefix"] = this.LogType.GetPrefix(promos1) ?? this.LogType.GetPrefix(promos2)) == null) this.LogType = null;
            s["*", "LogType", "     "] = this.LogType;
            s["*", "GameID", "      "] = 0;
            s["*", "CorpID", "      "] = this.CorpID;
            s["*", "MemberID", "    "] = (StringEx.sql_str)"a.ID";
            s["*", "MemberACNT", "  "] = this.MemberACNT;
            s["*", "AgentID", "     "] = (StringEx.sql_str)"b.ID";
            s["*", "AgentACNT", "   "] = (StringEx.sql_str)"b.ACNT";
            s["*", "Amount1", "     "] = this.Amount1;
            s[" ", "Amount2", "     "] = 0;
            s[" ", "State", "       "] = TranState.Initial;
            s[" ", "CurrencyA", "   "] = (StringEx.sql_str)"a.Currency";
            s[" ", "CurrencyB", "   "] = (StringEx.sql_str)"a.Currency";
            s[" ", "CurrencyX", "   "] = 1;
            s[" ", "SerialNumber", ""] = (StringEx.sql_str)"@SerialNumber";
            s[" ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
            s["N", "Memo1", "       "] = this.Memo1;
            s["N", "Memo2", "       "] = this.Memo2;
            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
            s.TestFieldNeeds();
            s.Values["CorpID"] = (StringEx.sql_str)"a.CorpID";
            s.Values["MemberACNT"] = (StringEx.sql_str)"a.ACNT";
            s.Values["CorpID_"] = this.CorpID;
            s.Values["ACNT_"] = this.MemberACNT;

            string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into PromoTran1 (ID, ", sqltool._Fields, @")
select @ID,", sqltool._Values, @"n from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID where a.CorpID={CorpID_} and a.ACNT={ACNT_}
select * from PromoTran1 nolock where ID=@ID");

            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            {
                PromoTranRow row = sqlcmd.ExecuteEx<PromoTranRow>(sqlstr);
                this.ID = row.ID;
                return op_exec(sqlcmd, row, json_s, args);
            }
        }

        public override PromoTranRow Update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                PromoTranRow row = sqlcmd.GetRowEx<PromoTranRow>(RowErrorCode.PromoTranNotFount, "select * from PromoTran1 nolock where ID='{0}'", this.ID);
                sqltool s = new sqltool();
                s["N", "Memo1", "", row.Memo1, ""] = this.Memo1;
                s["N", "Memo2", "", row.Memo2, ""] = this.Memo2;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["ID"] = row.ID;
                    string sqlstr = s.BuildEx("update PromoTran1 set ", sqltool._FieldValue, " where ID={ID} select * from PromoTran1 nolock where ID={ID}");
                    return sqlcmd.ExecuteEx<PromoTranRow>(sqlstr);
                }
                return op_exec(sqlcmd, row, json_s, args);
            }
        }

        PromoTranRow TranIn_upgrade(SqlCmd sqlcmd, PromoTranRow row)
        {
            try
            {
                sqlcmd.BeginTransaction();
                row = sqlcmd.ToObject<PromoTranRow>(@"
update PromoTran1 set LogType=LogType-10,ModifyTime=getdate(),ModifyUser={2} where ID='{0}' and LogType={1}
select * from PromoTran1 nolock where ID='{0}'", row.ID, (int)row.LogType.Value, (HttpContext.Current.User as User).ID);
                if (row == null)
                    throw new RowException(RowErrorCode.TranNotFound);
                sqlcmd.Commit();
                return row;
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
        }

        PromoTranRow op_exec(SqlCmd sqlcmd, PromoTranRow row, string json_s, params object[] args)
        {
            row.ModifyUser = _SystemUser.GetUser((HttpContext.Current.User as User).ID);
            try
            {
                if (row.LogType.In(promos2))
                {
                    if (this.op_Finish == 1) row = this.TranIn_upgrade(sqlcmd, row);
                    if (this.op_Delete == 1) this.TranIn_delete(sqlcmd, row);
                }
                else if (row.LogType.IsPromos())
                {
                    if (this.op_Finish == 1) { this.TranIn(sqlcmd, row); this.op_Delete = 1; }
                    if (this.op_Delete == 1) this.TranIn_delete(sqlcmd, row);
                }
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
            }
            return row;
        }
    }
}