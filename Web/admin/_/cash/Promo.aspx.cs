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
        class PromoSelect : jgrid.GridRequest
        {
            [JsonProperty("hist")]
            public bool? IsHistory;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class PromoUpdate
        {
            [JsonProperty]
            public int? ID;
            [JsonProperty]
            public PromoType? PromoType;
            [JsonProperty]
            public int? CorpID;
            [JsonProperty]
            public string MemberACNT;
            [JsonProperty]
            public decimal? Amount;
            [JsonProperty]
            public string RequestIP;
            [JsonProperty]
            public CurrencyCode? Currency;
            [JsonProperty]
            public PromoState? State;
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
        class PromoInsert : PromoUpdate { }

    }
    #endregion
    partial class _ExecuteCommand
    {
        [ObjectInvoke, api.Async]
        static object execute(PromoSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<PromoRow> data = new jgrid.GridResponse<PromoRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                string tableName = (command.IsHistory == true) ? "Promo2" : "Promo1";
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} a with(nolock) order by CreateTime desc", tableName))
                    data.rows.Add(r.ToObject<PromoRow>());
                return data;
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(PromoUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                sqltool s = new sqltool();
                PromoRow row;
                if (!sqlcmd.GetSingleRow(out row, "select * from Promo1 nolock where ID={0}", command.ID))
                    return jgrid.RowResponse.UpdateMissing();
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
                    string sqlstr = s.SqlExport(s.Build("update Promo1 set ", sqltool._FieldValue, " where ID={ID} select * from Promo1 nolock where ID={ID}"));
                    if (!sqlcmd.Execute(out row, out res, sqlstr))
                        return res;
                }
                return confirm(sqlcmd, row, command);
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse execute(PromoInsert command, string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*", "PromoType", " "] = command.PromoType;
            s["*", "CorpID", "    "] = command.CorpID;
            s["*", "MemberACNT", ""] = text.ValidAsACNT * command.MemberACNT;
            s["*", "Amount", "    "] = command.Amount;
            s[" ", "RequestIP", " "] = HttpContext.Current.RequestIP();
            s["N", "Memo1", "     "] = command.Memo1;
            s["N", "Memo2", "     "] = command.Memo2;
            s["N", "Memo3", "     "] = command.Memo3;
            s.SetUser(sqltool.ModifyUser);
            if (s.needs != null)
                return jgrid.RowResponse.FieldNeeds(s.needs);
            string sqlstr = s.SqlExport(s.Build("exec promo_proc @insert=1, @ID=0,", sqltool._AtFieldValue));
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                PromoRow row;
                jgrid.RowResponse res;
                if (!sqlcmd.Execute(out row, out res, sqlstr))
                    return res;
                if (row.err.HasValue)
                    return error(command, row);
                return confirm(sqlcmd, row, command);
            }
        }

        static jgrid.RowResponse error(PromoUpdate command, PromoRow row)
        {
            switch (row.err.Value)
            {
                case RowErrorCode.MemberNotExist: return jgrid.RowResponse.NotExist(command.CorpID, command.MemberACNT);
                case RowErrorCode.DepositNotExist: return jgrid.RowResponse.NotExist(command.CorpID, command.MemberACNT, command.Amount);
                default: return jgrid.RowResponse.Error(null);
            }
        }

        static jgrid.RowResponse confirm(SqlCmd sqlcmd, PromoRow row, PromoUpdate command)
        {
            AcceptOrReject? accept = null;
            if (command.Accept.HasValue && Enum.IsDefined(typeof(AcceptOrReject), command.Accept.Value))
                accept = command.Accept;

            if (accept.HasValue)
            {
                sqltool s = new sqltool();
                s[" ", "ID", "        "] = row.ID;
                s[" ", "PromoType", " "] = 0;
                s[" ", "CorpID", "    "] = 0;
                s[" ", "MemberACNT", ""] = 0;
                s[" ", "Amount", "    "] = 0;
                s[" ", "accept", "    "] = accept;
                s.SetUser(sqltool.ModifyUser);
                string sqlstr = s.SqlExport(s.Build("exec promo_proc @insert=0,", sqltool._AtFieldValue));

                jgrid.RowResponse res;
                try
                {
                    PromoRow row2 = null;
                    sqlcmd.BeginTransaction();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                        row2 = r.ToObject<PromoRow>();
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