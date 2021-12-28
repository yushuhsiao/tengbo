using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using web;
using extAPI.kg;
using System.Diagnostics;
using extAPI;

namespace web
{
    public abstract class MemberGameRow_AG : MemberGameRow
    {
        [DbImport, JsonProperty]
        public ag.actype? actype;
        [DbImport, JsonProperty]
        public ag.oddtype? oddtype;

        public string forwardGame_url;

        public ag agapi;
        public ag get_api(SqlCmd sqlcmd)
        {
            if (this.agapi == null)
            {
                if (this.GameID == BU.GameID.AG_AG)
                    this.agapi = extAPI.ag.AG.GetInstance(this.GetCorpID(sqlcmd));
                else if (this.GameID == BU.GameID.AG_AGIN)
                    this.agapi = extAPI.ag.AGIN.GetInstance(this.GetCorpID(sqlcmd));
                else if (this.GameID == BU.GameID.AG_DSP)
                    this.agapi = extAPI.ag.DSP.GetInstance(this.GetCorpID(sqlcmd));
            }
            return this.agapi;
        }
    }
    public abstract class MemberGameRowCommand_AG : MemberGameRowCommand
    {
        [JsonProperty]
        public extAPI.ag.actype? actype;
        [JsonProperty]
        public extAPI.ag.oddtype? oddtype;
    }



