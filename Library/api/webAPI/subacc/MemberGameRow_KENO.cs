using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using web;
using extAPI.kg;
using System.Diagnostics;

namespace web
{
    public abstract class MemberGameRow_KENO : MemberGameRow
    {
        [DbImport, JsonProperty]
        public extAPI.kg.IsTrial? mode;
        [DbImport, JsonProperty]
        public string gametype;
        [DbImport, JsonProperty]
        public string username;
        [DbImport("currencyid"), JsonProperty]
        public string currencyid;
        [DbImport, JsonProperty]
        public string language;

        /// <summary>
        /// 玩家选择赌注上限组别
        /// </summary>
        [DbImport, JsonProperty]
        public string stake;
    }
    public abstract class MemberGameRowCommand_KENO : MemberGameRowCommand
    {
        [JsonProperty]
        public IsTrial? mode;
        [JsonProperty]
        public string gametype;
        [JsonProperty]
        public string language;

        [JsonProperty]
        public string currencyid;
        [JsonProperty]
        public string username;

        [JsonProperty]
        public string stake;

        //internal MemberGameRowCommand_KENO(GameID gameid, string tableName) : base(gameid, tableName) { }

        //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override MemberGameRow_KENO UpdateRow(SqlCmd sqlcmd, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public abstract class MemberGame_KENO<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
        where T : MemberGame_KENO<T, TRow, TRowCommand>, new()
        where TRow : MemberGameRow_KENO, new()
        where TRowCommand : MemberGameRowCommand_KENO, new()
    {
        internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override TRow UpdateRow(SqlCmd sqlcmd, TRowCommand command, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        //protected override decimal? OnGetBalance(SqlCmd sqlcmd, TRow row)
        //{
        //    throw new NotImplementedException();
        //}

        protected override TRow GetBalance(SqlCmd sqlcmd, TRow row)
        {
            throw new NotImplementedException();
        }
    }

    public class MemberGame_KENO : MemberGame_KENO<MemberGame_KENO, MemberGame_KENO.Row, MemberGame_KENO.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_KENO
        {
        }
        public class RowCommand : MemberGameRowCommand_KENO
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_KENO.Instance; } }
        }

        public override GameID GameID { get { return BU.GameID.KENO; } }
        public override string TableName { get { return "Member_004"; } }
    }

    public class MemberGame_KENO_SSC : MemberGame_KENO<MemberGame_KENO_SSC, MemberGame_KENO_SSC.Row, MemberGame_KENO_SSC.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_KENO
        {
        }
        public class RowCommand : MemberGameRowCommand_KENO
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_KENO_SSC.Instance; } }
        }

        public override GameID GameID { get { return BU.GameID.KENO_SSC; } }
        public override string TableName { get { return "Member_007"; } }
    }




    //[DebuggerStepThrough]
    //public class MemberGameRowCommand_KENO : MemberGameRowCommand_KENO<MemberGameRowCommand_KENO>
    //{
    //    public MemberGameRowCommand_KENO() : base(GameID.KENO, "Member_004") { }
    //}
    //[DebuggerStepThrough]
    //public class MemberGameRowCommand_KENO_SSC : MemberGameRowCommand_KENO<MemberGameRowCommand_KENO_SSC>
    //{
    //    public MemberGameRowCommand_KENO_SSC() : base(GameID.KENO_SSC, "Member_007") { }
    //}

//    public abstract class _MemberGameRowCommand_KENO<TMemberGameRowCommand_KENO> : MemberGameRowCommand<TMemberGameRowCommand_KENO, MemberGameRow_KENO>, IGameAPI
//        where TMemberGameRowCommand_KENO : _MemberGameRowCommand_KENO<TMemberGameRowCommand_KENO>, new()
//    {
//        internal override MemberGameRow_KENO OnUpdate2(SqlCmd _sqlcmd, string json_s, params object[] args)
//        {
//             SqlCmd sqlcmd;
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
//            {
//                if (!this.MemberID.HasValue)
//                    throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
//                MemberGameRow_KENO row = this.GetRow2(sqlcmd, this.MemberID.Value);
//                if (row == null)
//                {
//                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.MemberID);
//                    extAPI.hg.api hgapi = extAPI.hg.api.GetInstance(member.CorpID);
//                    CorpRow corp = new CorpRow() { ID = member.CorpID };
//                    sqltool s = new sqltool();
//                    s["*", "MemberID", "   "] = member.ID;
//                    s["*", "GameID", "     "] = this.GameID;
//                    s["*", "Locked", "     "] = (text.ValidAsLocked * this.Locked) ?? 0;
//                    s["*", "Balance", "    "] = this.Balance.ToDecimal() ?? 0;
//                    s["*", "ACNT", "       "] = (text.ValidAsACNT * this.ACNT) ?? (hgapi.prefix ?? "").Trim() + member.ACNT;
//                    s[" ", "mode", "        "] = (int)(this.mode ?? (member.GroupID >= 1 ? extAPI.kg.IsTrial.否 : extAPI.kg.IsTrial.是));
//                    s[" ", "gametype", "       "] = this.gametype;
//                    s["*", "language", "  "] = language;
//                    s["*", "currencyid", " "] = this.currencyid ?? member.Currency.ToString();
//                    s["*", "username", " "] = string.IsNullOrEmpty(this.username) ? member.ACNT : this.username;
//                    s[" ", "stake", "    "] = this.stake;
//                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
//                    string sql = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
//                    select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
//                    row = sqlcmd.ExecuteEx<MemberGameRow_KENO>(sql);
//                }
//                else
//                {
//                    if (this.Balance == "*")
//                        this.Balance = this.GetBalance(sqlcmd, row);
//                    sqltool s = new sqltool();
//                    s[" ", "Locked", "     ", row.Locked, "     "] = text.ValidAsLocked * this.Locked;
//                    s[" ", "Balance", "    ", row.Balance, "    "] = this.Balance.ToDecimal();
//                    //s[" ", "ACNT", "       ", row.ACNT, "       "] = text.ValidAsString * this.ACNT;
//                    //s[" ", "mode", "        ",row.mode,"        "] = this.mode;
//                    //s[" ", "gametype", "   ", row.gametype, "   "] = this.gametype;
//                    //s[" ", "language", "   ", row.language, "   "] = this.language;
//                    //s[" ", "currencyid", " ", row.currencyid, "   "] = this.currencyid ;
//                    //s[" ", "username", "    ", row.username, "    "] = this.username;
//                    //s[" ", "stake", "    ", row.stake, "    "] = this.stake;
//                    if (s.fields.Count > 0)
//                    {
//                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                        s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
//                        s.Values["GameID"] = this.GameID;
//                        s.Values["MemberID"] = row.MemberID;
//                        string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
//                        row = sqlcmd.ExecuteEx<MemberGameRow_KENO>(sqlstr);
//                    }
//                }
//                return row;
//            }
//        }
//        internal string GetBalance(SqlCmd sqlcmd, MemberGameRow_KENO row)
//        {
//            if (row != null)
//            {
//                kgResponse response = extAPI.kg.kgapi.KGUserDetailSearch("kslonglong");
//                if (response != null && response.Count > 0)
//                {
//                    try
//                    {
//                        return response["Credit"];
//                    }
//                    catch (Exception)
//                    {
                        
