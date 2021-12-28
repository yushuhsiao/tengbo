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
using System.Web.SessionState;
using Tools;

namespace web
{
    partial class game
    {
        public class Member_015 : game.Member_XXX
        {
            [DbImport, JsonProperty]
            public string kioskadminname;

            [DbImport, JsonProperty]
            public string kioskname;

            [DbImport, JsonProperty]
            public string firstname;

            [DbImport, JsonProperty]
            public string lastname;

            public extAPI.pt.Request login_result;
        }

        [game("Member_015", "MemberID", BU.GameID.PT, true)]
        public class MemberRowCommand_PT : game.UserGameRowCommand<web.MemberRow, Member_015, MemberRowCommand_PT>
        {
            public extAPI.pt get_api()
            {
                return extAPI.pt.GetInstance(base._user.CorpID);
            }

            //public string UserPassword { get { return (HttpContext.Current.Session[typeof(User).FullName] as User).UserPassword; } }

            protected override decimal? OnGetBalance(SqlCmd sqlcmd, Member_015 row, bool throw_error)
            {
                base._user = base._user ?? MemberRow.GetMemberEx(sqlcmd, row.UserID, null, null);
                pt.Request res1 = this.get_api().GetPlayerInfo(row.ACNT);
                if (res1.USERNAME == row.ACNT)
                {
                    return res1.BALANCE;
                }
                if (throw_error) throw new RowException(RowErrorCode.GameAPIError, res1.ResponseText);
                else return null;
            }

            protected override Member_015 Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                base._user = _user;
                extAPI.pt pt = this.get_api();
                sqltool s = new sqltool();
                s["", this.Field_ID.value, ""] = _user.ID;
                s["", "GameID", "           "] = gameID;
                s["", "Locked", "           "] = 2;
                s["", "Balance", "          "] = 0;
                s["", "Currency", "         "] = CurrencyCode.CNY;
                s["", "pwd", "              "] = _user.Password;
                s["", "firstname", "        "] = pt.prefix;
                s["", "lastname", "         "] = _user.ACNT;
                s["", "kioskadminname", "   "] = pt.kioskadminname;
                s["", "kioskname", "        "] = pt.kioskname;
                foreach (Member_015 row in base.Register(sqlcmd, s, pt.prefix.ToUpper(), _user.ACNT.ToUpper()))
                {
                    if (row.Register_Success == true) return row;
                    pt.Request res = pt.CreatePlayer(row.ACNT, row.kioskadminname, row.kioskname, row.firstname, row.lastname, null);
                    if (res.playername == pt.prefix.ToUpper() + _user.ACNT.ToUpper() && res.error == null)
                    {
                        row.Register_Success = true;
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }

            public override void execute(tran.Game.GameDepositRowCommand command)
            {
                if (command.ID.HasValue) command.delete();
                else
                {
                    foreach (game.Member_015 _usergame in this.tran_insert(command, true))
                    {
                        pt.Request res1;
                        extAPI.pt api = this.get_api();
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            command.accept();
                            try { res1 = api.Deposit(_usergame.ACNT, command._tranrow.Amount,_usergame.kioskadminname, billno); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.result == "ok" && res1.error == null)
                            {
                                command.finish();
                                command._usergame = base.GetBalance(command.sqlcmd, _usergame, false);
                            }
                            command.update(res1.ResponseText, null);
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
                    foreach (game.Member_015 _usergame in this.tran_insert(command, false))
                    {
                        pt.Request res1;
                        extAPI.pt api = this.get_api();
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            try { res1 = api.Withdrawal(_usergame.ACNT, command._tranrow.Amount, _usergame.kioskadminname, billno); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.result == "ok" && res1.error == null)
                            {
                                command.finish();
                                command._usergame = base.GetBalance(command.sqlcmd, _usergame, false);
                            }
                            command.update(res1.ResponseText, null);
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
