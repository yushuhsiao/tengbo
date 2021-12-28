using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using System;
using web;

// 上級存入/提出點數
//abstract class ParentTran<T, TUserRow> : IRowCommand
//    where T : ParentTran<T, TUserRow>
//    where TUserRow : UserRow2, new()
//{
//    public abstract UserType UserType { get; }

//    protected abstract string _TableName { get; }
//    protected abstract string _ParentField { get; }
//    protected abstract RowErrorCode _ErrorNotFound { get; }

//    [JsonProperty]
//    public LogType? LogType;
//    public abstract int? UserID { get; }
//    [JsonProperty]
//    public decimal? Amount;

//    protected T execute(string json_s, params object[] args)
//    {
//        if (this.Amount <= 0)
//            this.Amount = null;
//        this.Amount = this.Amount ?? 0;
//        if (this.Amount == 0)
//            throw new RowException(RowErrorCode.FieldNeeds, "Amount");
//        if (!this.LogType.In(BU.LogType.ParentDeposit, BU.LogType.ParentWithdrawal))
//            throw new RowException(RowErrorCode.FieldNeeds, "LogType");

//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//        {
//            TUserRow user2, user1 = sqlcmd.GetRowEx<TUserRow>(this._ErrorNotFound, "select ID, CorpID, ACNT, [Name], Balance, {1} from {0} nolock where ID={2}", this._TableName, this._ParentField, this.UserID);
//            AgentRow parent2, parent1 = sqlcmd.GetRowEx<AgentRow>(RowErrorCode.AgentNotFound, "select ID, CorpID, ACNT, [Name], Balance, ParentID from Agent nolock where ID={0} and CorpID={1}", user1.ParentID, user1.CorpID);
//            try
//            {
//                string sql;
//                if (this.LogType.Value == BU.LogType.ParentDeposit)
//                {
//                    if (parent1.Balance.Value < 0)
//                        throw new RowException(RowErrorCode.BalanceNotEnough);
//                    sql = @"update {0} set Balance=Balance+{3} where ID={1}
//update Agent set Balance=Balance-{3} where ID={2} --and Balance>0";
//                }
//                else
//                {
//                    if (user1.Balance.Value < 0)
//                        throw new RowException(RowErrorCode.BalanceNotEnough);
//                    sql = @"update {0} set Balance=Balance-{3} where ID={1} --and Balance>0
//update Agent set Balance=Balance+{3} where ID={2}";
//                }
//                sqlcmd.BeginTransaction();
//                sqlcmd.ExecuteNonQuery(sql, this._TableName, user1.ID, user1.ParentID, this.Amount);
//                user2 = sqlcmd.ToObject<TUserRow>("select Balance from {0} nolock where ID={1}", this._TableName, user1.ID);
//                parent2 = sqlcmd.ToObject<AgentRow>("select Balance from Agent nolock where ID={0}", user1.ParentID);
//                if (user2.Balance.Value < 0)
//                    throw new RowException(RowErrorCode.BalanceNotEnough);
//                if (parent2.Balance.Value < 0)
//                    throw new RowException(RowErrorCode.BalanceNotEnough);
//                sqlcmd.Rollback();
//                log.message(null, "\r\n{0}\r\n{1}\r\n{2}\r\n{3}"
//                    , api.SerializeObject(user1)
//                    , api.SerializeObject(user2)
//                    , api.SerializeObject(parent1)
//                    , api.SerializeObject(parent2));
//            }
//            catch
//            {
//                sqlcmd.Rollback();
//                throw;
//            }
//        }
//        return (T)this;
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ParentTranA : ParentTran<ParentTranA, AgentRow>, IRowCommand
//{
//    public override UserType UserType
//    {
//        get { return BU.UserType.Agent; }
//    }

//    protected override string _TableName
//    {
//        get { return "Agent"; }
//    }

//    protected override string _ParentField
//    {
//        get { return "ParentID"; }
//    }

//    protected override RowErrorCode _ErrorNotFound
//    {
//        get { return RowErrorCode.AgentNotFound; }
//    }

//    [JsonProperty]
//    public int? AgentID;

//    public override int? UserID
//    {
//        get { return this.AgentID; }
//    }

//    [ObjectInvoke, Permissions("child_agent_balance", Permissions.Flag.Write)]
//    object execute(ParentTranA command, string json_s, params object[] args)
//    {
//        return base.execute(json_s, args);
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ParentTranM : ParentTran<ParentTranM, MemberRow>, IRowCommand
//{
//    public override UserType UserType
//    {
//        get { return BU.UserType.Member; }
//    }

//    protected override string _TableName
//    {
//        get { return "Member"; }
//    }

//    protected override string _ParentField
//    {
//        get { return "AgentID"; }
//    }

//    protected override RowErrorCode _ErrorNotFound
//    {
//        get { return RowErrorCode.MemberNotFound; }
//    }

//    [JsonProperty]
//    public int? MemberID;

//    public override int? UserID
//    {
//        get { return this.MemberID; }
//    }

//    [ObjectInvoke, Permissions("child_member_balance", Permissions.Flag.Write)]
//    object execute(ParentTranM command, string json_s, params object[] args)
//    {
//        return base.execute(json_s, args);
//    }
//}

#region
// 
#endregion

//class ParentTran
//{
//    [JsonProperty]
//    public LogType? LogType;
//    [JsonProperty]
//    public decimal? Amount;

