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
using Newtonsoft.Json.Linq;
namespace web
{
    partial class game
    {
        public class Member_015 : game.Member_XXX
        {
            //sub acnt 
            [DbImport, JsonProperty]
            public string prefix;

        }

        [game("Member_015", "MemberID", BU.GameID.PT, true)]
        public class MemberRowCommand_PT : game.UserGameRowCommand<web.MemberRow, Member_015, MemberRowCommand_PT>
        {
            //
            public extAPI.PtApi GetPtApi()
            {
                return extAPI.PtApi.GetInstance(base._user.CorpID);
            }

            protected override decimal? OnGetBalance(SqlCmd sqlcmd, Member_015 row, bool throw_error)
            {
                base._user = base._user ?? MemberRow.GetMemberEx(sqlcmd, row.UserID, null, null);
                PtApi.Request pt_response = this.GetPtApi().GetPlayerBalance(row.ACNT);
                JObject response = pt_response.response;
                string acnt = response["result"]["player"].ToString();
                string balance = response["result"]["balance"]["amount"].ToString();
                if(string.Compare(acnt,row.ACNT.ToUpper()) == 0)
                {
                    return Convert.ToDecimal(balance);
                }
                if (throw_error) throw new RowException(RowErrorCode.GameAPIError, pt_response.response_text);
                else return null;
            }
            protected override Member_015 Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                base._user = _user;
                extAPI.PtApi pt = this.GetPtApi();
                sqltool s = new sqltool();
                s["", this.Field_ID.value, ""] = _user.ID;
                s["", "GameID", "           "] = gameID;
                s["", "Locked", "           "] = 2;
                s["", "Balance", "          "] = 0;
                s["", "TotalBalance", "     "] = 0;
                s["", "Currency", "         "] = CurrencyCode.CNY;
                foreach (Member_015 row in base.Register(sqlcmd, s, pt.prefix, _user.ACNT))
                {
                    if (row.Register_Success == true) 
                        return row;
                    PtApi.Request res = null; // pt.CreatePlayer(row.ACNT.ToUpper(), row.Password);
                    if (string.Compare(res.playername,row.ACNT,true) == 0)
                    {
                        row.Register_Success = true;
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }

            public override void execute(tran.Game.GameDepositRowCommand command)
            {
                //if (command.ID.HasValue) command.delete();
                //else
                //{
                //    foreach (game.Member_015 _usergame in this.tran_insert(command, true))
                //    {
                //        PtApi.Request res1;
                //        extAPI.PtApi api = this.GetPtApi();
                //        command.insert();
                //        string externaltranid = command._tranrow.SerialNumber;
                //        //string billno = GetTranID(command._tranrow, command.prefix, 0);
                //        try
                //        {
                //            command.accept();
                //            //try { res1 = api.Transfer(_usergame.ACNT, _usergame.uppername, billno, bbin.tran_action.IN, command._tranrow.Amount, null, null); }
                //            //playername/{0}/amount/{1}/adminname/{2}/externaltranid
                //            try { res1 = api.Deposit(_usergame.ACNT, command._tranrow.Amount, externaltranid); }
                //            catch (Exception ex) { command.update(ex.Message, null); throw; }
                //            if (res1.result == true)
                //            {
                //                command.finish();
                //                command._usergame = base.GetBalance(command.sqlcmd, _usergame, false);
                //            }
                //            command.update(res1.response_text, null);
                //        }
                //        finally
                //        {
                //            command.delete();
                //        }
                //    }
                //}
            }

            public override void execute(tran.Game.GameWithdrawalRowCommand command)
            {
                //if (command.ID.HasValue) command.delete();
                //else
                //{
                //    foreach (game.Member_015 _usergame in this.tran_insert(command, false))
                //    {
                //        PtApi.Request res1;
                //        extAPI.PtApi api = this.GetPtApi();
                //        command.insert();
                //        string billno = GetTranID(command._tranrow, command.prefix, 0);
                //        try
                //        {
                //            //try { res1 = api.Transfer(_usergame.ACNT, _usergame.uppername, billno, bbin.tran_action.OUT, command._tranrow.Amount, null, null); }
                //            string externaltranid = command._tranrow.SerialNumber;
                //            try { res1 = api.Withdraw(_usergame.ACNT, command._tranrow.Amount, externaltranid); }
                //            catch (Exception ex) { command.update(ex.Message, null); throw; }
                //            if (res1.result == true)
                //            {
                //                command.finish();
                //                command._usergame = base.GetBalance(command.sqlcmd, _usergame, false);
                //            }
                //            command.update(res1.response_text, null);
                //        }
                //        finally
                //        {
                //            command.delete();
                //        }
                //    }
                //}
            }
        }
    }
}