//                        throw;
//                    }
//                }
//                else
//                {
//                    return row.Balance.ToString();
//                }
//            }
//            else
//            {
//                return row.Balance.ToString();
//            }
//        }
//        public MemberGameRow_KENO RegisterAndLogin(SqlCmd sqlcmd, MemberGameRow_KENO row)
//        {
//            if (row == null)
//                row = (MemberGameRow_KENO)this.OnUpdate(sqlcmd, "");
//            KGUserInfo kguser = new KGUserInfo();
//            kguser.PlayerId =  row.ACNT;
//            kguser.PlayerRealName = row.username;
//            kguser.PlayerCurrency = row.currencyid;
//            kguser.PlayerAllowStake = row.stake;// "65593";
//            kguser.Trial = row.mode.Value;
//            kguser.GameType = row.gametype;
//            kguser.Language = row.language;//"SC";
//            //kguser.PlayerIP = "127.0.0.1";
//            kgResponse response = extAPI.kg.kgapi.RegistAndLogin(kguser);
//            if (!string.IsNullOrEmpty(response["Link"]))
//            {
//                return row;
//            }
//            return null;
//        }
//        internal override bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
//        {
//            MemberGameRow_KENO row = this.GetRow2(sqlcmd, tran.MemberID.Value);
//            if (row == null)
//                row = (MemberGameRow_KENO)(new TMemberGameRowCommand_KENO() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");
//            KGAccountTransfer trans = new KGAccountTransfer();
//            trans.PlayerId = row.ACNT;
//            trans.Amount = -tran.Amount;
//            try
//            {
//                kgResponse res = extAPI.kg.kgapi.KGAccountFirstTransfer(trans);
//                if (!string.IsNullOrEmpty(res["FundIntegrationId"]))
//                {
//                    trans.FundIntegrationId = res["FundIntegrationId"].ToInt32();
//                    trans.PlayerIP = "127.0.0.1";
//                    trans.VendorRef = tran.SerialNumber;
//                    kgResponse retstr = extAPI.kg.kgapi.KGAccountConfirmTransfer(trans);
//                    if (string.IsNullOrEmpty(retstr["Remain"]))
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception e)
//            {
//                log.message("向kg存款失败", e.Message);
//            }
//            return false;
//        }
//        internal override bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
//        {
//            MemberGameRow_KENO row = this.GetRow2(sqlcmd, tran.MemberID.Value);
//            if (row == null)
//                row = (MemberGameRow_KENO)(new TMemberGameRowCommand_KENO() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");
//            KGAccountTransfer trans = new KGAccountTransfer();
//            trans.PlayerId = row.ACNT;
//            trans.Amount = tran.Amount1;
           
//            try
//            {
//                kgResponse retstr = extAPI.kg.kgapi.KGAccountFirstTransfer(trans);
//                int? result = retstr[""].ToInt32();
//                if (result != null)
//                {
//                    trans.FundIntegrationId = (int)result;
//                    trans.PlayerIP = "127.0.0.1";
//                    trans.VendorRef = tran.SerialNumber;
//                    retstr = extAPI.kg.kgapi.KGAccountConfirmTransfer(trans);
//                    double? result2 = retstr[""].ToDouble();
//                    if (result2 == null)
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }

//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception e)
//            {
//                log.message("从KG取款出错，消息：", e.Message);
//            }
//            return false;
//        }
//    }
}