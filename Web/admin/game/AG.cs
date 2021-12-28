using BU;
using extAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    partial class game
    {
        public class Member_011 : game.Member_XXX
        {
            [DbImport, JsonProperty]
            public extAPI.ag.actype? actype;
            [DbImport, JsonProperty]
            public extAPI.ag.oddtype? oddtype;

            public string forwardGame_url;
        }

        public abstract class MemberRowCommand_AG<T> : game.UserGameRowCommand<web.MemberRow, Member_011, T> where T : MemberRowCommand_AG<T>, new()
        {
            [JsonProperty]
            public extAPI.ag.actype? actype;
            [JsonProperty]
            public extAPI.ag.oddtype? oddtype;

            public ag get_api()
            {
                if (base.GameID == BU.GameID.AG_AG)
                    return extAPI.ag.AG.GetInstance(base._user.CorpID);
                else if (base.GameID == BU.GameID.AG_AGIN)
                    return extAPI.ag.AGIN.GetInstance(base._user.CorpID);
                else if (base.GameID == BU.GameID.AG_DSP)
                    return extAPI.ag.DSP.GetInstance(base._user.CorpID);
                else throw new RowException(RowErrorCode.InvalidGameID);
            }

            protected override decimal? OnGetBalance(SqlCmd sqlcmd, Member_011 row, bool throw_error)
            {
                base.GameID = row.GameID;
                base._user = base._user ?? MemberRow.GetMemberEx(sqlcmd, row.UserID, null, null);
                ag.Response res = this.get_api().GetBalance(row.ACNT, row.actype.Value, row.Password);
                if (res.result)
                {
                    decimal? balance = res.info.ToDecimal();
                    if (balance.HasValue)
                        return balance;
                }
                if (throw_error) throw new RowException(RowErrorCode.GameAPIError, HttpUtility.HtmlEncode(res.xml));
                else return null;
            }

            protected override Member_011 Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                base._user = _user;
                base.GameID = gameID;
                ag agapi = this.get_api();
                ag.actype actype = ag.actype.real;//_user.GroupID == 0 ? ag.actype.trial : ag.actype.real;
                sqltool s = new sqltool();
                s["", this.Field_ID.value, ""] = _user.ID;
                s["", "GameID", "           "] = gameID;
                s["", "Locked", "           "] = 2;
                s["", "Balance", "          "] = 0;
                s["", "Currency", "         "] = CurrencyCode.CNY;
                s["", "pwd", "              "] = agapi.password;
                s["", "actype", "           "] = actype;
                s["", "oddtype", "          "] = ag.oddtype.A;
                foreach (Member_011 row in base.Register(sqlcmd, s, agapi.prefix, _user.ACNT))
                {
                    if (row.Register_Success == true) return row;
                    ag.Response res_a = agapi.GetBalance(row.ACNT, actype, row.Password);
                    if ((res_a.result == true) && (res_a.info.ToInt32().HasValue))
                        continue;
                    ag.Response res_b = agapi.CheckOrCreateGameAccout(row.ACNT, actype, row.Password, row.oddtype.Value);
                    if (res_b.result == true)
                        row.Register_Success = true;
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }
            //protected override Member_011 Register<TRowCommand>(tran.Game.GameRowCommand<TRowCommand> command)
            //{
            //    UserRow _user = base._user = command._user;
            //    SqlCmd sqlcmd = command.sqlcmd;
            //    GameID gameID = command.GameID.Value;
            //    base.GameID = gameID;
            //    ag agapi = this.get_api();
            //    ag.actype actype = _user.GroupID == 0 ? ag.actype.trial : ag.actype.real;
            //    sqltool s = new sqltool();
            //    s["", this.Field_ID.value, ""] = _user.ID;
            //    s["", "GameID", "           "] = gameID;
            //    s["", "Locked", "           "] = 2;
            //    s["", "Balance", "          "] = 0;
            //    s["", "Currency", "         "] = CurrencyCode.CNY;
            //    s["", "pwd", "              "] = agapi.password;
            //    s["", "actype", "           "] = actype;
            //    s["", "oddtype", "          "] = ag.oddtype.A;
            //    foreach (Member_011 row in base.Register(sqlcmd, s, agapi.prefix, _user.ACNT))
            //    {
            //        if (row.Register_Success == true) return row;
            //        ag.Response res_a = agapi.GetBalance(row.ACNT, actype, row.Password);
            //        if ((res_a.result == true) && (res_a.info.ToInt32().HasValue))
            //            continue;
            //        ag.Response res_b = agapi.CheckOrCreateGameAccout(row.ACNT, actype, row.Password, row.oddtype.Value);
            //        if (res_b.result == true)
            //            row.Register_Success = true;
            //    }
            //    throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            //}

            public override void execute(tran.Game.GameDepositRowCommand command)
            {
                if (command.ID.HasValue) command.delete();
                else
                {
                    foreach (game.Member_011 _usergame in this.tran_insert(command, true))
                    {
                        ag.Response res1, res2;
                        base.GameID = command.GameID;
                        ag agapi = this.get_api();
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            command.accept();
                            try { res1 = agapi.PrepareTransferCredit(_usergame.ACNT, billno, ag.trantype.IN, command._tranrow.Amount, _usergame.actype.Value, _usergame.Password); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if ((res1.result == true) && (res1.info.ToInt32() == 0))
                            {
                                try { res2 = agapi.TransferCreditConfirm(_usergame.ACNT, billno, ag.trantype.IN, command._tranrow.Amount, _usergame.actype.Value, true, _usergame.Password); }
                                catch (Exception ex) { command.update(null, ex.Message); throw; }
                                if ((res2.result == true) && (res2.info.ToInt32() == 0))
                                {
                                    command.finish();
                                    try { command._usergame = base.GetBalance(command.sqlcmd, _usergame, false); }
                                    catch { }
                                }
                                command.update(res1.xml, res2.xml);
                            }
                            else command.update(res1.xml, null);
                        }
                        finally
                        {
                            command.delete();
                        }
                    }
                }
            }

            public override void execute(tran.Game.GameWithdrawalRowCommand command)
            {
                if (command.ID.HasValue) command.delete();
                else
                {
                    foreach (game.Member_011 _usergame in this.tran_insert(command, false))
                    {
                        ag.Response res1, res2;
                        base.GameID = command.GameID;
                        ag agapi = this.get_api();
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            try { res1 = agapi.PrepareTransferCredit(_usergame.ACNT, billno, ag.trantype.OUT, command._tranrow.Amount, _usergame.actype.Value, _usergame.Password); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if ((res1.result == true) && (res1.info.ToInt32() == 0))
                            {
                                try { res2 = agapi.TransferCreditConfirm(_usergame.ACNT, billno, ag.trantype.OUT, command._tranrow.Amount, _usergame.actype.Value, true, _usergame.Password); }
                                catch (Exception ex) { command.update(null, ex.Message); throw; }
                                if ((res2.result == true) && (res2.info.ToInt32() == 0))
                                {
                                    command.finish();
                                    try { command._usergame = base.GetBalance(command.sqlcmd, _usergame, false); }
                                    catch { }
                                }
                                command.update(res1.xml, res2.xml);
                            }
                            else command.update(res1.xml, null);
                        }
                        finally
                        {
                            command.delete();
                        }
                    }
                }
            }
        }

        [game("Member_011", "MemberID", BU.GameID.AG_AG, true)]
        public class MemberRowCommand_AG_AG : game.MemberRowCommand_AG<MemberRowCommand_AG_AG> { }

        [game("Member_012", "MemberID", BU.GameID.AG_AGIN, true)]
        public class MemberRowCommand_AG_AGIN : game.MemberRowCommand_AG<MemberRowCommand_AG_AGIN> { }

        [game("Member_013", "MemberID", BU.GameID.AG_DSP, true)]
        public class MemberRowCommand_AG_DSP : game.MemberRowCommand_AG<MemberRowCommand_AG_DSP> { }
    }
}