using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using extAPI.ea;
using web;
using System.Diagnostics;

namespace web
{
    public class MemberGame_EA : MemberGame<MemberGame_EA, MemberGame_EA.Row, MemberGame_EA.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow
        {
            [DbImport, JsonProperty]
            public extAPI.ea.LoginMode mode;
            /// <summary>
            /// 玩家昵称
            /// </summary>
            /// <value>The name of the user.</value>
            [DbImport, JsonProperty]
            public string username;
            /// <summary>
            /// 合营商代理
            /// 只有使用合营系统以及需要发送合营商代码
            ///以便标志在玩家账号的商家才需要提供此代码,不
            ///需要此代码的商家只需要回复空值即可
            /// </summary>
            /// <value>The acode.</value>
            [DbImport, JsonProperty]
            public string agentid;
            /// <summary>
            /// 货币ID（根据国际标准化组织的数字标准） ，例如： “ 156 “ ， “ 840 “等，
            /// </summary>
            /// <value>The currency ID.</value>
            [DbImport("currencyid"), JsonProperty]
            public string currencyid;
        }
        public class RowCommand : MemberGameRowCommand
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_EA.Instance; } }
            //public MemberGameRowCommand_EA() : base(GameID.EA, "Member_002") { }

            [JsonProperty]
            public LoginMode? mode;
            [JsonProperty]
            public string username;
            [JsonProperty]
            public int? agentid;

            //        internal override MemberGameRow_EA OnUpdate2(SqlCmd _sqlcmd, string json_s, params object[] args)
            //        {
            //            SqlCmd sqlcmd;
            //            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
            //            {
            //                if (!this.MemberID.HasValue)
            //                    throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
            //                MemberGameRow_EA row = this.GetRow2(sqlcmd, this.MemberID.Value);
            //                if (row == null)
            //                {
            //                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.MemberID);
            //                    CorpRow corp = new CorpRow() { ID = member.CorpID };
            //                    sqltool s = new sqltool();
            //                    s["*", "MemberID", "   "] = member.ID;
            //                    s["*", "GameID", "     "] = this.GameID;
            //                    s["*", "Locked", "     "] = (text.ValidAsLocked * this.Locked) ?? 0;
            //                    s["*", "Balance", "    "] = this.Balance.ToDecimal() ?? 0;
            //                    s["*", "ACNT", "       "] = member.ACNT;
            //                    s["*N", "username", "       "] = this.username == "" ? this.ACNT : this.username;
            //                    s[" ", "pwd", "        "] = text.ValidAsString * this.Password;
            //                    s[" ", "mode", "       "] = (int)(this.mode ?? (member.GroupID >= 1 ? extAPI.ea.LoginMode.真正 : extAPI.ea.LoginMode.试玩));
            //                    s["*", "currencyid", " "] = this.Currency ?? member.Currency;
            //                    s[" ", "agentid", "    "] = this.agentid;
            //                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            //                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
            //                    string sql = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
            //                    select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
            //                    row = sqlcmd.ExecuteEx<MemberGameRow_EA>(sql);
            //                }
            //                else
            //                {
            //                    if (this.Balance == "*")
            //                        this.Balance = Convert.ToString(this.GetBalance(sqlcmd, row));
            //                    sqltool s = new sqltool();
            //                    s[" ", "Locked", "     ", row.Locked, "     "] = text.ValidAsLocked * this.Locked;
            //                    s[" ", "Balance", "    ", row.Balance, "    "] = this.Balance.ToDecimal();
            //                    //s[" ", "ACNT", "       ", row.ACNT, "       "] = text.ValidAsString * this.ACNT;
            //                    //s["*N", "username","", row.username, "      "] = this.username == "" ? this.ACNT : this.username;
            //                    //s[" ", "pwd", "        ", row.Password, "   "] = text.ValidAsString * this.Password;
            //                    //s[" ", "mode", "       ", row.mode, "       "] = this.mode;
            //                    //s[" ", "currencyid", " ", row.Currency, "   "] = this.Currency;
            //                    //s[" ", "agentid", "    ", row.agentid, "    "] = this.agentid;
            //                    if (s.fields.Count > 0)
            //                    {
            //                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
            //                        s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
            //                        s.Values["GameID"] = this.GameID;
            //                        s.Values["MemberID"] = row.MemberID;
            //                        string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
            //                        row = sqlcmd.ExecuteEx<MemberGameRow_EA>(sqlstr);
            //                    }
            //                }
            //                return row;
            //            }
            //        }
            //        internal float? GetBalance(SqlCmd sqlcmd, MemberGameRow_EA row)
            //        {
            //            if (row != null)
            //            {
            //                EAFanishInfo res1 = new EAFanishInfo() { UserID = row.ACNT, Mode = row.mode, CurrencyID = row.currencyid };
            //                res1 = extAPI.ea.eaApi.GetUserBalance(res1);
            //                if (res1.Status == "0")
            //                    return res1.Balance;
            //            }
            //            return null;
            //        }
            //        internal override bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
            //        {
            //            MemberGameRow_EA row = this.GetRow2(sqlcmd, tran.MemberID.Value);
            //            if (row == null)
            //                row = (MemberGameRow_EA)(new MemberGameRowCommand_EA() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");
            //            Random rdm = new Random();
            //            EAFanishInfo eafain = new EAFanishInfo();
            //            eafain.FuncID = "D" + tran.SerialNumber;
            //            eafain.UserID = row.ACNT;
            //            eafain.CurrencyID = row.currencyid;
            //            eafain.Mode = row.mode;
            //            eafain.Amount = float.Parse((-tran.Amount.Value).ToString());
            //            eafain.Refno = tran.SerialNumber;
            //            try
            //            {
            //                EAFanishInfo reeafain = extAPI.ea.eaApi.DepositFirst(eafain);
            //                if (!string.IsNullOrEmpty(reeafain.Status))
            //                {
            //                    if (reeafain.Status == "0")
            //                    {
            //                        reeafain.FuncID = eafain.FuncID;
            //                        reeafain.Status = "0";
            //                        try
            //                        {
            //                            extAPI.ea.eaApi.DepositConfirm(reeafain);
            //                            log.message("向EA存款" + tran.Amount + "成功，订单号：", tran.SerialNumber);
            //                            return true;
            //                        }
            //                        catch (Exception e)
            //                        {
            //                            log.message("向EA存款审核失败，错误消息：", e.Message);
            //                        }
            //                        return false;
            //                    }
            //                    else
            //                    {
            //                        log.message("向EA存款" + tran.Amount + "失败，错误消息：", reeafain.Errdesc);
            //                        return false;
            //                    }
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                log.message("向EA存款失败，错误消息：", e.Message);
            //            }
            //            return false;
            //        }
            //        internal override bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
            //        {
            //            MemberGameRow_EA row = this.GetRow2(sqlcmd, tran.MemberID.Value);
            //            if (row == null)
            //                row = (MemberGameRow_EA)(new MemberGameRowCommand_EA() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");
            //            Random rdm = new Random();
            //            EAFanishInfo eafain = new EAFanishInfo();
            //            eafain.FuncID = "D" + tran.SerialNumber;
            //            eafain.UserID = row.ACNT;
            //            eafain.CurrencyID = row.currencyid;
            //            eafain.Mode = row.mode;
            //            eafain.Amount = float.Parse(tran.Amount1.Value.ToString());
            //            eafain.Refno = tran.SerialNumber;
            //            try
            //            {
            //                EAFanishInfo reeafain = extAPI.ea.eaApi.Withdraw(eafain);
            //                if (!string.IsNullOrEmpty(reeafain.Status))
            //                {
            //                    if (reeafain.Status == "0")
            //                    {
            //                        log.message("从EA提款" + tran.Amount.Value + "成功，订单号：", tran.SerialNumber);
            //                        return true;
            //                    }
            //                    else
            //                    {
            //                        log.message("从EA提款" + tran.Amount.Value + "失败，错误消息：", reeafain.Errdesc);
            //                        return false;
            //                    }
            //                }
            //                else
            //                {
            //                    log.message("从EA提款" + tran.Amount.Value + "失败，错误消息：", reeafain.Errdesc);
            //                    return false;
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                log.message("从EA提款失败，错误消息：", e.Message);
            //            }
            //            return false;
            //        }

            //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
            //{
            //    throw new NotImplementedException();
            //}

            //internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
            //{
            //    throw new NotImplementedException();
            //}

            //protected override MemberGameRow_EA UpdateRow(SqlCmd sqlcmd, string json_s, params object[] args)
            //{
            //    throw new NotImplementedException();
            //}
        }

        public override GameID GameID { get { return BU.GameID.EA; } }
        public override string TableName { get { return "Member_002"; } }

        internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override MemberGame_EA.Row UpdateRow(SqlCmd sqlcmd, MemberGame_EA.RowCommand command, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        //protected override decimal? OnGetBalance(SqlCmd sqlcmd, MemberGame_EA.Row row)
        //{
        //    throw new NotImplementedException();
        //}

        protected override MemberGame_EA.Row GetBalance(SqlCmd sqlcmd, MemberGame_EA.Row row)
        {
            throw new NotImplementedException();
        }
    }
}