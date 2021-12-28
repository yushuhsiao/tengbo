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
        public class Member_009 : game.Member_XXX
        {
            [DbImport, JsonProperty]
            public string uppername;

            [DbImport, JsonProperty]
            public decimal? TotalBalance;

            public extAPI.bbin.Request login_result;
        }

        [game("Member_009", "MemberID", BU.GameID.BBIN, true)]
        public class MemberRowCommand_BBIN : game.UserGameRowCommand<web.MemberRow, Member_009, MemberRowCommand_BBIN>
        {
            [JsonProperty]
            public string uppername;

            [JsonProperty]
            public decimal? TotalBalance;

            public extAPI.bbin get_api()
            {
                return extAPI.bbin.GetInstance(base._user.CorpID);
            }

            protected override decimal? OnGetBalance(SqlCmd sqlcmd, Member_009 row, bool throw_error)
            {
                base._user = base._user ?? MemberRow.GetMemberEx(sqlcmd, row.UserID, null, null);
                bbin.Request res1 = this.get_api().CheckUsrBalance(row.ACNT, row.uppername);
                if (res1.result == true)
                {
                    foreach (bbin.UserBalance n in res1.balance_data_each())
                    {
                        if (n.LoginName != row.ACNT) continue;
                        //s["", "TotalBalance", ""] = this.TotalBalance = n.TotalBalance;
                        return n.Balance;
                    }
                }
                if (throw_error) throw new RowException(RowErrorCode.GameAPIError, res1.ResponseText);
                else return null;
            }

            protected override Member_009 Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                base._user = _user;
                extAPI.bbin bbin = this.get_api();
                sqltool s = new sqltool();
                s["", this.Field_ID.value, ""] = _user.ID;
                s["", "GameID", "           "] = gameID;
                s["", "Locked", "           "] = 2;
                s["", "Balance", "          "] = 0;
                s["", "TotalBalance", "     "] = 0;
                s["", "Currency", "         "] = CurrencyCode.CNY;
                s["", "uppername", "        "] = bbin.uppername;
                foreach (Member_009 row in base.Register(sqlcmd, s, bbin.prefix, _user.ACNT))
                {
                    if (row.Register_Success == true) return row;
                    bbin.Request res = bbin.CreateMember(row.ACNT, row.uppername, row.Password);
                    if (res.result == true)
                    {
                        row.Register_Success = true;
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }
            //protected override Member_009 Register<TRowCommand>(tran.Game.GameRowCommand<TRowCommand> command)
            //{
            //    UserRow _user = base._user = command._user;
            //    SqlCmd sqlcmd = command.sqlcmd;
            //    GameID gameID = command.GameID.Value;
            //    extAPI.bbin bbin = this.get_api();
            //    sqltool s = new sqltool();
            //    s["", this.Field_ID.value, ""] = _user.ID;
            //    s["", "GameID", "           "] = gameID;
            //    s["", "Locked", "           "] = 2;
            //    s["", "Balance", "          "] = 0;
            //    s["", "TotalBalance", "     "] = 0;
            //    s["", "Currency", "         "] = CurrencyCode.CNY;
            //    s["", "uppername", "        "] = bbin.uppername;
            //    foreach (Member_009 row in base.Register(sqlcmd, s, bbin.prefix, _user.ACNT))
            //    {
            //        if (row.Register_Success == true) return row;
            //        bbin.Request res = bbin.CreateMember(row.ACNT, row.uppername, row.Password);
            //        if (res.result == true)
            //        {
            //            row.Register_Success = true;
            //        }
            //    }
            //    throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            //}

            public override void execute(tran.Game.GameDepositRowCommand command)
            {
                if (command.ID.HasValue) command.delete();
                else
                {
                    foreach (game.Member_009 _usergame in this.tran_insert(command, true))
                    {
                        bbin.Request res1;
                        extAPI.bbin api = this.get_api();
                        //command._usergame = _usergame = _usergame ?? this.Register(command.sqlcmd, command.GameID.Value, command._user);
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            command.accept();
                            try { res1 = api.Transfer(_usergame.ACNT, _usergame.uppername, billno, bbin.tran_action.IN, command._tranrow.Amount, null, null); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.result == true)
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
                    foreach (game.Member_009 _usergame in this.tran_insert(command, false))
                    {
                        bbin.Request res1;
                        extAPI.bbin api = this.get_api();
                        command.insert();
                        string billno = GetTranID(command._tranrow, command.prefix, 0);
                        try
                        {
                            try { res1 = api.Transfer(_usergame.ACNT, _usergame.uppername, billno, bbin.tran_action.OUT, command._tranrow.Amount, null, null); }
                            catch (Exception ex) { command.update(ex.Message, null); throw; }
                            if (res1.result == true)
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