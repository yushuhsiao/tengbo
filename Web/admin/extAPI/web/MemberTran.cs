using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Web;
using web;
using web.protocol;

namespace BU.data
{
    public abstract partial class MemberTranProtocol : TranProtocol<MemberTranRow>
    {
        [JsonProperty]
        public virtual string a_BankName { get; set; }
        [JsonProperty]
        public virtual string a_CardID { get; set; }
        [JsonProperty]
        public virtual string a_Name { get; set; }

        [JsonProperty]
        public virtual DateTime? a_TranTime { get; set; }
        [JsonProperty]
        public virtual string a_TranSerial { get; set; }
        [JsonProperty]
        public virtual string a_TranMemo { get; set; }

        [JsonProperty]
        public virtual string b_BankName { get; set; }
        [JsonProperty]
        public virtual string b_CardID { get; set; }
        [JsonProperty]
        public virtual string b_Name { get; set; }

        [JsonProperty]
        public virtual DateTime? b_TranTime { get; set; }
        [JsonProperty]
        public virtual string b_TranSerial { get; set; }
        [JsonProperty]
        public virtual string b_TranMemo { get; set; }

        [JsonProperty]
        public virtual string Memo1 { get; set; }
        [JsonProperty]
        public virtual string Memo2 { get; set; }

        protected override string _tableName { get { return "MemberTran1"; } }

        public override jgrid.RowResponse update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                MemberTranRow row = null;
                if (this.ID.HasValue)
                    row = sqlcmd.ToObject<MemberTranRow>("select * from MemberTran1 nolock where ID='{0}'", this.ID);
                if (row == null)
                    return jgrid.RowResponse.UpdateMissing();
                sqltool s = new sqltool();
                s["N", "a_BankName", "  ", row.a_BankName, "  "] = this.a_BankName;
                s[" ", "a_CardID", "    ", row.a_CardID, "    "] = this.a_CardID;
                s["N", "a_Name", "      ", row.a_Name, "      "] = this.a_Name;
                s[" ", "a_TranTime", "  ", row.a_TranTime, "  "] = this.a_TranTime;
                s[" ", "a_TranSerial", "", row.a_TranSerial, ""] = this.a_TranSerial;
                s["N", "a_TranMemo", "  ", row.a_TranMemo, "  "] = this.a_TranMemo;
                s["N", "b_BankName", "  ", row.b_BankName, "  "] = this.b_BankName;
                s[" ", "b_CardID", "    ", row.b_CardID, "    "] = this.b_CardID;
                s["N", "b_Name", "      ", row.b_Name, "      "] = this.b_Name;
                s[" ", "b_TranTime", "  ", row.b_TranTime, "  "] = this.b_TranTime;
                s[" ", "b_TranSerial", "", row.b_TranSerial, ""] = this.b_TranSerial;
                s["N", "b_TranMemo", "  ", row.b_TranMemo, "  "] = this.b_TranMemo;
                s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1;
                s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["ID"] = row.ID;
                    string sqlstr = s.SqlExport(s.Build("update MemberTran1 set ", sqltool._FieldValue, " where ID={ID} select * from MemberTran1 nolock where ID={ID}"));
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
            if ((s.Values["prefix"] = this.LogType.GetPrefix(BU.LogType.Deposit, BU.LogType.Withdrawal)) == null)
                this.LogType = null;
            s["*", "LogType", "         "] = this.LogType;
            s["*", "GameID", "          "] = 0;
            s["*", "MemberID", "        "] = (StringEx.sql_str)"ID";
            s["*", "CorpID", "          "] = this.CorpID;
            s["*", "MemberACNT", "      "] = this.MemberACNT *= text.ValidAsACNT;
            s["*", "Amount1", "         "] = this.Amount1;
            s[" ", "Amount2", "         "] = 0;
            s[" ", "State", "           "] = TranState.Initial;
            s[" ", "CurrencyA", "       "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyB", "       "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyX", "       "] = 1;
            s[" ", "SerialNumber", "    "] = (StringEx.sql_str)"@SerialNumber";
            s[" ", "RequestIP", "       "] = HttpContext.Current.RequestIP();
            s["N", "a_BankName", "      "] = this.a_BankName;
            s[" ", "a_CardID", "        "] = this.a_CardID;
            s["N", "a_Name", "          "] = this.a_Name;
            s[" ", "a_TranTime", "      "] = this.a_TranTime;
            s[" ", "a_TranSerial", "    "] = this.a_TranSerial;
            s["N", "a_TranMemo", "      "] = this.a_TranMemo;
            s["N", "b_BankName", "      "] = this.b_BankName;
            s[" ", "b_CardID", "        "] = this.b_CardID;
            s["N", "b_Name", "          "] = this.b_Name;
            s[" ", "b_TranTime", "      "] = this.b_TranTime;
            s[" ", "b_TranSerial", "    "] = this.b_TranSerial;
            s["N", "b_TranMemo", "      "] = this.b_TranMemo;
            s["N", "Memo1", "           "] = this.Memo1;
            s["N", "Memo2", "           "] = this.Memo2;
            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
            if (s.needs != null)
                return jgrid.RowResponse.FieldNeeds(s.needs);
            s.Values["CorpID"] = (StringEx.sql_str)"CorpID";
            s.Values["MemberACNT"] = (StringEx.sql_str)"ACNT";
            s.Values["CorpID_"] = this.CorpID;
            s.Values["ACNT_"] = this.MemberACNT;

