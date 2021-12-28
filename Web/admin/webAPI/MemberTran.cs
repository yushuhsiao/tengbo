using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberTranRow : TranRow
    {
        [DbImport, JsonProperty]
        public string a_BankName;           // 銀行卡資訊(公司)
        [DbImport, JsonProperty]            //   存款時為系統分配給玩家的銀行卡    
        public string a_CardID;             //   提款時為財務匯款給玩家所使用的銀行卡
        [DbImport, JsonProperty]            //   第三方轉帳不使用
        public string a_Name;

        [DbImport, JsonProperty]
        public DateTime? a_TranTime;        // 交易資訊(公司)
        [DbImport, JsonProperty]            //   存款時為財務核對時的銀行卡資訊    
        public string a_TranSerial;         //   提款時為財務匯款完成之後輸入匯款資訊
        [DbImport, JsonProperty]            //   第三方轉帳不使用
        public string a_TranMemo;

        [DbImport, JsonProperty]
        public string b_BankName;           // 銀行卡資訊(客戶)
        [DbImport, JsonProperty]            //   存款時為玩家輸入的銀行卡
        public string b_CardID;             //   提款時為玩家在系統登記的銀行卡
        [DbImport, JsonProperty]            //   第三方轉帳不使用
        public string b_Name;

        [DbImport, JsonProperty]
        public DateTime? b_TranTime;        // 交易資訊(客戶)
        [DbImport, JsonProperty]            //   存款時為玩家匯款完成之後輸入的資訊
        public string b_TranSerial;         //   提款時不使用
        [DbImport, JsonProperty]            //   第三方轉帳不使用
        public string b_TranMemo;

        [DbImport, JsonProperty]
        public string Memo1;                // 客服備註
        [DbImport, JsonProperty]
        public string Memo2;                // 帳務備註
    }

    public partial class MemberTranRowCommand : TranRowCommand<MemberTranRow>
    {
        [JsonProperty]
        public virtual string a_BankName { get; set; }
        [JsonProperty]
        public virtual string a_CardID { get; set; }
        [JsonProperty]
        public virtual string a_Name { get; set; }

        [JsonProperty]
        public virtual DateTime? a_TranTime { get; set; }
        public bool a_TranTime_GetDate;
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
        public bool b_TranTime_GetDate;
        [JsonProperty]
        public virtual string b_TranSerial { get; set; }
        [JsonProperty]
        public virtual string b_TranMemo { get; set; }

        [JsonProperty]
        public virtual string Memo1 { get; set; }
        [JsonProperty]
        public virtual string Memo2 { get; set; }

        public bool VerifyMemberID;

        protected override string TableName { get { return "MemberTran"; } }

//        public MemberTranRow update(string json_s, params object[] args)
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                MemberTranRow row = sqlcmd.GetRowEx<MemberTranRow>(RowErrorCode.MemberTranNotFound, "select * from MemberTran1 nolock where ID='{0}'", this.ID);
//                sqltool s = new sqltool();
//                object a_TranTime = this.a_TranTime; if (this.a_TranTime_GetDate) a_TranTime = (StringEx.sql_str)"getdate()";
//                object b_TranTime = this.b_TranTime; if (this.b_TranTime_GetDate) b_TranTime = (StringEx.sql_str)"getdate()";
//                if (VerifyMemberID)
//                    if (row.MemberID != (HttpContext.Current.User as User).ID)
//                        return row;

//                s["N", "a_BankName", "  ", row.a_BankName, "  "] = this.a_BankName;
//                s[" ", "a_CardID", "    ", row.a_CardID, "    "] = this.a_CardID;
//                s["N", "a_Name", "      ", row.a_Name, "      "] = this.a_Name;
//                s[" ", "a_TranTime", "  ", row.a_TranTime, "  "] = a_TranTime;
//                s[" ", "a_TranSerial", "", row.a_TranSerial, ""] = this.a_TranSerial;
//                s["N", "a_TranMemo", "  ", row.a_TranMemo, "  "] = this.a_TranMemo;
//                s["N", "b_BankName", "  ", row.b_BankName, "  "] = this.b_BankName;
//                s[" ", "b_CardID", "    ", row.b_CardID, "    "] = this.b_CardID;
//                s["N", "b_Name", "      ", row.b_Name, "      "] = this.b_Name;
//                s[" ", "b_TranTime", "  ", row.b_TranTime, "  "] = b_TranTime;
//                s[" ", "b_TranSerial", "", row.b_TranSerial, ""] = this.b_TranSerial;
//                s["N", "b_TranMemo", "  ", row.b_TranMemo, "  "] = this.b_TranMemo;
//                s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1;
//                s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2;
//                if (s.fields.Count > 0)
//                {
//                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                    s.Values["ID"] = row.ID;
//                    string sqlstr = s.BuildEx("update MemberTran1 set ", sqltool._FieldValue, " where ID={ID} select * from MemberTran1 nolock where ID={ID}");
//                    row = sqlcmd.ExecuteEx<MemberTranRow>(sqlstr);
//                }
//                return this.process_tran(sqlcmd, row);
//            }
//        }

//        public MemberTranRow insert(string json_s, params object[] args)
//        {
//            if (this.Amount1 <= 0) this.Amount1 = null;
//            sqltool s = new sqltool();
//            if ((s.Values["prefix"] = this.LogType.GetPrefix(BU.LogType.Deposit, BU.LogType.Withdrawal, BU.LogType.Dinpay, BU.LogType.Alipay)) == null)
//                this.LogType = null;
//            object
//                memberID = this.MemberID,
//                corpID = this.CorpID,
//                acnt = (this.MemberACNT *= text.ValidAsACNT);
//            string sql2;
//            if (this.MemberID.HasValue)
//            {
//                corpID = (StringEx.sql_str)"CorpID";
//                acnt = (StringEx.sql_str)"ACNT";
//                sql2 = "ID={MemberID}";
//            }
//            else
//            {
//                memberID = (StringEx.sql_str)"ID";
//                sql2 = "CorpID={CorpID_} and ACNT={ACNT_}";
//            }
//            s["*", "LogType", "         "] = this.LogType;
//            s["*", "GameID", "          "] = 0;
//            s["*", "MemberID", "        "] = memberID;
//            s["*", "CorpID", "          "] = corpID;
//            s["*", "MemberACNT", "      "] = acnt;
//            s["*", "Amount1", "         "] = this.Amount1;
//            s[" ", "Amount2", "         "] = 0;
//            s[" ", "State", "           "] = TranState.Initial;
//            s[" ", "CurrencyA", "       "] = (StringEx.sql_str)"Currency";
//            s[" ", "CurrencyB", "       "] = (StringEx.sql_str)"Currency";
//            s[" ", "CurrencyX", "       "] = 1;
//            s[" ", "SerialNumber", "    "] = (StringEx.sql_str)"@SerialNumber";
//            s[" ", "RequestIP", "       "] = HttpContext.Current.RequestIP();
//            s["N", "a_BankName", "      "] = this.a_BankName;
//            s[" ", "a_CardID", "        "] = this.a_CardID;
//            s["N", "a_Name", "          "] = this.a_Name;
//            s[" ", "a_TranTime", "      "] = this.a_TranTime;
//            s[" ", "a_TranSerial", "    "] = this.a_TranSerial;
//            s["N", "a_TranMemo", "      "] = this.a_TranMemo;
//            s["N", "b_BankName", "      "] = this.b_BankName;
//            s[" ", "b_CardID", "        "] = this.b_CardID;
//            s["N", "b_Name", "          "] = this.b_Name;
//            s[" ", "b_TranTime", "      "] = this.b_TranTime;
//            s[" ", "b_TranSerial", "    "] = this.b_TranSerial;
//            s["N", "b_TranMemo", "      "] = this.b_TranMemo;
//            s["N", "Memo1", "           "] = this.Memo1;
//            s["N", "Memo2", "           "] = this.Memo2;
//            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
//            s.TestFieldNeeds();
//            if (!this.MemberID.HasValue)
//            {
//                s.Values["CorpID"] = (StringEx.sql_str)"CorpID";
//                s.Values["MemberACNT"] = (StringEx.sql_str)"ACNT";
//                s.Values["CorpID_"] = this.CorpID;
//                s.Values["ACNT_"] = this.MemberACNT;
//            }

//            string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
//insert into MemberTran1 (ID, ", sqltool._Fields, @")
//select @ID,", sqltool._Values, " from Member nolock where ", sql2, @"
//select * from MemberTran1 nolock where ID=@ID");
//            SqlCmd sqlcmd;
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//            {
//                MemberTranRow row = sqlcmd.ExecuteEx<MemberTranRow>(sqlstr);
//                return this.process_tran(sqlcmd, row);
//            }
//        }

//        MemberTranRow process_tran(SqlCmd sqlcmd, MemberTranRow row)
//        {
//            this._TranValidOp();
//            try
//            {
//                if ((row.LogType.In(BU.LogType.Deposit, BU.LogType.Dinpay, BU.LogType.Alipay)) && this.commit.HasValue)
//                    this._TranIn(sqlcmd, row);
//                else if ((row.LogType == BU.LogType.Withdrawal) && (this.accept.HasValue || this.commit.HasValue))
//                {
//                    if (this.accept == AcceptOrReject.Accept)
//                        this._TranOut_accept(sqlcmd, row);
//                    this._TranOut(sqlcmd, row, BU.LogType.WithdrawalRollback);
//                }
//            }
//            catch (Exception ex)
//            {
//                log.error_msg(ex);
//                //jgrid.RowResponse res2 = jgrid.RowResponse.Error(ex);
//            }
//            return row;
//        }

//        protected override void _TranFinish(SqlCmd sqlcmd, MemberTranRow row, string amount2)
//        {
//            sqlcmd.ExecuteNonQuery(@"
//insert into MemberTran2(ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,Amount2,State,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP,FinishTime,CreateTime,CreateUser,ModifyTime,ModifyUser,a_BankName,a_CardID,a_Name,a_TranTime,a_TranSerial,a_TranMemo,b_BankName,b_CardID,b_Name,b_TranTime,b_TranSerial,b_TranMemo,Memo1,Memo2)
//select					ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,{3}    ,{1}  ,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP,getdate() ,CreateTime,CreateUser,getdate() ,{2}       ,a_BankName,a_CardID,a_Name,a_TranTime,a_TranSerial,a_TranMemo,b_BankName,b_CardID,b_Name,b_TranTime,b_TranSerial,b_TranMemo,Memo1,Memo2
//from MemberTran1 nolock where ID='{0}' delete MemberTran1 where ID='{0}'", row.ID, (int)row.State.Value, row.ModifyUser, amount2);
//        }

        public override MemberTranRow Insert(string json_s, params object[] args)
        {
            if (this.Amount1 <= 0) this.Amount1 = null;
            sqltool s = new sqltool();
            if ((s.Values["prefix"] = this.LogType.GetPrefix(text.MemberDepositLogTypes) ?? this.LogType.GetPrefix(text.MemberWithdrawalLogTypes)) == null)
                this.LogType = null;
            object
                memberID = this.MemberID,
                corpID = this.CorpID,
                acnt = (this.MemberACNT *= text.ValidAsACNT);
            string sql2;
            if (this.MemberID.HasValue)
            {
                corpID = (StringEx.sql_str)"a.CorpID";
                acnt = (StringEx.sql_str)"a.ACNT";
                sql2 = "a.ID={MemberID}";
            }
            else
            {
                memberID = (StringEx.sql_str)"a.ID";
                sql2 = "a.CorpID={CorpID_} and a.ACNT={ACNT_}";
            }
            s["*", "LogType", "         "] = this.LogType;
            s["*", "GameID", "          "] = 0;
            s["*", "CorpID", "          "] = corpID;
            s["*", "MemberID", "        "] = memberID;
            s["*", "MemberACNT", "      "] = acnt;
            s["*", "AgentID", "         "] = (StringEx.sql_str)"b.ID";
            s["*", "AgentACNT", "       "] = (StringEx.sql_str)"b.ACNT";
            s["*", "Amount1", "         "] = this.Amount1;
            s[" ", "Amount2", "         "] = this.Amount2 ?? 0; //this.LogType == BU.LogType.Deposit ? this.Amount1 * 0.01m : 0; // 存款時自動添加手續費 1%
            s[" ", "State", "           "] = TranState.Initial;
            s[" ", "CurrencyA", "       "] = (StringEx.sql_str)"a.Currency";
            s[" ", "CurrencyB", "       "] = (StringEx.sql_str)"a.Currency";
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
            s.TestFieldNeeds();
            if (!this.MemberID.HasValue)
            {
                s.Values["CorpID"] = (StringEx.sql_str)"a.CorpID";
                s.Values["MemberACNT"] = (StringEx.sql_str)"a.ACNT";
                s.Values["CorpID_"] = this.CorpID;
                s.Values["ACNT_"] = this.MemberACNT;
            }

            string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into MemberTran1 (ID,", sqltool._Fields, @")
select @ID,", sqltool._Values, " from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID where ", sql2, @"
select * from MemberTran1 nolock where ID=@ID");
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            {
                MemberTranRow row = sqlcmd.ExecuteEx<MemberTranRow>(sqlstr);
                this.ID = row.ID;
                return op_exec(sqlcmd, row, json_s, args);
            }
        }

        protected virtual void OnGetRow(SqlCmd sqlcmd, MemberTranRow row) { }

        public override MemberTranRow Update(string json_s, params object[] args)
        {
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            {
                MemberTranRow row = sqlcmd.GetRowEx<MemberTranRow>(RowErrorCode.MemberTranNotFound, "select * from MemberTran1 nolock where ID='{0}'", this.ID);
                this.OnGetRow(sqlcmd, row);
                sqltool s = new sqltool();
                object a_TranTime = this.a_TranTime; if (this.a_TranTime_GetDate) a_TranTime = StringEx.sql_str.getdate;
                object b_TranTime = this.b_TranTime; if (this.b_TranTime_GetDate) b_TranTime = StringEx.sql_str.getdate;
                if (VerifyMemberID)
                    if (row.MemberID != (HttpContext.Current.User as User).ID)
                        return row;

                s[" ", "Amount2", "     ", row.Amount2, "     "] = this.Amount2;
                s["N", "a_BankName", "  ", row.a_BankName, "  "] = this.a_BankName;
                s[" ", "a_CardID", "    ", row.a_CardID, "    "] = this.a_CardID;
                s["N", "a_Name", "      ", row.a_Name, "      "] = this.a_Name;
                s[" ", "a_TranTime", "  ", row.a_TranTime, "  "] = a_TranTime;
                s[" ", "a_TranSerial", "", row.a_TranSerial, ""] = this.a_TranSerial;
                s["N", "a_TranMemo", "  ", row.a_TranMemo, "  "] = this.a_TranMemo;
                s["N", "b_BankName", "  ", row.b_BankName, "  "] = this.b_BankName;
                s[" ", "b_CardID", "    ", row.b_CardID, "    "] = this.b_CardID;
                s["N", "b_Name", "      ", row.b_Name, "      "] = this.b_Name;
                s[" ", "b_TranTime", "  ", row.b_TranTime, "  "] = b_TranTime;
                s[" ", "b_TranSerial", "", row.b_TranSerial, ""] = this.b_TranSerial;
                s["N", "b_TranMemo", "  ", row.b_TranMemo, "  "] = this.b_TranMemo;
                s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1;
                s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["ID"] = row.ID;
                    string sqlstr = s.BuildEx("update MemberTran1 set ", sqltool._FieldValue, " where ID={ID} select * from MemberTran1 nolock where ID={ID}");
                    row = sqlcmd.ExecuteEx<MemberTranRow>(sqlstr);
                }
                return op_exec(sqlcmd, row, json_s, args);
            }
        }

        //public static BU.LogType[] LogTypes_Deposit = new BU.LogType[] { BU.LogType.Deposit, BU.LogType.Dinpay, BU.LogType.Alipay, };
        //public static BU.LogType[] LogTypes_Withdrawal = new BU.LogType[] { BU.LogType.Withdrawal, };

        protected override LogType? LogType_Rollback
        {
            get { return BU.LogType.WithdrawalRollback; }
        }

        MemberTranRow op_exec(SqlCmd sqlcmd, MemberTranRow row, string json_s, params object[] args)
        {
            row.ModifyUser = _SystemUser.GetUser((HttpContext.Current.User as User).ID);
            try
            {
                if (row.LogType.In(text.MemberWithdrawalLogTypes))
                {
                    if (this.op_Accept == 1) this.TranOut(sqlcmd, row);
                    if (this.op_Finish == 1) this.TranOut_confirm(sqlcmd, row);
                    if (this.op_Delete == 1) this.TranOut_delete(sqlcmd, row);
                }
                else if (row.LogType.In(text.MemberDepositLogTypes))
                {
                    if (this.op_Finish == 1) this.TranIn(sqlcmd, row);
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