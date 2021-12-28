using BU;
using BU.data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using web.protocol;

namespace web
{
    #region protocol
    namespace protocol
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class WithdrawalSelect : jgrid.GridRequest
        {
            [JsonProperty("hist")]
            public bool? IsHistory;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class WithdrawalUpdate
        {
            [JsonProperty]
            public int? ID;
            [JsonProperty]
            public int? CorpID;
            [JsonProperty]
            public string MemberACNT;
            [JsonProperty]
            public decimal? Amount;
            [JsonProperty]
            public int? BankCardID;
            [JsonProperty]
            public int? BankTranID;
            [JsonProperty]
            public int? PaymentID;
            [JsonProperty]
            public TranState? State;
            [JsonProperty]
            public DateTime? FinishTime;
            [JsonProperty]
            public string Memo1;
            [JsonProperty]
            public string Memo2;
            [JsonProperty]
            public string Memo3;

            [JsonProperty]
            public AcceptOrReject? Accept;
            [JsonProperty]
            public CommitOrRollback? Commit;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class WithdrawalInsert : WithdrawalUpdate { }
    }
    #endregion
    partial class _ExecuteCommand
    {
        [ObjectInvoke, api.Async]
        static object execute(WithdrawalSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<WithdrawalRow> data = new jgrid.GridResponse<WithdrawalRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                string tableName = (command.IsHistory == true) ? "Withdrawal2" : "Withdrawal1";
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} a with(nolock) order by CreateTime desc", tableName))
                    data.rows.Add(r.ToObject<WithdrawalRow>());
                return data;
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(WithdrawalUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                sqltool s = new sqltool();
                WithdrawalRow row;
                if (!sqlcmd.GetSingleRow(out row, "select * from Withdrawal1 nolock where ID={0}", command.ID))
                    return jgrid.RowResponse.UpdateMissing();
                s[" ", "BankCardID", "", row.BankCardID, "  "] = command.BankCardID;
                s[" ", "BankTranID", "", row.BankTranID, "  "] = command.BankTranID;
                s[" ", "PaymentID", " ", row.PaymentID, "   "] = command.PaymentID;
                s["N", "Memo1", "     ", row.Memo1, "       "] = command.Memo1;
                s["N", "Memo2", "     ", row.Memo2, "       "] = command.Memo2;
                s["N", "Memo3", "     ", row.Memo3, "       "] = command.Memo3;
                jgrid.RowResponse res;
                if (s.fields.Count == 0)
                    res = jgrid.RowResponse.UpdateIgnore(row);
                else
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["ID"] = row.ID;
                    string sqlstr = s.SqlExport(s.Build("update Withdrawal1 set ", sqltool._FieldValue, " where ID={ID} select * from Withdrawal1 nolock where ID={ID}"));
                    if (!sqlcmd.Execute(out row, out res, sqlstr))
                        return res;
                }
                return accept_commit(sqlcmd, row, command);
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(WithdrawalInsert command, string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*", "CorpID", "       "] = command.CorpID;
            s["*", "MemberACNT", "   "] = text.ValidAsACNT * command.MemberACNT;
            s["*", "Amount", "       "] = command.Amount;
            s[" ", "RequestIP", "    "] = HttpContext.Current.RequestIP();
            s[" ", "BankCardID", "   "] = command.BankCardID;
            s[" ", "BankTranID", "   "] = command.BankTranID;
            s[" ", "PaymentID", "    "] = command.PaymentID;
            s["N", "Memo1", "        "] = command.Memo1;
            s["N", "Memo2", "        "] = command.Memo2;
            s["N", "Memo3", "        "] = command.Memo3;
            s.SetUser(sqltool.ModifyUser);
            if (s.needs != null)
                return jgrid.RowResponse.FieldNeeds(s.needs);
            string sqlstr = s.SqlExport(s.Build("exec withdrawal_proc @insert=1, @ID=null,", sqltool._AtFieldValue));
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                WithdrawalRow row;
                jgrid.RowResponse res;
                if (!sqlcmd.Execute(out row, out res, sqlstr))
                    return res;
                if (row.err.HasValue)
                    return error(command, row);
                return accept_commit(sqlcmd, row, command);
            }
        }

        static jgrid.RowResponse error(WithdrawalUpdate command, WithdrawalRow row)
        {
            switch (row.err.Value)
            {
                case RowErrorCode.MemberNotExist: return jgrid.RowResponse.NotExist(command.CorpID, command.MemberACNT);
                case RowErrorCode.WithdrawalNotExist: return jgrid.RowResponse.NotExist(command.ID);
                case RowErrorCode.MemberBalanceNotEnought: return new jgrid.RowResponse(row.err.Value, command.CorpID, command.MemberACNT);
                default: return jgrid.RowResponse.Error(null);
            }
        }

        static jgrid.RowResponse accept_commit(SqlCmd sqlcmd, WithdrawalRow row, WithdrawalUpdate command)
        {
            AcceptOrReject? accept = null;
            CommitOrRollback? commit = null;
            if (command.Accept.HasValue && Enum.IsDefined(typeof(AcceptOrReject), command.Accept.Value))
                accept = command.Accept;
            if (command.Commit.HasValue && Enum.IsDefined(typeof(CommitOrRollback), command.Commit.Value))
                commit = command.Commit;
            if (accept.HasValue || commit.HasValue)
            {
                sqltool s = new sqltool();
                s[" ", "ID", ""] = row.ID;
                s[" ", "CorpID", ""] = 0;
                s[" ", "MemberACNT", ""] = 0;
                s[" ", "Amount", ""] = 0;
                s[" ", "accept", ""] = accept;
                s[" ", "commit", ""] = commit;
                s.SetUser(sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.SqlExport(s.Build("exec withdrawal_proc @insert=0,", sqltool._AtFieldValue));
                jgrid.RowResponse res;
                try
                {
                    WithdrawalRow row2 = null;
                    sqlcmd.BeginTransaction();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                        row2 = r.ToObject<WithdrawalRow>();
                    if (row2.err.HasValue)
                    {
                        sqlcmd.Rollback();
                        res = error(command, row);
                    }
                    else
                    {
                        sqlcmd.Commit();
                        row = row2;
                    }
                }
                catch (Exception ex)
                {
                    sqlcmd.Rollback();
                    log.message("error", ex.Message);
                    res = jgrid.RowResponse.Error(ex);
                }
            }
            return jgrid.RowResponse.Success(row);
        }

        // accept
        // confirm

        //static jgrid.RowResponse confirm(SqlCmd sqlcmd, DepositRow row, AcceptOrReject? op)
        //{

        //    if ((op == AcceptOrReject.Accept) || (op == AcceptOrReject.Reject))
        //    {
        //        sqltool s = new sqltool();
        //        s[" ", "ID", "     "] = row.ID;
        //        s[" ", "State", "  "] = op.Value == AcceptOrReject.Accept ? DepositState.Accepted : DepositState.Rejected;
        //        s[" ", "Confirm", ""] = op.Value == AcceptOrReject.Accept ? 1 : 0;
        //        s.SetUser(sqltool.ModifyUser);
        //        string sql = s.Build("exec deposit_proc @op='confirm',@CorpID=0,@MemberACNT='',@Amount=0,", sqltool._AtFieldValue);
        //        sql = s.SqlExport(sql);

        //        try
        //        {
        //            sqlcmd.BeginTransaction();
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
        //            {
        //                break;
        //            }
        //            sqlcmd.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            log.message("error", ex.Message);
        //            sqlcmd.Rollback();
        //        }
        //    }
        //    return jgrid.RowResponse.Success(row);
        //}
    }
}