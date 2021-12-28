using BU;
using extAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    public class MemberGame_BBIN : MemberGame<MemberGame_BBIN, MemberGame_BBIN.Row, MemberGame_BBIN.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow
        {
            [DbImport, JsonProperty]
            public string uppername;

            [DbImport, JsonProperty]
            public decimal? TotalBalance;

            public bbin.Request login_result;

            public extAPI.bbin bbin_api;
            public extAPI.bbin get_api(SqlCmd sqlcmd)
            {
                if (this.bbin_api == null)
                    this.bbin_api = extAPI.bbin.GetInstance(this.GetCorpID(sqlcmd));
                return this.bbin_api;
            }
        }
        public class RowCommand : MemberGameRowCommand
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_BBIN.Instance; } }
            
            [JsonProperty]
            public string uppername;

            [JsonProperty]
            public decimal? TotalBalance;
        }
        
        public override GameID GameID { get { return BU.GameID.BBIN; } }
        public override string TableName { get { return "Member_009"; } }

        const int reg_id_max = 10;
        public Row Register(SqlCmd sqlcmd, int? memberID, int? corpID)
        {
            extAPI.bbin bbin = extAPI.bbin.GetInstance(corpID);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", memberID.Value);
                sqltool s = new sqltool();
                s["", "MemberID", "     "] = member.ID;
                s["", "GameID", "       "] = this.GameID;
                s["", "Locked", "       "] = 2;
                s["", "Balance", "      "] = 0;
                s["", "TotalBalance", " "] = 0;
                s["", "Currency", "     "] = CurrencyCode.CNY;
                s["", "uppername", "    "] = bbin.uppername;
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;



                for (int id = 0; id < reg_id_max; id++ )
                {
                    Row row;
                    try
                    {  
                        StringBuilder acnt = new StringBuilder();
                        acnt.Append(bbin.prefix);
                        for (int i = 0; i < member.ACNT.Length; i++)
                            if (System.Security.Cryptography.RandomString.LowerNumber.Contains(member.ACNT[i]))
                                acnt.Append(member.ACNT[i]);
                        if (id != 0)
                            acnt.Append(id.ToString());
                        s["", "ACNT", ""] = acnt.ToString(); //string.Format("{0}{1}{2}", bbin.prefix, member.ACNT, id == 0 ? "" : id.ToString());
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
                    catch(Exception ex)
                    {
                        sqlcmd.Rollback();
                        log.error(ex);
                        continue;
                    }
                    bbin.Request res = bbin.CreateMember(row.ACNT, row.uppername, row.Password);
                    if (res.result == true)
                    {
                        return sqlcmd.ToObject<Row>(true, s.BuildEx(
                            "update {MemberTable} set Locked=0 where GameID={GameID} and MemberID={MemberID}",
                            " select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                    }
                }
                throw new RowException(RowErrorCode.MemberGame_UnableAllocID);
            }
        }

        public Row Login(SqlCmd sqlcmd, int? memberID, int? corpID, bbin.page_site? page_site)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                Row row = this.OnSelectRow(sqlcmd, memberID, false);
                if (row == null)
                    row = this.Register(sqlcmd, memberID, corpID);
                if (row.Locked == BU.Locked.Active)
                {
                    row.CorpID = corpID;
                    bbin.Request res = row.get_api(sqlcmd).Logout(row.ACNT);
                    row.login_result = row.get_api(sqlcmd).Login(row.ACNT, row.uppername, row.Password, bbin.lang.zh_cn, page_site);
                }
                else throw new RowException(BU.RowErrorCode.AccountLocked);
                return row;
            }
        }

        const int len_TranID = 18;
        public static string GetTranID(Guid tranID)
        {
            // remitno :
            // 將 TranID 轉換成數字之後, 取末 19 碼
            byte[] n1 = tranID.ToByteArray();
            Array.Resize(ref n1, n1.Length + 1);
            BigInteger n2 = new BigInteger(n1);
            string n3 = n2.ToString();
            if (n3.Length < len_TranID)
                n3 = new string(' ', len_TranID) + n3;
            string remitno = n3.Substring(Math.Max(n3.Length - len_TranID, 0));
            return remitno;
        }

        //bbin.Request bbin_tran(SqlCmd sqlcmd, Row row, GameTranRow tran)
        //{
        //    bbin.tran_action action;
        //    if (tran.LogType == LogType.GameWithdrawal)
        //        action = bbin.tran_action.OUT;
        //    else if (tran.LogType == LogType.GameDeposit)
        //        action = bbin.tran_action.IN;
        //    else
        //        return null;
        //    return row.get_api(sqlcmd).Transfer(row.ACNT, row.uppername, GetTranID(tran.ID.Value), action, tran.Amount1.Value, null, null);
        //}

        //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        Row row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false) ?? this.Register(sqlcmd, tran.MemberID, tran.CorpID);
        //        if (row != null)
        //        {
        //            command.TranOut(sqlcmd, tran);
        //            if (tran.Amount < 0)
        //            {
        //                row.CorpID = tran.CorpID;
        //                bbin.Request res = row.get_api(sqlcmd).Transfer(row.ACNT, row.uppername, GetTranID(tran.ID.Value), bbin.tran_action.IN, tran.Amount1.Value, null, null);
        //                //bbin.Request res = bbin_tran(sqlcmd, row, tran);
        //                if (res.result == true)
        //                {
        //                    command.TranOut_confirm(sqlcmd, tran);
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
        //        Row row = this.OnSelectRow(sqlcmd, tran.MemberID.Value, false);
        //        if (row != null)
        //        {
        //            row.CorpID = tran.CorpID;
        //            bbin.Request res = row.get_api(sqlcmd).Transfer(row.ACNT, row.uppername, GetTranID(tran.ID.Value), bbin.tran_action.OUT, tran.Amount1.Value, null, null);
        //            //bbin.Request res = bbin_tran(sqlcmd, row, tran);
        //            if (res.result == true)
        //            {
        //                command.TranIn(sqlcmd, tran);
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

        protected override Row UpdateRow(SqlCmd sqlcmd, MemberGame_BBIN.RowCommand command, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                Row row = this.OnSelectRow(sqlcmd, command.MemberID.Value, false);
                if (row == null)
                    return new Row() { GameID = this.GameID };
                sqltool s = new sqltool();
                s[" ", "Locked", "      ", row.Locked, "        "] = command.Locked;
                s[" ", "Balance", "     ", row.Balance, "       "] = command.Balance;
                s[" ", "ACNT", "        ", row.ACNT, "          "] = text.ValidAsString * command.ACNT;
                s[" ", "pwd", "         ", row.Password, "      "] = text.ValidAsString * command.Password;
                s[" ", "uppername", "   ", row.uppername, "     "] = text.ValidAsString * command.uppername;
                s[" ", "TotalBalance", "", row.TotalBalance, "  "] = command.TotalBalance;
                s[" ", "Currency", "    ", row.Currency, "      "] = command.Currency;
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

        protected override Row GetBalance(SqlCmd sqlcmd, MemberGame_BBIN.Row row)
        {
            bbin.Request res1 = row.get_api(sqlcmd).CheckUsrBalance(row.ACNT, row.uppername);
            if (res1.result == true)
            {
                StringBuilder sql = new StringBuilder();
                foreach (bbin.UserBalance n in res1.balance_data_each())
                {
                    if ((n.LoginName == row.ACNT) && (n.Balance == row.Balance) && (n.TotalBalance == row.TotalBalance)) continue;
                    sql.AppendFormat(@"update {0} set Balance={3}, TotalBalance={4}, GetBalanceTime=getdate() where ACNT='{1}' and GameID={2} and Locked<2",
                        this.TableName, n.LoginName, (int)this.GameID, n.Balance, n.TotalBalance);
                    sql.AppendLine();
                }
                sql.AppendFormat("select * from {0} nolock where MemberID={1} and GameID={2}", this.TableName, row.MemberID, (int)this.GameID);
                string sqlstr = sql.ToString();
                sqlcmd.FillObject(sqlstr.StartsWith("update"), row, sqlstr);
            }
            return row;
        }
    }
}