    public abstract class MemberGame_AG<TAPI, T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
        where TAPI : ag.api<TAPI>
        where T : MemberGame_AG<TAPI, T, TRow, TRowCommand>, new()
        where TRow : MemberGameRow_AG, new()
        where TRowCommand : MemberGameRowCommand_AG, new()
    {
        const int reg_id_max = 10;
        public TRow Register(SqlCmd sqlcmd, int? memberID, int? corpID/*, bool trial*/)
        {
            //ag.actype actype = trial ? ag.actype.trial : ag.actype.real ;
            TAPI agapi = ag.api<TAPI>.GetInstance(corpID);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", memberID.Value);
                ag.actype actype = member.GroupID == 0 ? ag.actype.trial : ag.actype.real;
                sqltool s = new sqltool();
                s["", "MemberID", "     "] = member.ID;
                s["", "GameID", "       "] = this.GameID;
                s["", "Locked", "       "] = 2;
                s["", "Balance", "      "] = 0;
                s["", "Currency", "     "] = CurrencyCode.CNY;
                s["", "pwd", "          "] = agapi.password;
                s["", "actype", "       "] = actype;
                s["", "oddtype", "      "] = ag.oddtype.A;
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;

                for (int id = 0; id < reg_id_max; id++)
                {
                    TRow row;
                    try
                    {
                        s["", "ACNT", ""] = string.Format("{0}{1}{2}", agapi.prefix, member.ACNT, id == 0 ? "" : id.ToString());
                        sqlcmd.BeginTransaction();
                        row = sqlcmd.ToObject<TRow>(s.BuildEx(
                            "delete {MemberTable} where GameID={GameID} and MemberID={MemberID} and Locked=2",
                            " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                        if (row == null)
                        {
                            int cnt = sqlcmd.ExecuteScalar<int>(s.BuildEx("select count(*) from {MemberTable} nolock where GameID={GameID} and ACNT={ACNT}")) ?? 0;
                            if (cnt == 0)
                                row = sqlcmd.ToObject<TRow>(s.BuildEx(
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
                    ag.Response res_a = agapi.GetBalance(row.ACNT, actype, row.Password);
                    if ((res_a.result == true) && (res_a.info.ToInt32().HasValue))
                        continue;
                    ag.Response res_b = agapi.CheckOrCreateGameAccout(row.ACNT, actype, row.Password, row.oddtype.Value);
                    if (res_b.result == true)
                    {
                        return sqlcmd.ToObject<TRow>(true, s.BuildEx(
                            "update {MemberTable} set Locked=0 where GameID={GameID} and MemberID={MemberID}",
                            " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }
        }

        public TRow Login(SqlCmd sqlcmd, int? memberID, int? corpID/*, bool trial*/)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                TRow row = this.OnSelectRow(sqlcmd, memberID, false);
                if (row == null)
                    row = this.Register(sqlcmd, memberID, corpID/*, trial*/);
                if (row.Locked == BU.Locked.Active)
                {
                    row.CorpID = corpID;
                    row.forwardGame_url = row.get_api(sqlcmd).forwardGame(row.ACNT, row.Password, null, null, row.actype.Value, null, null);
                }
                else throw new RowException(BU.RowErrorCode.AccountLocked);
                return row;
            }
        }

        //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        TRow row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false) ?? this.Register(sqlcmd, tran.MemberID, tran.CorpID/*, true*/);
        //        if (row != null)
        //        {
        //            command.TranOut(sqlcmd, tran);
        //            if (tran.Amount < 0)
        //            {
        //                row.CorpID = tran.CorpID;
        //                ag agapi = row.get_api(sqlcmd);
        //                string billno = MemberGame_BBIN.GetTranID(tran.ID.Value);
        //                ag.Response res1 = agapi.PrepareTransferCredit(row.ACNT, billno, ag.trantype.IN, tran.Amount1.Value, row.actype.Value, row.Password);
        //                if ((res1.result == true) && (res1.info.ToInt32() == 0))
        //                {
        //                    ag.Response res2 = agapi.TransferCreditConfirm(row.ACNT, billno, ag.trantype.IN, tran.Amount1.Value, row.actype.Value, true, row.Password);
        //                    if ((res2.result == true) && (res2.info.ToInt32() == 0))
        //                        command.TranOut_confirm(sqlcmd, tran);
        //                    else
        //                        command.TranOut_delete(sqlcmd, tran);
        //                    try { tran.MemberGameRow = this.GetBalance(sqlcmd, row); }
        //                    catch { }
        //                }
        //            }
        //        }
        //        return tran;
        //    }
        //    finally
        //    {
        //        command.TranOut_delete(sqlcmd, tran);
        //    }
        //}

        //internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        TRow row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false);
        //        if (row != null)
        //        {
        //            row.CorpID = tran.CorpID;
        //            ag agapi = row.get_api(sqlcmd);
        //            string billno = MemberGame_BBIN.GetTranID(tran.ID.Value);
        //            ag.Response res = agapi.PrepareTransferCredit(row.ACNT, billno, ag.trantype.OUT, tran.Amount1.Value, row.actype.Value, row.Password);
        //            if ((res.result == true) && (res.info.ToInt32() == 0))
        //            {
        //                ag.Response res2 = agapi.TransferCreditConfirm(row.ACNT, billno, ag.trantype.OUT, tran.Amount1.Value, row.actype.Value, true, row.Password);
        //                if ((res2.result == true) && (res2.info.ToInt32() == 0))
        //                    command.TranIn(sqlcmd, tran);
        //                else
        //                    command.TranIn_delete(sqlcmd, tran);
        //                try { tran.MemberGameRow = this.GetBalance(sqlcmd, row); }
        //                catch { }
        //            }
        //        }
        //        return tran;
        //    }
        //    finally
        //    {
        //        command.TranIn_delete(sqlcmd, tran);
        //    }
        //}

        protected override TRow UpdateRow(SqlCmd sqlcmd, TRowCommand command, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                TRow row = this.OnSelectRow(sqlcmd, command.MemberID.Value, false);
                if (row == null)
                    return new TRow() { GameID = this.GameID };
                sqltool s = new sqltool();
                s[" ", "Locked", "      ", row.Locked, "        "] = command.Locked;
                s[" ", "Balance", "     ", row.Balance, "       "] = command.Balance;
                s[" ", "ACNT", "        ", row.ACNT, "          "] = text.ValidAsString * command.ACNT;
                s[" ", "pwd", "         ", row.Password, "      "] = text.ValidAsString * command.Password;
                s[" ", "actype", "      ", row.actype, "        "] = command.actype;
                s[" ", "oddtype", "     ", row.oddtype, "       "] = command.oddtype;
                s[" ", "Currency", "    ", row.Currency, "      "] = command.Currency;
                if (s.fields.Count > 0)
                {
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
                    s.Values["GameID"] = this.GameID;
                    s.Values["MemberID"] = row.MemberID;
                    string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
                    row = sqlcmd.ExecuteEx<TRow>(sqlstr);
                }
                return row;
            }
        }

        protected override TRow GetBalance(SqlCmd sqlcmd, TRow row)
        {
            ag.Response res = row.get_api(sqlcmd).GetBalance(row.ACNT, row.actype.Value, row.Password);
            if (res.result)
            {
                decimal? balance = res.info.ToDecimal();
                if (balance.HasValue)
                {
                    sqlcmd.FillObject(true, row, @"update {0} set Balance={3}, GetBalanceTime=getdate() where GameID={1} and MemberID={2} and Locked<2
select * from {0} nolock where GameID={1} and MemberID={2} and Locked<2", this.TableName, (int)this.GameID, row.MemberID, balance);
                }
            }
            return row;
        }
    }



    public class MemberGame_AG_AG : MemberGame_AG<ag.AG, MemberGame_AG_AG, MemberGame_AG_AG.Row, MemberGame_AG_AG.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_AG { }
        public class RowCommand : MemberGameRowCommand_AG { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_AG_AG.Instance; } } }

        public override GameID GameID { get { return BU.GameID.AG_AG; } }
        public override string TableName { get { return "Member_011"; } }
    }

    public class MemberGame_AG_AGIN : MemberGame_AG<ag.AGIN, MemberGame_AG_AGIN, MemberGame_AG_AGIN.Row, MemberGame_AG_AGIN.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_AG { }
        public class RowCommand : MemberGameRowCommand_AG { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_AG_AGIN.Instance; } } }

        public override GameID GameID { get { return BU.GameID.AG_AGIN; } }
        public override string TableName { get { return "Member_012"; } }
    }

    public class MemberGame_AG_DSP : MemberGame_AG<ag.DSP, MemberGame_AG_DSP, MemberGame_AG_DSP.Row, MemberGame_AG_DSP.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_AG { }
        public class RowCommand : MemberGameRowCommand_AG { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_AG_DSP.Instance; } } }

        public override GameID GameID { get { return BU.GameID.AG_DSP; } }
        public override string TableName { get { return "Member_013"; } }
    }
}