//    protected void check_args()
//    {
//        if (this.Amount <= 0)
//            this.Amount = null;
//        if (this.LogType == BU.LogType.ParentWithdrawal)
//            this.Amount *= -1;
//        else if (this.LogType == BU.LogType.ParentDeposit)
//        { }
//        else
//            throw new RowException(RowErrorCode.FieldNeeds);
//        if (!this.Amount.HasValue)
//            throw new RowException(RowErrorCode.FieldNeeds);
//    }

//    //protected const string sql_getagent = "select ID, CorpID, ACNT, [Name], Balance, ParentID from Agent nolock where ID={0}";
//    //protected const string sql_getmember= "select ID, CorpID, ACNT, [Name], Balance, AgentID from Member nolock where ID={0}";
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ParentTranA : ParentTran, IRowCommand
//{
//    [JsonProperty]
//    public int? AgentID;
//    [JsonProperty]
//    public AgentTranLogRow dst;
//    [JsonProperty]
//    public AgentTranLogRow src;

//    [ObjectInvoke, Permissions(Permissions.Code.child_agent_balance, Permissions.Flag.Write)]
//    object execute(ParentTranA command, string json_s, params object[] args)
//    {
//        check_args();
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//        {
//            dst = sqlcmd.GetRowEx<AgentTranLogRow>(RowErrorCode.AgentNotFound, "select ID as AgentID, ACNT as AgentACNT, ParentID from Agent nolock where ID={AgentID}".SqlExport(null, this));
//            src = sqlcmd.GetRowEx<AgentTranLogRow>(RowErrorCode.ParentAgentNotFound, "select ID as AgentID, ACNT as AgentACNT, ParentID from Agent nolock where ID={ParentID} and ID<>0".SqlExport(null, dst));
//            dst.Amount = command.Amount;
//            dst.AgentACNT = src.AgentACNT;
//            try
//            {
//                sqlcmd.BeginTransaction();
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @b1 decimal(19,6), @b2 decimal(19,6)
//select @b1=Balance from Agent nolock where ID={AgentID}
//select @b2=Balance from Agent nolock where ID={ParentID}
//update Agent set Balance=Balance+({Amount}) where ID={AgentID}
//update Agent set Balance=Balance-({Amount}) where ID={ParentID}
//select ID, @b1 as PrevBalance, Balance, Balance-@b1 as Amount from Agent nolock where ID={AgentID}
//select ID, @b2 as PrevBalance, Balance, Balance-@b2 as Amount from Agent nolock where ID={ParentID}".SqlExport(null, dst)))
//                    r.FillObject(r.GetInt32("ID") == src.AgentID ? (object)src : dst);
//                if ((dst.Balance < 0) && (command.Amount < 0))
//                    throw new RowException(RowErrorCode.AgentBalanceNotEnough);
//                if (src.Balance < 0)
//                    throw new RowException(RowErrorCode.ParentAgentBalanceNotEnough);
//                sqlcmd.FillObject(src, "select ID as ParentID, ACNT as ParentACNT from Agent nolock where ID={ParentID}".SqlExport(null, src));
//                sqlcmd.Commit();
//            }
//            catch
//            {
//                sqlcmd.Rollback();
//                throw;
//            }
//            return this;
//        }
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ParentTranM : ParentTran, IRowCommand
//{
//    [JsonProperty]
//    public int? MemberID;
//    [JsonProperty]
//    public MemberTranLogRow dst;
//    [JsonProperty]
//    public AgentTranLogRow src;

//    [ObjectInvoke, Permissions(Permissions.Code.child_member_balance, Permissions.Flag.Write)]
//    object execute(ParentTranM command, string json_s, params object[] args)
//    {
//        check_args();
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//        {
//            dst = sqlcmd.GetRowEx<MemberTranLogRow>(RowErrorCode.MemberNotFound, "select ID as MemberID, ACNT as MemberACNT, AgentID from Member nolock where ID={MemberID}".SqlExport(null, this));
//            src = sqlcmd.GetRowEx<AgentTranLogRow>(RowErrorCode.ParentAgentNotFound, "select ID as AgentID, ACNT as AgentACNT, ParentID from Agent nolock where ID={AgentID} and ID<>0".SqlExport(null, dst));
//            dst.Amount = command.Amount;
//            dst.AgentACNT = src.AgentACNT;
//            try
//            {
//                sqlcmd.BeginTransaction();
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @b1 decimal(19,6), @b2 decimal(19,6)
//select @b1=Balance from Member nolock where ID={MemberID}
//select @b2=Balance from Agent  nolock where ID={AgentID}
//update Member set Balance=Balance+({Amount}) where ID={MemberID}
//update Agent  set Balance=Balance-({Amount}) where ID={AgentID}
//select ID, @b1 as PrevBalance, Balance, Balance-@b1 as Amount from Member nolock where ID={MemberID}
//select ID, @b2 as PrevBalance, Balance, Balance-@b2 as Amount from Agent  nolock where ID={AgentID}".SqlExport(null, dst)))
//                    r.FillObject(r.GetInt32("ID") == src.AgentID ? (object)src : dst);
//                if ((dst.Balance < 0) && (command.Amount < 0))
//                    throw new RowException(RowErrorCode.MemberBalanceNotEnough);
//                if (src.Balance < 0)
//                    throw new RowException(RowErrorCode.ParentAgentBalanceNotEnough);
//                sqlcmd.FillObject(src, "select ID as ParentID, ACNT as ParentACNT from Agent nolock where ID={ParentID}".SqlExport(null, src));
//                sqlcmd.Commit();
//            }
//            catch
//            {
//                sqlcmd.Rollback();
//                throw;
//            }
//            return this;
//        }
//    }
//}