            string sqlstr = s.SqlExport(s.Build(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into MemberTran1 (ID, ", sqltool._Fields, @")
select @ID,", sqltool._Values, @"n from Member nolock where CorpID={CorpID_} and ACNT={ACNT_}
select * from MemberTran1 nolock where ID=@ID"));
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                jgrid.RowResponse res;
                MemberTranRow row;
                if (!sqlcmd.Execute(out row, out res, sqlstr))
                    return res;
                return this.process_tran(sqlcmd, row);
            }
        }

        jgrid.RowResponse process_tran(SqlCmd sqlcmd, MemberTranRow row)
        {
            this._TranValidOp();
            try
            {
                if ((row.LogType == BU.LogType.Deposit) && this.commit.HasValue)
                    this._TranIn(sqlcmd, row);
                else if ((row.LogType == BU.LogType.Withdrawal) && (this.accept.HasValue || this.commit.HasValue))
                {
                    if (this.accept == AcceptOrReject.Accept)
                        this._TranOut_accept(sqlcmd, row);
                    this._TranOut(sqlcmd, row, BU.LogType.WithdrawalRollback);
                }
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
                jgrid.RowResponse res2 = jgrid.RowResponse.Error(ex);
            }
            return jgrid.RowResponse.Success(row);
        }

        protected override void _TranFinish(SqlCmd sqlcmd, MemberTranRow row, string amount2)
        {
            sqlcmd.ExecuteNonQuery(@"
insert into MemberTran2(ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,Amount2,State,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP,FinishTime,CreateTime,CreateUser,ModifyTime,ModifyUser,a_BankName,a_CardID,a_Name,a_TranTime,a_TranSerial,a_TranMemo,b_BankName,b_CardID,b_Name,b_TranTime,b_TranSerial,b_TranMemo,Memo1,Memo2)
select					ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,{3}    ,{1}  ,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP,getdate() ,CreateTime,CreateUser,getdate() ,{2}       ,a_BankName,a_CardID,a_Name,a_TranTime,a_TranSerial,a_TranMemo,b_BankName,b_CardID,b_Name,b_TranTime,b_TranSerial,b_TranMemo,Memo1,Memo2
from MemberTran1 nolock where ID='{0}' delete MemberTran1 where ID='{0}'", row.ID, (int)row.State.Value, row.ModifyUser, amount2);
        }
    }
}