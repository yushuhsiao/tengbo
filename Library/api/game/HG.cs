using BU;
using extAPI.hg;
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
        public class Member_001 : game.Member_XXX
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
        }

        [game("Member_001", "MemberID", BU.GameID.HG, true)]
        public class MemberRowCommand_HG : game.UserGameRowCommand<web.MemberRow, Member_001, MemberRowCommand_HG>
        {
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

            public extAPI.hg.api get_api()
            {
                return extAPI.hg.api.GetInstance(base._user.CorpID);
            }

            protected override decimal? OnGetBalance(SqlCmd sqlcmd, Member_001 row, bool throw_error)
            {
                base._user = base._user ?? MemberRow.GetMemberEx(sqlcmd, row.UserID, null, null);
                hgResponse1 res = this.get_api().accountbalance(row.ACNT, row.mode.Value);
                if (res.status == StatusCode.SUCCESS)
                    return res.balance;
                if (throw_error) throw new RowException(RowErrorCode.GameAPIError, HttpUtility.HtmlEncode(res.xml));
                return null;
            }

            protected override Member_001 Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                base._user = _user;
                extAPI.hg.api hgapi = this.get_api();
                sqltool s = new sqltool();
                s["", this.Field_ID.value, ""] = _user.ID;
                s["", "GameID", "           "] = gameID;
                s["", "Locked", "           "] = 2;
                s["", "Balance", "          "] = 0;
                s["", "mode", "             "] = (int)(_user.GroupID >= 1 ? extAPI.hg.LoginMode.真正 : extAPI.hg.LoginMode.娛樂);
                s["", "firstname", "        "] = _user.ACNT;
                s["", "lastname", "         "] = hgapi.prefix ?? "";
                s["", "currencyid", "       "] = _user.Currency;
                foreach (Member_001 row in base.Register(sqlcmd, s, hgapi.prefix, _user.ACNT))
                {
                    if (row.Register_Success == true) return row;
                    hgResponse1 res_a = hgapi.accountbalance(row.ACNT, row.mode.Value);
                    if (res_a.status == StatusCode.ERR_WITH_USER)
                    {
                        hgResponse1 res_b = hgapi.registration(row.ACNT, row.Password, row.mode.Value, row.firstname, row.lastname, row.currencyid, row.agentid, row.affiliateid, row.testusr, row.playerlevel);
                        if (res_b.status == StatusCode.SUCCESS)
                        {
                            row.ticket = res_b.ticket;
                            row.Register_Success = true;
                        }
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }
            //protected override Member_001 Register<TRowCommand>(tran.Game.GameRowCommand<TRowCommand> command)
            //{
            //    UserRow _user = base._user = command._user;
            //    SqlCmd sqlcmd = command.sqlcmd;
            //    GameID gameID = command.GameID.Value;
            //    extAPI.hg.api hgapi = this.get_api();
            //    sqltool s = new sqltool();
            //    s["", this.Field_ID.value, ""] = _user.ID;
            //    s["", "GameID", "           "] = gameID;
            //    s["", "Locked", "           "] = 2;
            //    s["", "Balance", "          "] = 0;
            //    s["", "mode", "             "] = (int)(_user.GroupID >= 1 ? extAPI.hg.LoginMode.真正 : extAPI.hg.LoginMode.娛樂);
            //    s["", "firstname", "        "] = _user.ACNT;
            //    s["", "lastname", "         "] = hgapi.prefix ?? "";
            //    s["", "currencyid", "       "] = _user.Currency;
            //    foreach (Member_001 row in base.Register(sqlcmd, s, hgapi.prefix, _user.ACNT))
            //    {
            //        if (row.Register_Success == true) return row;
            //        hgResponse1 res_a = hgapi.accountbalance(row.ACNT, row.mode.Value);
            //        if (res_a.status == StatusCode.ERR_WITH_USER)
            //        {
            //            hgResponse1 res_b = hgapi.registration(row.ACNT, row.Password, row.mode.Value, row.firstname, row.lastname, row.currencyid, row.agentid, row.affiliateid, row.testusr, row.playerlevel);
            //            if (res_b.status == StatusCode.SUCCESS)
            //            {
            //                row.ticket = res_b.ticket;
            //                row.Register_Success = true;
            //            }
            //        }
            //    }
            //    throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            //}

            public override void execute(tran.Game.GameDepositRowCommand command)
            {
                if (command.ID.HasValue) command.delete();
                else
                {
                    foreach (game.Member_001 _usergame in this.tran_insert(command, true))
                    {
                        extAPI.hg.hgResponse1 res1, res2;
                        command.insert();
                        try
                        {
                            command.accept();
                            try { res1 = this.get_api().deposit(_usergame.ACNT, _usergame.mode.Value, _usergame.currencyid, command._tranrow.Amount, command._tranrow.SerialNumber, null, null); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.status == extAPI.hg.StatusCode.SUCCESS)
                            {
                                try { res2 = this.get_api().deposit_confirm(extAPI.hg.StatusCode.SUCCESS, res1.paymentid, res1.errdesc); }
                                catch (Exception ex) { command.update(null, ex.Message); throw; }
                                if (res2.status == extAPI.hg.StatusCode.SUCCESS)
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
                    foreach (game.Member_001 _usergame in this.tran_insert(command, false))
                    {
                        extAPI.hg.hgResponse1 res1, res2;
                        extAPI.hg.api hgapi = this.get_api();
                        command.insert();
                        try
                        {
                            try { res1 = hgapi.withdrawal(_usergame.ACNT, _usergame.mode.Value, _usergame.currencyid, command._tranrow.Amount, command._tranrow.SerialNumber, null); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.status == extAPI.hg.StatusCode.SUCCESS)
                            {
                                try { res2 = hgapi.withdrawal_confirm(extAPI.hg.StatusCode.SUCCESS, res1.paymentid, res1.errdesc); }
                                catch (Exception ex) { command.update(null, ex.Message); throw; }
                                if (res2.status == extAPI.hg.StatusCode.SUCCESS)
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
    }
}