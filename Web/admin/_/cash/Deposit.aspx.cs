using BU;
using BU.data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using Tools.Protocol;
using web.data;
using web.protocol;

namespace web
{
    #region protocol
    namespace protocol
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class DepositSelect : jgrid.GridRequest
        {
            [JsonProperty("hist")]
            public bool? IsHistory;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class DepositUpdate
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
            public decimal? RequestAmount;
            [JsonProperty]
            public int? BankCardID;
            [JsonProperty]
            public int? BankTranID;
            [JsonProperty]
            public int? PaymentID;
            [JsonProperty]
            public DepositState? State;
            [JsonProperty]
            public string Memo1;
            [JsonProperty]
            public string Memo2;
            [JsonProperty]
            public string Memo3;

            [JsonProperty]
            public AcceptOrReject? Accept;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class DepositInsert : DepositUpdate { }

    }
    #endregion
    partial class _ExecuteCommand
    {
        [ObjectInvoke, api.Async]
        static object execute(DepositSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<DepositRow> data = new jgrid.GridResponse<DepositRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                string tableName = (command.IsHistory == true) ? "Deposit2" : "Deposit1";
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} a with(nolock) order by CreateTime desc", tableName))
                    data.rows.Add(r.ToObject<DepositRow>());
                return data;
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(DepositUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                DepositRow row;
                if (!sqlcmd.GetSingleRow(out row, "select * from Deposit1 nolock where ID={0}", command.ID))
                    return jgrid.RowResponse.UpdateMissing();
                sqltool s = new sqltool();
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
                    string sqlstr = s.SqlExport(s.Build("update Deposit1 set ", sqltool._FieldValue, " where ID={ID} select * from Deposit1 nolock where ID={ID}"));
                    if (!sqlcmd.Execute(out row, out res, sqlstr))
                        return res;
                }
                return confirm(sqlcmd, row, command);
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(DepositInsert command, string json_s, params object[] args)
        {
            decimal? amount = command.Amount, requestAmount = command.RequestAmount;
            if (!amount.HasValue && requestAmount.HasValue)
                amount = requestAmount.Value + (decimal)RandomValue.RNG.GetInt32(100) / 100m;

            sqltool s = new sqltool();
            s["*", "CorpID", "       "] = command.CorpID;
            s["*", "MemberACNT", "   "] = text.ValidAsACNT * command.MemberACNT;
            s["*", "Amount", "       "] = amount;
            s[" ", "RequestAmount", ""] = requestAmount;
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
            string sqlstr = s.SqlExport(s.Build("exec deposit_proc @insert=1, @ID=0,", sqltool._AtFieldValue));
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                DepositRow row;
                jgrid.RowResponse res;
                if (!sqlcmd.Execute(out row, out res, sqlstr))
                    return res;
                if (row.err.HasValue)
                    return error(command, row);
                return confirm(sqlcmd, row, command);
            }
        }

        static jgrid.RowResponse error(DepositUpdate command, DepositRow row)
        {
            switch (row.err.Value)
            {
                case RowErrorCode.MemberNotExist: return jgrid.RowResponse.NotExist(command.CorpID, command.MemberACNT);
                case RowErrorCode.DepositNotExist: return jgrid.RowResponse.NotExist(command.CorpID, command.MemberACNT, command.Amount);
                default: return jgrid.RowResponse.Error(null);
            }
        }

        static jgrid.RowResponse confirm(SqlCmd sqlcmd, DepositRow row, DepositUpdate command)
        {
            AcceptOrReject? accept = null;
            if (command.Accept.HasValue && Enum.IsDefined(typeof(AcceptOrReject), command.Accept.Value))
                accept = command.Accept;

            if (accept.HasValue)
            {
                sqltool s = new sqltool();
                s[" ", "ID", ""] = row.ID;
                s[" ", "CorpID", ""] = 0;
                s[" ", "MemberACNT", ""] = 0;
                s[" ", "Amount", ""] = 0;
                s[" ", "accept", ""] = accept;
                s.SetUser(sqltool.ModifyUser);
                string sql = s.Build("exec deposit_proc @insert=0,", sqltool._AtFieldValue);
                sql = s.SqlExport(sql);

                jgrid.RowResponse res;
                try
                {
                    DepositRow row2 = null;
                    sqlcmd.BeginTransaction();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
                        row2 = r.ToObject<DepositRow>();
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
    }
}