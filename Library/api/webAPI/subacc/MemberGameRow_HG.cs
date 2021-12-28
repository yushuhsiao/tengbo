using BU;
using extAPI.hg;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using web;

namespace web
{
    public class MemberGame_HG : MemberGame<MemberGame_HG, MemberGame_HG.Row, MemberGame_HG.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow
        {
            [DbImport, JsonProperty]
            public extAPI.hg.LoginMode? mode;
            [DbImport, JsonProperty]
            public string firstname;
            [DbImport, JsonProperty]
            public string lastname;
            [DbImport("currencyid"), JsonProperty]
            public string currencyid;
            [DbImport("currencyid"), JsonProperty]
            public override string Currency
            {
                get { return base.Currency; }
                set { base.Currency = value; }
            }
            [DbImport, JsonProperty]
            public int? agentid;
            [DbImport, JsonProperty]
            public string affiliateid;
            [DbImport, JsonProperty]
            public int? playerlevel;
            [DbImport, JsonProperty]
            public extAPI.hg.TestUser? testusr;

            public string ticket;

            public extAPI.hg.api hgapi;
            public extAPI.hg.api get_api(SqlCmd sqlcmd)
            {
                if (this.hgapi == null)
                    this.hgapi = extAPI.hg.api.GetInstance(this.GetCorpID(sqlcmd));
                return this.hgapi;
            }
        }
        public class RowCommand : MemberGameRowCommand
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_HG.Instance; } } 

            [JsonProperty]
            public extAPI.hg.LoginMode? mode;
            [JsonProperty]
            public string firstname;
            [JsonProperty]
            public string lastname;
            [JsonProperty]
            public int? agentid;
            [JsonProperty]
            public string affiliateid;
            [JsonProperty]
            public int? playerlevel;
            [JsonProperty]
            public extAPI.hg.TestUser? testusr;
        }

        public override GameID GameID { get { return BU.GameID.HG; } }
        public override string TableName { get { return "Member_001"; } }

        const int reg_id_max = 10;
        public Row Register(SqlCmd sqlcmd, int? memberID, int? corpID)
        {
            extAPI.hg.api hgapi = extAPI.hg.api.GetInstance(corpID);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", memberID.Value);
                sqltool s = new sqltool();
                //string acnt1 = string.Format("{0}{1}", hgapi.prefix, member.ACNT);
                s["", "MemberID", "   "] = member.ID;
                s["", "GameID", "     "] = this.GameID;
                s["", "Locked", "     "] = 2;
                s["", "Balance", "    "] = 0;
                s["", "mode", "       "] = (int)(member.GroupID >= 1 ? extAPI.hg.LoginMode.真正 : extAPI.hg.LoginMode.娛樂);
                s["", "firstname", "  "] = member.ACNT;
                s["", "lastname", "   "] = hgapi.prefix ?? "";
                s["", "currencyid", " "] = member.Currency;
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;

                //const string sql_select = " select * from {MemberTable} nolock";
                //const string sql_where = " where GameID={GameID} and MemberID={MemberID}";

                for (int id = 0; id < reg_id_max; id++)
                {
                    Row row;
                    try
                    {
                        s["", "ACNT", ""] = string.Format("{0}{1}{2}", hgapi.prefix, member.ACNT, id == 0 ? "" : id.ToString());
                        sqlcmd.BeginTransaction();
                        row = sqlcmd.ToObject<Row>(s.BuildEx(
                            "delete {MemberTable} where GameID={GameID} and MemberID={MemberID} and Locked=2",
                            " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                        if (row == null)
                        {
                            int cnt = sqlcmd.ExecuteScalar<int>(s.BuildEx("select count(*) from {MemberTable} nolock where GameID={GameID} and ACNT={ACNT}")) ?? 0;
                            if (cnt == 0)
                                row = sqlcmd.ToObject<Row>(s.BuildEx(
                                    "insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, ")",
                                    " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                            sqlcmd.Commit();
                            if (row == null)
                                continue;
                        }
                        else
                        {
                            sqlcmd.Commit();
                            return row;
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlcmd.Rollback();
                        log.error(ex);
                        continue;
                    }
                    hgResponse1 res_a = hgapi.accountbalance(row.ACNT, row.mode.Value);
                    if (res_a.status == StatusCode.ERR_WITH_USER)
                    {
                        hgResponse1 res_b = hgapi.registration(row.ACNT, row.Password, row.mode.Value, row.firstname, row.lastname, row.currencyid, row.agentid, row.affiliateid, row.testusr, row.playerlevel);
                        if (res_b.status == StatusCode.SUCCESS)
                        {
                            row = sqlcmd.ToObject<Row>(true, s.BuildEx(
                                "update {MemberTable} set Locked=0 where GameID={GameID} and MemberID={MemberID}",
                                " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                            row.ticket = res_b.ticket;
                            return row;
                        }
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);

                //for (int id = 0; id < reg_id_max; )
                //{
                //    Row row;
                //    try
                //    {
                //        sqlcmd.BeginTransaction();
                //        row = sqlcmd.ToObject<Row>(s.BuildEx(@"delete {MemberTable}", sql_where, " and Locked=2", sql_select, sql_where));
                //        if (row != null) { sqlcmd.Commit(); return row; }
                //        bool ok = false;
                //        for (; id < reg_id_max; id++)
                //        {
                //            s["", "ACNT", ""] = string.Format("{0}{1}{2}", hgapi.prefix, member.ACNT, id == 0 ? null : (int?)id);
                //            int cnt = sqlcmd.ExecuteScalar<int>(s.BuildEx("select count(*) from {MemberTable} nolock where GameID={GameID} and ACNT={ACNT}")) ?? 0;
                //            if (ok = cnt == 0)
                //                break;
                //        }
                //        if (!ok) throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
                //        row = sqlcmd.ToObject<Row>(s.BuildEx(@"insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, ")", sql_select, sql_where));
                //        sqlcmd.Commit();
                //    }
                //    catch { sqlcmd.Rollback(); throw; }
                //    hgResponse1 res_a = hgapi.accountbalance(row.ACNT, row.mode.Value);
                //    if (res_a.status == StatusCode.ERR_WITH_USER)
                //    {
                //        hgResponse1 res_b = hgapi.registration(row.ACNT, row.Password, row.mode.Value, row.firstname, row.lastname, row.currencyid, row.agentid, row.affiliateid, row.testusr, row.playerlevel);
                //        if (res_b.status == StatusCode.SUCCESS)
                //        {
                //            try
                //            {
                //                sqlcmd.BeginTransaction();
                //                sqlcmd.ExecuteNonQuery(s.BuildEx(@"update {MemberTable} set Locked=0", sql_where));
                //                sqlcmd.Commit();
                //                row.ticket = res_b.ticket;
                //                return row;
                //            }
                //            catch { sqlcmd.Rollback(); throw; }
                //        }
                //    }
                //}
                //return null;
            }
        }

        public Row Login(SqlCmd sqlcmd, int? memberID, int? corpID)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                Row row = this.OnSelectRow(sqlcmd, memberID, false);
                if (row == null)
                    return this.Register(sqlcmd, memberID, corpID);
                if (row.Locked == BU.Locked.Active)
                {
                    row.CorpID = corpID;

                    hgResponse1 res1 = row.get_api(sqlcmd).accountbalance(row.ACNT, row.mode.Value);
                    if (res1.status == StatusCode.ERR_WITH_USER)
                        return this.Register(sqlcmd, memberID, corpID);

                    hgResponse1 res2 = row.get_api(sqlcmd).registration(row.ACNT, row.Password, row.mode.Value, row.firstname, row.lastname, row.currencyid, row.agentid, row.affiliateid, row.testusr, row.playerlevel);
                    if (res2.status == StatusCode.SUCCESS)
                        row.ticket = res2.ticket;
                    return row;
                }
                else throw new RowException(BU.RowErrorCode.AccountLocked);
            }
        }

        //protected override decimal? OnGetBalance(SqlCmd sqlcmd, MemberGame_HG.Row row)
        //{
        //    if (row != null)
        //    {
        //        hgResponse1 res1 = row.get_api(sqlcmd).accountbalance(row.ACNT, row.mode.Value);
        //        if (res1.status == StatusCode.SUCCESS)
        //            return res1.balance;
        //    }
        //    return null;
        //}

        internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            try
            {
                Row row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false) ?? this.Register(sqlcmd, tran.MemberID, tran.CorpID);
                if (row != null)
                {
                    command.TranOut(sqlcmd, tran);
                    if (tran.Amount < 0)
                    {
                        row.CorpID = tran.CorpID;
                        extAPI.hg.hgResponse1 res1 = row.get_api(sqlcmd).deposit(row.ACNT, row.mode.Value, row.currencyid, -tran.Amount.Value, tran.SerialNumber, null, null);
                        if (res1.status == extAPI.hg.StatusCode.SUCCESS)
                        {
                            extAPI.hg.hgResponse1 res2 = row.get_api(sqlcmd).deposit_confirm(extAPI.hg.StatusCode.SUCCESS, res1.paymentid, res1.errdesc);
                            command.TranOut_confirm(sqlcmd, tran);
                            try { tran.MemberGameRow = this.GetBalance(sqlcmd, row); }
                            catch { }
                        }
                    }
                }
                return tran;
            }
            finally
            {
                command.TranOut_delete(sqlcmd, tran);
            }
        }

        internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            try
            {
                Row row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false);
                if (row != null)
                {
                    row.CorpID = tran.CorpID;
                    extAPI.hg.hgResponse1 res1 = row.get_api(sqlcmd).withdrawal(row.ACNT, row.mode.Value, row.currencyid, tran.Amount1.Value, tran.SerialNumber, null);
                    if (res1.status == extAPI.hg.StatusCode.SUCCESS)
                    {
                        extAPI.hg.hgResponse1 res2 = row.get_api(sqlcmd).withdrawal_confirm(extAPI.hg.StatusCode.SUCCESS, res1.paymentid, res1.errdesc);
                        command.TranIn(sqlcmd, tran);
                        try { tran.MemberGameRow = this.GetBalance(sqlcmd, row); }
                        catch { }
                    }
                }
                return tran;
            }
            finally
            {
                command.TranIn_delete(sqlcmd, tran);
            }
        }

        protected override Row UpdateRow(SqlCmd sqlcmd, MemberGame_HG.RowCommand command, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                Row row = this.OnSelectRow(sqlcmd, command.MemberID.Value, false);
                if (row == null)
                    return new Row() { GameID = this.GameID };
                sqltool s = new sqltool();
                s[" ", "Locked", "     ", row.Locked, "     "] = command.Locked;
                s[" ", "Balance", "    ", row.Balance, "    "] = command.Balance;
                s[" ", "ACNT", "       ", row.ACNT, "       "] = text.ValidAsString * command.ACNT;
                s[" ", "pwd", "        ", row.Password, "   "] = text.ValidAsString * command.Password;
                s[" ", "mode", "       ", row.mode, "       "] = command.mode;
                s[" ", "firstname", "  ", row.firstname, "  "] = text.ValidAsString * command.firstname;
                s[" ", "lastname", "   ", row.lastname, "   "] = text.ValidAsString * command.lastname;
                s[" ", "currencyid", " ", row.Currency, "   "] = command.Currency;
                s[" ", "agentid", "    ", row.agentid, "    "] = command.agentid;
                s[" ", "affiliateid", "", row.affiliateid, ""] = text.ValidAsString * command.affiliateid;
                s[" ", "testusr", "    ", row.testusr, "    "] = command.testusr;
                s[" ", "playerlevel", "", row.playerlevel, ""] = command.playerlevel;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
                    s.Values["GameID"] = this.GameID;
                    s.Values["MemberID"] = row.MemberID;
                    string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
                    row = sqlcmd.ExecuteEx<Row>(sqlstr);
                }
                return row;
            }
        }

        protected override Row GetBalance(SqlCmd sqlcmd, MemberGame_HG.Row row)
        {
            hgResponse1 res1 = row.get_api(sqlcmd).accountbalance(row.ACNT, row.mode.Value);
            if (res1.status == StatusCode.SUCCESS)
            {
                if (row.Balance != res1.balance)
                {
                    sqlcmd.FillObject(true, row, @"update {0} set Balance={3}, GetBalanceTime=getdate() where GameID={1} and MemberID={2} and Locked<2
select * from {0} nolock where GameID={1} and MemberID={2} and Locked<2", this.TableName, (int)this.GameID, row.MemberID, res1.balance);
                }
            }
            return row;
        }
    }
}