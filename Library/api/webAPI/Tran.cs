using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace web
{
    public abstract class TranRowCommand<_TranRow> where _TranRow : TranRow, new()
    {
        [JsonProperty("Accept")]
        public virtual int? op_Accept { get; set; }   // 額度轉出, 預扣
        [JsonProperty("Finish")]
        public virtual int? op_Finish { get; set; }
        public virtual int? op_Delete { get; set; }

        //public virtual AcceptOrReject? accept { get; set; }
        //public virtual CommitOrRollback? commit { get; set; }
        [JsonProperty]
        public virtual LogType? LogType { get; set; }

        [JsonProperty]
        public virtual Guid? ID { get; set; }

        public virtual CurrencyCode? Currency { get; set; }
        [JsonProperty]
        public virtual GameID? GameID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string MemberACNT { get; set; }
        [JsonProperty]
        public virtual int? MemberID { get; set; }
        [JsonProperty]
        public virtual decimal? Amount1 { get; set; }
        [JsonProperty]
        public virtual decimal? Amount2 { get; set; }

        protected abstract string TableName { get; }

        //protected void _TranValidOp()
        //{
        //    if (this.accept.HasValue)
        //        if (!Enum.IsDefined(typeof(AcceptOrReject), this.accept.Value))
        //            this.accept = null;
        //    if (this.commit.HasValue)
        //        if (!Enum.IsDefined(typeof(CommitOrRollback), this.commit.Value))
        //            this.commit = null;
        //}

        //        protected void _TranWriteLog(SqlCmd sqlcmd, _TranRow row, LogType logType, decimal amount)
        //        {
        //            sqltool s = new sqltool();
        //            s["", "LogType", "     "] = logType;
        //            s["", "GameID", "      "] = row.GameID;
        //            s["", "MemberID", "    "] = row.MemberID;
        //            s["", "CorpID", "      "] = row.CorpID;
        //            s["", "ACNT", "        "] = row.MemberACNT;
        //            s["", "PrevBalance", " "] = row.PrevBalance;
        //            s["", "Amount", "      "] = amount;
        //            s["", "Balance", "     "] = row.Balance;
        //            s["", "CurrencyA", "   "] = row.CurrencyA;
        //            s["", "CurrencyB", "   "] = row.CurrencyB;
        //            s["", "CurrencyX", "   "] = row.CurrencyX;
        //            s["", "SerialNumber", ""] = row.SerialNumber;
        //            s["", "TranID", "      "] = row.ID;
        //            s["", "RequestIP", "   "] = row.RequestIP;
        //            s["", "RequestTime", " "] = row.CreateTime;
        //            s["", "FinishTime", "  "] = row.FinishTime;
        //            sqlcmd.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_000 (", sqltool._Fields, ") values (", sqltool._Values, ")"));
        //        }

        //        protected void _TranGetAmount(SqlCmd sqlcmd, _TranRow row, string amount)
        //        {
        //            row.ModifyUser = (HttpContext.Current.User as User).ID;
        //            row.MemberID = null;
        //            row.Amount = row.PrevBalance = row.Balance = null;
        //            if (string.IsNullOrEmpty(amount)) return;
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @MemberID int, @Amount decimal(19,6)
        //select @Amount={2}, @MemberID=MemberID from {1} nolock where ID='{0}'
        //select @Amount as Amount, @MemberID as MemberID
        //if @MemberID is not null select Balance as PrevBalance from Member nolock where ID=@MemberID", row.ID, this._tableName, amount)) r.FillObject(row);
        //            if (!row.Amount.HasValue) throw new RowException(RowErrorCode.TranNotFound);
        //            if (!row.PrevBalance.HasValue) throw new RowException(RowErrorCode.MemberNotFound);
        //        }

        //        protected void _TranUpdateBalance(SqlCmd sqlcmd, _TranRow row, TranState state, LogType logType)
        //        {
        //            row.Balance = null;
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update Member set Balance=Balance+{1} where ID={0}
        //select Balance,{2} as [State] from Member nolock where ID={0}", row.MemberID, row.Amount, (int)state))
        //                r.FillObject(row);
        //            if (!row.Balance.HasValue)
        //                throw new RowException(RowErrorCode.MemberNotFound);
        //            this._TranWriteLog(sqlcmd, row, logType, row.Amount.Value);
        //        }

        //        protected void _TranIn(SqlCmd sqlcmd, _TranRow row)
        //        {
        //            try
        //            {
        //                sqlcmd.BeginTransaction();
        //                if (this.commit == CommitOrRollback.Commit)
        //                {
        //                    this._TranGetAmount(sqlcmd, row, "Amount1");
        //                    if (row.Amount.Value != 0)
        //                        this._TranUpdateBalance(sqlcmd, row, TranState.Transferred, row.LogType.Value);
        //                }
        //                else
        //                    row.State = TranState.Rejected;
        //                this._TranFinish(sqlcmd, row, "Amount2");
        //                sqlcmd.Commit();
        //            }
        //            catch
        //            {
        //                sqlcmd.Rollback();
        //                throw;
        //            }
        //        }

        //        protected void _TranOut_accept(SqlCmd sqlcmd, _TranRow row)
        //        {
        //            this._TranGetAmount(sqlcmd, row, "Amount1-Amount2");
        //            if (row.Amount.Value == 0) return;
        //            if (row.PrevBalance.Value < row.Amount.Value)
        //                throw new RowException(RowErrorCode.BalanceNotEnough);
        //            try
        //            {
        //                sqlcmd.BeginTransaction();
        //                row.Balance = null;
        //                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update Member set Balance=Balance-{3} where ID={1}
        //update {0} set Amount2=Amount2+{3}, [State]={4},ModifyTime=getdate(), ModifyUser={5} where ID='{2}'
        //select Balance from Member nolock where ID={1}
        //select * from {0} nolock where ID='{2}'", this._tableName, row.MemberID, row.ID, row.Amount, (int)TranState.Accepted, row.ModifyUser)) r.FillObject(row);
        //                if (!row.Balance.HasValue)
        //                    throw new RowException(RowErrorCode.MemberNotFound);
        //                this._TranWriteLog(sqlcmd, row, row.LogType.Value, -row.Amount.Value);
        //                sqlcmd.Commit();
        //            }
        //            catch
        //            {
        //                sqlcmd.Rollback();
        //                throw;
        //            }
        //        }

        //        protected void _TranOut(SqlCmd sqlcmd, _TranRow row, LogType logType_rollback)
        //        {
        //            if ((this.accept == AcceptOrReject.Reject) || (this.commit.HasValue))
        //            {
        //                try
        //                {
        //                    sqlcmd.BeginTransaction();
        //                    this._TranGetAmount(sqlcmd, row, "Amount2");
        //                    if (row.Amount.Value == 0)
        //                        row.State = TranState.Rejected;
        //                    else if (this.commit == CommitOrRollback.Commit)
        //                        row.State = TranState.Transferred;
        //                    else
        //                        this._TranUpdateBalance(sqlcmd, row, TranState.Failed, logType_rollback);
        //                    row.Amount2 = 0;
        //                    this._TranFinish(sqlcmd, row, "0");
        //                    sqlcmd.Commit();
        //                }
        //                catch
        //                {
        //                    sqlcmd.Rollback();
        //                    throw;
        //                }
        //            }
        //        }

        //        protected virtual void _TranFinish(SqlCmd sqlcmd, _TranRow row, string amount2) { }



        /// <summary>
        /// 產生新訂單
        /// </summary>
        public virtual _TranRow Insert(string json_s, params object[] args) { return null; }


        /// <summary>
        /// 更新訂單內容
        /// </summary>
        public virtual _TranRow Update(string json_s, params object[] args) { return null; }
        public _TranRow Delete(string json_s, params object[] args)
        {
            this.op_Delete = 1;
            return this.Update(json_s, args);
        }


        protected void WriteTranLog(SqlCmd sqlcmd, _TranRow row, LogType logType, decimal amount)
        {
            if (amount == 0) return;
            sqltool s = new sqltool();
            if (row.FinishTime.HasValue)
            {
                s["", "CreateTime", "  "] = row.FinishTime;
                s["", "ACTime", "  "] = row.FinishTime.Value.ToACTime();
            }
            else
            {
                s["", "CreateTime", "  "] = StringEx.sql_str.getdate;
                s["", "ACTime", "  "] = (StringEx.sql_str)"dateadd(dd,datediff(dd,0,dateadd(hh,-12,getdate())),0)";
            }
            s["", "LogType", "     "] = logType;
            s["", "GameID", "      "] = row.GameID;
            s["", "MemberID", "    "] = row.MemberID;
            s["", "CorpID", "      "] = row.CorpID;
            s["", "ACNT", "        "] = row.MemberACNT;
            s["", "AgentID", "     "] = row.AgentID;
            s["", "AgentACNT", "   "] = row.AgentACNT;
            s["", "PrevBalance", " "] = row.PrevBalance;
            s["", "Amount", "      "] = amount;
            s["", "Balance", "     "] = row.Balance;
            s["", "CurrencyA", "   "] = row.CurrencyA;
            s["", "CurrencyB", "   "] = row.CurrencyB;
            s["", "CurrencyX", "   "] = row.CurrencyX;
            s["", "SerialNumber", ""] = row.SerialNumber;
            s["", "TranID", "      "] = row.ID;
            s["", "RequestIP", "   "] = row.RequestIP;
            s["", "RequestTime", " "] = row.CreateTime;
            s["", "FinishTime", "  "] = row.FinishTime;
            sqlcmd.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_000 (", sqltool._Fields, ") values (", sqltool._Values, ")"));
        }

        //        protected void GetAmount(SqlCmd sqlcmd, _TranRow row, string amount)
        //        {
        //            row.ModifyUser = (HttpContext.Current.User as User).ID;
        //            row.MemberID = null;
        //            row.Amount = row.PrevBalance = row.Balance = null;
        //            row.FinishTime = null;
        //            if (string.IsNullOrEmpty(amount)) return;
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @MemberID int, @Amount decimal(19,6), @FinishTime datetime
        //select @Amount={2}, @MemberID=MemberID, @FinishTime=FinishTime from {1} nolock where ID='{0}'
        //select @Amount as Amount, @FinishTime as FinishTime
        //if @MemberID is not null select ID as MemberID, Balance as PrevBalance from Member nolock where ID=@MemberID", row.ID, this._tableName, amount)) r.FillObject(row);
        //            if (!row.Amount.HasValue) throw new RowException(RowErrorCode.TranNotFound);
        //            if (!row.PrevBalance.HasValue) throw new RowException(RowErrorCode.MemberNotFound);
        //        }

        //_TranRow getrow(SqlCmd sqlcmd, _TranRow row, string sql, Guid? id)
        //{
        //    if (row == null)
        //        row = sqlcmd.GetRow<_TranRow>(sql, this._tableName, this.ID);
        //    if (row == null)
        //        throw new RowException(RowErrorCode.TranNotFound, null, this.ID);
        //    if (row.FinishTime.HasValue)
        //        throw new RowException(RowErrorCode.TranAlreadyFinished, null, this.ID);
        //    return row;
        //}

        //static string sql_in_del;

        //static string sql_out_del;

        protected virtual BU.LogType? LogType_DepositRollback { get { return null; } }
        protected virtual BU.LogType? LogType_Rollback { get { return null; } }

        void update_balance(SqlCmd sqlcmd, _TranRow row, decimal amount)
        {
            row.PrevBalance = row.Balance = null;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select Balance as PrevBalance from Member nolock where ID={0}
update Member set Balance=Balance+({1}) where ID={0} and (Balance+({1})) >= 0
select Balance from Member nolock where ID={0}", row.MemberID, amount)) r.FillObject(row);
            if (!row.Balance.HasValue || !row.PrevBalance.HasValue)
                throw new RowException(RowErrorCode.MemberNotFound);
            row.Amount = row.Balance - row.PrevBalance;
        }

        void delete_row(sqltool s, SqlCmd sqlcmd, _TranRow row)
        {
            if (s.fields.Count == 0)
                throw new RowException(RowErrorCode.TranNotFound);
            s.Values["TableName"] = (StringEx.sql_str)this.TableName;
            s.Values["_ID"] = row.ID;
            sqlcmd.FillObject(row, s.BuildEx(@"
insert into {TableName}2 (", sqltool._Fields, @")
select                   ", sqltool._Values, @"
from {TableName}1 nolock where ID={_ID} delete {TableName}1 where ID={_ID} select 1 as _RowDeleted"));
        }

        internal protected void TranIn(SqlCmd sqlcmd, _TranRow row)
        {
            User user = HttpContext.Current.User as User;
            row.Amount1 = null;
            row.FinishTime = null;
            sqlcmd.FillObject(row, "select MemberID,Amount1,Amount2,FinishTime from {0}1 nolock where ID='{1}'", this.TableName, row.ID);
            if (!row.Amount1.HasValue)
                throw new RowException(RowErrorCode.TranNotFound);
            decimal amount = row.Amount1.Value + row.Amount2.Value;
            if ((amount == 0) || row.FinishTime.HasValue)
                return;
            try
            {
                sqlcmd.BeginTransaction();
                update_balance(sqlcmd, row, amount);
                if (row.Amount.Value != 0)
                {
                    sqlcmd.FillObject(row, "update {0}1 set [State]={3},FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={2} where ID='{1}' and FinishTime is null select @@rowcount as __rowcount, * from {0}1 nolock where ID='{1}'"
                        , this.TableName, row.ID, user.ID, (int)TranState.Transferred);
                    if (row.__rowcount != 1)
                        throw new RowException(RowErrorCode.NoResult);
                    this.WriteTranLog(sqlcmd, row, row.LogType.Value, row.Amount.Value);
                }
                sqlcmd.Commit();
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
            #region
            //            try
            //            {
            //                sqlcmd.BeginTransaction();
            //                row = sqlcmd.ToObject<_TranRow>(@"declare @MemberID int, @FinishTime datetime, @PrevBalance decimal(19,6), @Balance decimal(19,6), @AmountA decimal(19,6), @AmountB decimal(19,6)
            //select @MemberID=MemberID, @AmountA=Amount1, @FinishTime=FinishTime from {0}1 nolock where ID='{1}'
            //if @FinishTime is null and isnull(@AmountA,0)<>0
            //begin
            //    select @PrevBalance=Balance from Member nolock where ID=@MemberID
            //    update Member set Balance=Balance+@AmountA where ID=@MemberID
            //    select @Balance=Balance from Member nolock where ID=@MemberID
            //    select @AmountB=@Balance-@PrevBalance
            //    if @AmountB <> 0 update {0}1 set [State]={2},FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={3} where ID='{1}'
            //end
            //select @PrevBalance as PrevBalance, @Balance as Balance, @AmountB as Amount, * from {0}1 nolock where ID='{1}'"
            //                    , this._tableName, this.ID, (int)TranState.Transferred, (HttpContext.Current.User as User).ID);
            //                if (row == null)
            //                    throw new RowException(RowErrorCode.TranNotFound);
            //                if (row.Amount.HasValue)
            //                {
            //                    this.WriteTranLog(sqlcmd, row, row.LogType.Value, row.Balance.Value - row.PrevBalance.Value);
            //                    sqlcmd.Commit();
            //                    return row;
            //                }
            //                else throw new RowException(RowErrorCode.MemberNotFound, null, row.MemberID, row.CorpID, row.MemberACNT);
            //            }
            //            catch
            //            {
            //                sqlcmd.Rollback();
            //                throw;
            #endregion
        }

        internal protected void TranIn_delete(SqlCmd sqlcmd, _TranRow row)
        {
            User user = HttpContext.Current.User as User;
            try
            {
                sqltool s = new sqltool();
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update {0}1 set State={3}, FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={2} where ID='{1}' and FinishTime is null
select * from {0}1 nolock where ID='{1}'", this.TableName, row.ID, user.ID, (int)TranState.Rejected))
                {
                    r.FillObject(row);
                    for (int i = 0; i < r.FieldCount; i++) s["", r.GetName(i), ""] = (StringEx.sql_str)string.Format("[{0}]", r.GetName(i));
                }
                delete_row(s, sqlcmd, row);
                sqlcmd.Commit();
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
            #region
            //            string sql;
            //            lock (typeof(_TranRow))
            //            {
            //                if (sql_in_del == null)
            //                {
            //                    sqltool s = new sqltool();
            //                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select top(1) * from {0}1 nolock", this._tableName))
            //                        for (int i = 0; i < r.FieldCount; i++)
            //                            s["", r.GetName(i), ""] = (StringEx.sql_str)string.Format("[{0}]", r.GetName(i));
            //                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
            //                    s.Values["ModifyTime"] = (StringEx.sql_str)"getdate()";
            //                    s.Values["ModifyUser"] = (StringEx.sql_str)"{1}";
            //                    s.Values["TableName"] = (StringEx.sql_str)this._tableName;
            //                    s.Values["_ID"] = (StringEx.sql_str)"'{0}'";
            //                    s.Values["State_Rejected"] = TranState.Rejected;
            //                    sql_in_del = s.BuildEx(@"
            //update {TableName}1 set FinishTime=getdate(), State={State_Rejected} where ID={_ID} and FinishTime is null
            //select * from {TableName}1 nolock where ID={_ID}
            //insert into {TableName}2 (", sqltool._Fields, @")
            //select ", sqltool._Values, @"
            //from {TableName}1 nolock where ID={_ID}
            //delete {TableName}1 where ID={_ID}");
            //                }
            //                sql = sql_in_del;
            //            }
            //            try
            //            {
            //                sqlcmd.BeginTransaction();
            //                row = sqlcmd.ToObject<_TranRow>(sql, this.ID, (HttpContext.Current.User as User).ID);
            //                sqlcmd.Commit();
            //                row._RowDeleted = 1;
            //                return row;
            //            }
            //            catch
            //            {
            //                sqlcmd.Rollback();
            //                throw;
            //            }
            #endregion
        }

        internal protected void TranOut(SqlCmd sqlcmd, _TranRow row)
        {
            User user = HttpContext.Current.User as User;
            row.Amount = null;
            row.FinishTime = null;
            sqlcmd.FillObject(row, "select MemberID, Amount1-Amount2 as Amount, FinishTime from {0}1 nolock where ID='{1}'", this.TableName, row.ID);
            if (!row.Amount.HasValue)
                throw new RowException(RowErrorCode.TranNotFound);
            if ((row.Amount.Value == 0) || row.FinishTime.HasValue)
                return;
            try
            {
                sqlcmd.BeginTransaction();
                update_balance(sqlcmd, row, -row.Amount.Value);
                if (row.Amount.Value != 0)
                {
                    sqlcmd.FillObject(row, @"update {0}1 set Amount2=Amount2-({4}), [State]={3},ModifyTime=getdate(), ModifyUser={2} where ID='{1}' select * from {0}1 nolock where ID='{1}'"
                        , this.TableName, row.ID, user.ID, (int)TranState.Accepted, row.Amount);
                    this.WriteTranLog(sqlcmd, row, row.LogType.Value, row.Amount.Value);
                }
                sqlcmd.Commit();
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
            #region
            //            try
            //            {
            //                sqlcmd.BeginTransaction();
            //                row = sqlcmd.ToObject<_TranRow>(@"declare @MemberID int, @FinishTime datetime, @PrevBalance decimal(19,6), @Balance decimal(19,6), @AmountA decimal(19,6), @AmountB decimal(19,6)
            //select @MemberID=MemberID, @AmountA=Amount1-Amount2, @FinishTime=FinishTime from {0}1 nolock where ID='{1}'
            //if @FinishTime is null and @AmountA<>0
            //begin
            //    select @PrevBalance=Balance from Member nolock where ID=@MemberID
            //    update Member set Balance=Balance-@AmountA where ID=@MemberID and Balance>=@AmountA
            //    select @Balance=Balance from Member nolock where ID=@MemberID
            //    select @AmountB=@Balance-@PrevBalance
            //    if @AmountB <> 0 update {0}1 set Amount2=Amount2-@AmountB, [State]={2},ModifyTime=getdate(), ModifyUser={3} where ID='{1}'
            //end
            //select @PrevBalance as PrevBalance, @Balance as Balance, @AmountB as Amount, * from {0}1 nolock where ID='{1}'"
            //                    , this._tableName, this.ID, (int)TranState.Accepted, (HttpContext.Current.User as User).ID);
            //                if (row == null)
            //                    throw new RowException(RowErrorCode.TranNotFound);
            //                if (row.Amount.HasValue)
            //                {
            //                    this.WriteTranLog(sqlcmd, row, row.LogType.Value, row.Amount.Value);
            //                    sqlcmd.Commit();
            //                    return row;
            //                }
            //                else throw new RowException(RowErrorCode.MemberNotFound, null, row.MemberID, row.CorpID, row.MemberACNT);
            //            }
            //            catch
            //            {
            //                sqlcmd.Rollback();
            //                throw;
            //            }
            #endregion
        }

        internal protected void TranOut_confirm(SqlCmd sqlcmd, _TranRow row)
        {
            User user = HttpContext.Current.User as User;
            row.Amount2 = null;
            row.FinishTime = null;
            sqlcmd.FillObject(row, "select Amount2, FinishTime from {0}1 nolock where ID='{1}'", this.TableName, row.ID);
            if (!row.Amount2.HasValue)
                throw new RowException(RowErrorCode.TranNotFound);
            if ((row.Amount2.Value == 0) || row.FinishTime.HasValue)
                return;
            //try
            //{
            //    sqlcmd.BeginTransaction();
            sqlcmd.FillObject(true, row, "update {0}1 set Amount2=0,[State]={3},FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={2} where ID='{1}' select * from {0}1 nolock where ID='{1}'"
                , this.TableName, row.ID, user.ID, (int)(row.Amount2.Value == 0 ? TranState.Rejected : TranState.Transferred));
            //    sqlcmd.Commit();
            //    #region
            //                row = sqlcmd.ToObject<_TranRow>(@"declare @Amount2 decimal(19,6), @State int, @FinishTime datetime
            //select @Amount2=Amount2, @FinishTime=FinishTime from {0}1 nolock where ID='{1}'
            //if @FinishTime is null
            //begin
            //    if @Amount2=0 set @State={2} else set @State={3}
            //    update {0}1 set Amount2=0,[State]=@State,FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={4} where ID='{1}'
            //end
            //select * from {0}1 nolock where ID='{1}'"
            //                    , this._tableName, this.ID, (int)TranState.Rejected, (int)TranState.Transferred, (HttpContext.Current.User as User).ID);
            //                if (row == null)
            //                    throw new RowException(RowErrorCode.TranNotFound);
            //                sqlcmd.Commit();
            //                return row;
            //    #endregion
            //}
            //catch
            //{
            //    sqlcmd.Rollback();
            //    throw;
            //}
        }

        internal protected void TranOut_delete(SqlCmd sqlcmd, _TranRow row)
        {
            User user = HttpContext.Current.User as User;
            row.Amount2 = null;
            sqlcmd.FillObject(row, "select MemberID,Amount2 from {0}1 nolock where ID='{1}'", this.TableName, row.ID);
            if (!row.Amount2.HasValue)
                throw new RowException(RowErrorCode.TranNotFound);
            try
            {
                row.Amount = 0;
                if (row.Amount2.Value != 0)
                {
                    update_balance(sqlcmd, row, row.Amount2.Value);
                }
                sqltool s = new sqltool();
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update {0}1 set Amount2=0, State={3}, FinishTime=getdate(),ModifyTime=getdate(), ModifyUser={2} where ID='{1}' and FinishTime is null
select * from {0}1 nolock where ID='{1}'", this.TableName, row.ID, user.ID, (int)(row.Amount.Value == 0 ? TranState.Rejected : TranState.Failed)))
                {
                    r.FillObject(row);
                    for (int i = 0; i < r.FieldCount; i++)
                        s["", r.GetName(i), ""] = (StringEx.sql_str)string.Format("[{0}]", r.GetName(i));
                }
                delete_row(s, sqlcmd, row);
                if (row.Amount.Value != 0)
                    this.WriteTranLog(sqlcmd, row, this.LogType_Rollback ?? row.LogType.Value, row.Amount.Value);
                sqlcmd.Commit();
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
            #region
            //            string sql;
            //            lock (typeof(_TranRow))
            //            {
            //                if (sql_out_del == null)
            //                {
            //                    sqltool s = new sqltool();
            //                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select top(1) * from {0}1 nolock", this.TableName))
            //                        for (int i = 0; i < r.FieldCount; i++)
            //                            s["", r.GetName(i), ""] = (StringEx.sql_str)string.Format("[{0}]", r.GetName(i));
            //                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
            //                    s.Values["ModifyTime"] = (StringEx.sql_str)"getdate()";
            //                    s.Values["ModifyUser"] = (StringEx.sql_str)"{1}";
            //                    s.Values["TableName"] = (StringEx.sql_str)this.TableName;
            //                    s.Values["_ID"] = (StringEx.sql_str)"'{0}'";
            //                    s.Values["State_Transferred"] = TranState.Transferred;
            //                    s.Values["State_Rejected"] = TranState.Rejected;
            //                    s.Values["State_Failed"] = TranState.Failed;
            //                    sql_out_del = s.BuildEx(@"declare @MemberID int, @FinishTime datetime, @PrevBalance decimal(19,6), @Balance decimal(19,6), @AmountA decimal(19,6), @AmountB decimal(19,6), @State int
            //select @MemberID=MemberID, @AmountA=Amount2, @AmountB=0, @FinishTime=FinishTime from {TableName}1 nolock where ID={_ID}
            //if @FinishTime is null
            //begin
            //    if @AmountA<>0
            //    begin
            //        select @PrevBalance=Balance from Member nolock where ID=@MemberID
            //        update Member set Balance=Balance+@AmountA where ID=@MemberID
            //        select @Balance=Balance from Member nolock where ID=@MemberID
            //        select @AmountB=@Balance-@PrevBalance
            //    end
            //    if @AmountB <> 0 set @State={State_Failed} else set @State={State_Rejected}
            //    update {TableName}1 set Amount2=0, FinishTime=getdate(), State=@State where ID={_ID}
            //end
            //select @PrevBalance as PrevBalance, @Balance as Balance, @AmountB as Amount, * from {TableName}1 nolock where ID={_ID}
            //insert into {TableName}2 (", sqltool._Fields, @")
            //select ", sqltool._Values, @"
            //from {TableName}1 nolock where ID={_ID}
            //delete {TableName}1 where ID={_ID}");
            //                }
            //                sql = sql_out_del;
            //            }
            //            try
            //            {
            //                sqlcmd.BeginTransaction();
            //                row = sqlcmd.ToObject<_TranRow>(sql, this.ID, (HttpContext.Current.User as User).ID);
            //                if (row.Amount.HasValue)
            //                    this.WriteTranLog(sqlcmd, row, this.LogType_Rollback ?? row.LogType.Value, row.Amount.Value);
            //                sqlcmd.Commit();
            //                row._RowDeleted = 1;
            //                return row;
            //            }
            //            catch
            //            {
            //                sqlcmd.Rollback();
            //                throw;
            //            }
            #endregion
        }
    }

    public abstract class TranRow
    {
        [DbImport, JsonProperty]
        public Guid? ID;
        [DbImport, JsonProperty]
        public LogType? LogType;
        [DbImport, JsonProperty]
        public GameID? GameID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public int? MemberID;
        [DbImport, JsonProperty]
        public string MemberACNT;
        [DbImport, JsonProperty]
        public string MemberName;
        [DbImport, JsonProperty]
        public int AgentID;
        [DbImport, JsonProperty]
        public string AgentACNT;
        [DbImport, JsonProperty]
        public decimal? Amount1;
        [DbImport, JsonProperty]
        public decimal? Amount2;
        [DbImport, JsonProperty]
        public TranState? State;
        [DbImport, JsonProperty]
        public CurrencyCode? CurrencyA;
        [DbImport, JsonProperty]
        public CurrencyCode? CurrencyB;
        [DbImport, JsonProperty]
        public decimal? CurrencyX;
        [DbImport, JsonProperty]
        public string SerialNumber;
        [DbImport, JsonProperty]
        public string RequestIP;
        [DbImport, JsonProperty]
        public DateTime? FinishTime;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        [DbImport]
        public decimal? PrevBalance;
        [DbImport, JsonProperty]
        public decimal? Balance;
        [DbImport]
        public decimal? Amount;
        [DbImport, JsonProperty]
        public int? _RowDeleted;

        [DbImport]
        public int? __rowcount;
    }
}

namespace web
{
    //public class TranLogRow
    //{
    //    public static void TranLog(SqlCmd sqlcmd, DateTime actime, LogType logType, GameID gameID, int corpID, int memberID, string memberACNT, string memberName, int agentID, string agentACNT, string agentName, decimal prevBalance, decimal amount, decimal balance, CurrencyCode currencyA, CurrencyCode currencyB, decimal currencyX, string serialNumber, Guid? tranID, string requestIP, DateTime requestTime, DateTime? finishTime)
    //    {
    //        // INSERT INTO tengfa.dbo.TranLog (ACTime ,LogType ,GameID ,CorpID ,UserType ,UserID ,UserACNT ,UserName ,ParentID ,ParentACNT ,ParentName ,Amount ,Balance1 ,Balance2 ,CurrencyA ,CurrencyB ,CurrencyX ,SerialNumber ,TranID ,RequestIP ,RequestTime ,FinishTime ,CreateTime) 
    //        // VALUES (ACTime ,LogType ,GameID ,CorpID ,UserType ,UserID ,UserACNT ,UserName ,ParentID ,ParentACNT ,ParentName ,Amount ,Balance1 ,Balance2 ,CurrencyA ,CurrencyB ,CurrencyX ,SerialNumber ,TranID ,RequestIP ,RequestTime ,FinishTime ,CreateTime) 
    //        sqltool s = new sqltool();
    //        if (finishTime.HasValue)
    //        {
    //            s["", "CreateTime", "  "] = finishTime;
    //            s["", "ACTime    ", "  "] = finishTime.Value.ToACTime();
    //        }
    //        else
    //        {
    //            s["", "CreateTime", "  "] = (StringEx.sql_str)"getdate()";
    //            s["", "ACTime    ", "  "] = (StringEx.sql_str)"dateadd(dd,datediff(dd,0,dateadd(hh,-12,getdate())),0)";
    //        }
    //        s["", "ACTime        ", ""] = actime;
    //        s["", "LogType       ", ""] = logType;
    //        s["", "GameID        ", ""] = gameID;
    //        s["", "CorpID        ", ""] = corpID;
    //        s["", "MemberID      ", ""] = memberID;
    //        s["", "MemberACNT    ", ""] = memberACNT;
    //        s["", "MemberName    ", ""] = memberName;
    //        s["", "AgentID       ", ""] = agentID;
    //        s["", "AgentACNT     ", ""] = agentACNT;
    //        s["", "AgentName     ", ""] = agentName;
    //        s["", "PrevBalance   ", ""] = prevBalance;
    //        s["", "Amount        ", ""] = amount;
    //        s["", "Balance       ", ""] = balance;
    //        s["", "CurrencyA     ", ""] = currencyA;
    //        s["", "CurrencyB     ", ""] = currencyB;
    //        s["", "CurrencyX     ", ""] = currencyX;
    //        s["", "SerialNumber  ", ""] = serialNumber;
    //        s["", "TranID        ", ""] = tranID;
    //        s["", "RequestIP     ", ""] = requestIP;
    //        s["", "RequestTime   ", ""] = requestTime;
    //        s["", "FinishTime    ", ""] = finishTime;
    //        sqlcmd.ExecuteNonQuery(s.BuildEx(@"insert into TranLog (", sqltool._Fields, ") values (", sqltool._Values, ")"));
    //    }
    //}

    //public class MemberTranLogRow
    //{
    //    [DbImport, JsonProperty]
    //    public long? sn;
    //    [DbImport, JsonProperty]
    //    public DateTime? ACTime;
    //    [DbImport, JsonProperty]
    //    public LogType? LogType;
    //    [DbImport, JsonProperty]
    //    public GameID? GameID;
    //    [DbImport, JsonProperty]
    //    public int? CorpID;
    //    [DbImport, JsonProperty]
    //    public int? MemberID;
    //    [DbImport, JsonProperty]
    //    public string MemberACNT;
    //    [DbImport, JsonProperty]
    //    public int? AgentID;
    //    [DbImport, JsonProperty]
    //    public string AgentACNT;
    //    [DbImport, JsonProperty]
    //    public decimal? PrevBalance;
    //    [DbImport, JsonProperty]
    //    public decimal? Amount;
    //    [DbImport, JsonProperty]
    //    public decimal? Balance;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyA;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyB;
    //    [DbImport, JsonProperty]
    //    public decimal? CurrencyX;
    //    [DbImport, JsonProperty]
    //    public string SerialNumber;
    //    [DbImport, JsonProperty]
    //    public Guid? TranID;
    //    [DbImport, JsonProperty]
    //    public string RequestIP;
    //    [DbImport, JsonProperty]
    //    public DateTime? RequestTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? FinishTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? CreateTime;
    //}
    //public class AgentTranLogRow
    //{
    //    [DbImport, JsonProperty]
    //    public long? sn;
    //    [DbImport, JsonProperty]
    //    public DateTime? ACTime;
    //    [DbImport, JsonProperty]
    //    public LogType? LogType;
    //    [DbImport, JsonProperty]
    //    public GameID? GameID;
    //    [DbImport, JsonProperty]
    //    public int? CorpID;
    //    [DbImport, JsonProperty]
    //    public int? ParentID;
    //    [DbImport, JsonProperty]
    //    public string ParentACNT;
    //    [DbImport, JsonProperty]
    //    public int? AgentID;
    //    [DbImport, JsonProperty]
    //    public string AgentACNT;
    //    [DbImport, JsonProperty]
    //    public decimal? PrevBalance;
    //    [DbImport, JsonProperty]
    //    public decimal? Amount;
    //    [DbImport, JsonProperty]
    //    public decimal? Balance;
    //    [DbImport, JsonProperty]
    //    public decimal? ParentPCT;
    //    [DbImport, JsonProperty]
    //    public decimal? ParentAmount;
    //    [DbImport, JsonProperty]
    //    public decimal? AgentPCT;
    //    [DbImport, JsonProperty]
    //    public decimal? AgentAmount;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyA;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyB;
    //    [DbImport, JsonProperty]
    //    public decimal? CurrencyX;
    //    [DbImport, JsonProperty]
    //    public string SerialNumber;
    //    [DbImport, JsonProperty]
    //    public Guid? TranID;
    //    [DbImport, JsonProperty]
    //    public string RequestIP;
    //    [DbImport, JsonProperty]
    //    public DateTime? RequestTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? FinishTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? CreateTime;
    //}

    //public abstract class tranRow
    //{
    //    [DbImport, JsonProperty]
    //    public Guid? ID;
    //    [DbImport, JsonProperty]
    //    public LogType LogType;
    //    [DbImport, JsonProperty]
    //    public GameID GameID;
    //    [DbImport, JsonProperty]
    //    public int? CorpID;
    //    [DbImport, JsonProperty]
    //    public UserType? UserType;
    //    [DbImport, JsonProperty]
    //    public int? UserID;
    //    [DbImport, JsonProperty]
    //    public string UserACNT;
    //    [DbImport, JsonProperty]
    //    public string UserName;
    //    [DbImport, JsonProperty]
    //    public int? ParentID;
    //    [DbImport, JsonProperty]
    //    public string ParentACNT;
    //    [DbImport, JsonProperty]
    //    public string ParentName;
    //    [DbImport, JsonProperty]
    //    public decimal? Amount1;
    //    [DbImport, JsonProperty]
    //    public decimal? Amount2;
    //    [DbImport, JsonProperty]
    //    public TranState? State;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyA;
    //    [DbImport, JsonProperty]
    //    public CurrencyCode? CurrencyB;
    //    [DbImport, JsonProperty]
    //    public decimal? CurrencyX;
    //    [DbImport, JsonProperty]
    //    public string SerialNumber;
    //    [DbImport, JsonProperty]
    //    public string RequestIP;
    //    [DbImport, JsonProperty]
    //    public DateTime? FinishTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? CreateTime;
    //    [DbImport, JsonProperty]
    //    public int? CreateUser;
    //    [DbImport, JsonProperty]
    //    public DateTime? ModifyTime;
    //    [DbImport, JsonProperty]
    //    public int? ModifyUser;
    //}

    //public class TranBalanceRow : tranRow
    //{
    //    [DbImport, JsonProperty]
    //    public decimal? Fees1;
    //    [DbImport, JsonProperty]
    //    public decimal? Fees2;
    //    [DbImport, JsonProperty]
    //    public string a_BankName;
    //    [DbImport, JsonProperty]
    //    public string a_CardID;
    //    [DbImport, JsonProperty]
    //    public string a_Name;
    //    [DbImport, JsonProperty]
    //    public DateTime? a_TranTime;
    //    [DbImport, JsonProperty]
    //    public string a_TranSerial;
    //    [DbImport, JsonProperty]
    //    public string a_TranMemo;
    //    [DbImport, JsonProperty]
    //    public string b_BankName;
    //    [DbImport, JsonProperty]
    //    public string b_CardID;
    //    [DbImport, JsonProperty]
    //    public string b_Name;
    //    [DbImport, JsonProperty]
    //    public DateTime? b_TranTime;
    //    [DbImport, JsonProperty]
    //    public string b_TranSerial;
    //    [DbImport, JsonProperty]
    //    public string b_TranMemo;
    //    [DbImport, JsonProperty]
    //    public string Memo1;
    //    [DbImport, JsonProperty]
    //    public string Memo2;
    //}
    //public class TranBalanceRowCommand
    //{
    //}

    //public class TranGameRow : tranRow
    //{
    //}
    //public class TranGameRowCommand
    //{
    //}

    //public class TranPromoRow : tranRow
    //{
    //    [DbImport, JsonProperty]
    //    public string Memo1;
    //    [DbImport, JsonProperty]
    //    public string Memo2;

    //    // 首存優惠
    //    public Guid? 存款單號;
    //    public decimal? 流水要求;

    //    // 存款優惠
    //    public decimal? 存款金額;

    //    // 洗碼優惠
    //    public DateTime? 起始時間;
    //    public DateTime? 結束時間;
    //    public decimal? 總投注額;
    //    public decimal? 有效投注額;
    //    public decimal? 洗碼比率;
    //}
    //public class TranPromoRowCommand
    //{
    //}
}