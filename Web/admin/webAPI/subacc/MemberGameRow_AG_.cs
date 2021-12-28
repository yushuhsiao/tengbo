using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using web;

namespace web
{

    //        internal override MemberGameRow_AG OnUpdate2(SqlCmd _sqlcmd, string json_s, params object[] args)
    //        {
    //            SqlCmd sqlcmd;
    //            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
    //            {
    //                if (!this.MemberID.HasValue)
    //                    throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
    //                MemberGameRow_AG row = this.GetRow2(sqlcmd, this.MemberID.Value);
    //                if (row == null)
    //                {
    //                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.MemberID);
    //                    CorpRow corp = new CorpRow() { ID = member.CorpID };
    //                    sqltool s = new sqltool();
    //                    s["*", "MemberID", ""] = member.ID;
    //                    s["*", "GameID", "  "] = this.GameID;
    //                    s["*", "Locked", "  "] = (text.ValidAsLocked * this.Locked) ?? 0;
    //                    s["*", "Balance", " "] = this.Balance.ToDecimal() ?? 0;
    //                    s["*", "ACNT", "    "] = text.ValidAsACNT * this.ACNT;
    //                    s[" ", "pwd", "     "] = text.ValidAsString * this.Password;
    //                    s["*", "Currency", ""] = this.Currency ?? member.Currency;
    //                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
    //                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
    //                    string sql = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
    //                    select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
    //                    return sqlcmd.ExecuteEx<MemberGameRow_AG>(sql);
    //                }
    //                else
    //                {
    //                    sqltool s = new sqltool();
    //                    s[" ", "Locked", "  ", row.Locked, "  "] = text.ValidAsLocked * this.Locked;
    //                    s[" ", "Balance", " ", row.Balance, " "] = this.Balance.ToDecimal();
    //                    s[" ", "ACNT", "    ", row.ACNT, "    "] = text.ValidAsString * this.ACNT;
    //                    s[" ", "pwd", "     ", row.Password, ""] = text.ValidAsString * this.Password;
    //                    s[" ", "Currency", "", row.Currency, ""] = this.Currency;
    //                    if (s.fields.Count == 0) return row;
    //                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
    //                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
    //                    s.Values["GameID"] = this.GameID;
    //                    s.Values["MemberID"] = row.MemberID;
    //                    string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
    //                    return sqlcmd.ExecuteEx<MemberGameRow_AG>(sqlstr);
    //                }
    //            }
    //        }
    //        internal override bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
    //        {
    //            return false;
    //        }
    //        internal override bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
    //        {
    //            return false;
    //        }
}