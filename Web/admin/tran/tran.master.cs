using BU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tools;
using web;

namespace web
{
    #region 上分/卸分

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class UserLoadBalance : tran.UserLoadBalance, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.load_agent_balance, Permissions.Flag.Write)]
        static object execute(UserLoadBalance command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }
    }

    #endregion

    #region 會員存款

    public class MemberDeposit_master : tran.Cash.DepositRowCommand.tran_master<MemberRow, MemberDeposit_master.MemberDeposit_Select, MemberDeposit_master.MemberDeposit_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_member_deposit;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_member_deposit_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberDeposit_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberDeposit_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberDeposit_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberDeposit_Update : tran.Cash.DepositRowCommand, IRowCommand { }
    }

    #endregion
    #region 會員提款

    public class MemberWithdrawal_master : tran.Cash.WithdrawalRowCommand.tran_master<MemberRow, MemberWithdrawal_master.MemberWithdrawal_Select, MemberWithdrawal_master.MemberWithdrawal_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_member_withdrawal;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_member_withdrawal_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberWithdrawal_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberWithdrawal_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberWithdrawal_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberWithdrawal_Update : tran.Cash.WithdrawalRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理存款

    public class AgentDeposit_master : tran.Cash.DepositRowCommand.tran_master<AgentRow, AgentDeposit_master.AgentDeposit_Select, AgentDeposit_master.AgentDeposit_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_agent_deposit;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_agent_deposit_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentDeposit_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentDeposit_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentDeposit_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentDeposit_Update : tran.Cash.DepositRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理提款

    public class AgentWithdrawal_master : tran.Cash.WithdrawalRowCommand.tran_master<AgentRow, AgentWithdrawal_master.AgentWithdrawal_Select, AgentWithdrawal_master.AgentWithdrawal_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_agent_withdrawal;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_agent_withdrawal_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentWithdrawal_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentWithdrawal_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentWithdrawal_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentWithdrawal_Update : tran.Cash.WithdrawalRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 第三方支付

    public class ThirdPayment_master : tran.Cash.ThirdPaymentRowCommand.tran_master<AdminRow, ThirdPayment_master.ThirdPayment_Select, ThirdPayment_master.ThirdPayment_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_thirdpay;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_thirdpay_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(ThirdPayment_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(ThirdPayment_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class ThirdPayment_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class ThirdPayment_Update : tran.Cash.ThirdPaymentRowCommand, IRowCommand { }
    }
    
    #endregion

    #region 會員轉帳到遊戲

    public class MemberGameDeposit_master : tran.Game.GameDepositRowCommand.tran_master<MemberRow, MemberGameDeposit_master.MemberGameDeposit_Select, MemberGameDeposit_master.MemberGameDeposit_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_member_gamedeposit;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_member_gamedeposit_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberGameDeposit_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberGameDeposit_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberGameDeposit_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberGameDeposit_Update : tran.Game.GameDepositRowCommand, IRowCommand { }
    }

    #endregion
    #region 會員轉帳到遊戲

    public class MemberGameWithdrawal_master : tran.Game.GameWithdrawalRowCommand.tran_master<MemberRow, MemberGameWithdrawal_master.MemberGameWithdrawal_Select, MemberGameWithdrawal_master.MemberGameWithdrawal_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_member_gamewithdrawal;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_member_gamewithdrawal_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberGameWithdrawal_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberGameWithdrawal_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberGameWithdrawal_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberGameWithdrawal_Update : tran.Game.GameWithdrawalRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理轉帳到遊戲

    public class AgentGameDeposit_master : tran.Game.GameDepositRowCommand.tran_master<AgentRow, AgentGameDeposit_master.AgentGameDeposit_Select, AgentGameDeposit_master.AgentGameDeposit_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_agent_gamedeposit;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_agent_gamedeposit_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentGameDeposit_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentGameDeposit_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentGameDeposit_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentGameDeposit_Update : tran.Game.GameDepositRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理轉帳到遊戲

    public class AgentGameWithdrawal_master : tran.Game.GameWithdrawalRowCommand.tran_master<AgentRow, AgentGameWithdrawal_master.AgentGameWithdrawal_Select, AgentGameWithdrawal_master.AgentGameWithdrawal_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.tran_agent_gamewithdrawal;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.tran_agent_gamewithdrawal_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentGameWithdrawal_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentGameWithdrawal_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentGameWithdrawal_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentGameWithdrawal_Update : tran.Game.GameWithdrawalRowCommand, IRowCommand { }
    }
    
    #endregion

    #region 會員優惠

    public class MemberPromo_master : tran.Promo.PromoRowCommand.tran_master<MemberRow, MemberPromo_master.MemberPromo_Select, MemberPromo_master.MemberPromo_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_member_other;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_member_other_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberPromo_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberPromo_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberPromo_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberPromo_Update : tran.Promo.PromoRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理優惠

    public class AgentPromo_master : tran.Promo.PromoRowCommand.tran_master<AgentRow, AgentPromo_master.AgentPromo_Select, AgentPromo_master.AgentPromo_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_agent_other;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_agent_other_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentPromo_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentPromo_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentPromo_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentPromo_Update : tran.Promo.PromoRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 會員洗碼

    public class MemberBetAmt_master : tran.Promo.BetAmtRowCommand.tran_master<MemberRow, MemberBetAmt_master.MemberBetAmt_Select, MemberBetAmt_master.MemberBetAmt_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_member_betamt;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_member_betamt_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberBetAmt_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberBetAmt_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberBetAmt_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberBetAmt_Update : tran.Promo.BetAmtRowCommand, IRowCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberBetAmt_Import : IRowCommand
        {
            [JsonProperty]
            public int[] CorpID;
            [JsonProperty]
            public UserType? UserType;
            [JsonProperty]
            public DateTime? ACTime;

            [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
            static object execute(MemberBetAmt_Import command, string json_s, params object[] args)
            {
                int cnt = 0;
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read), sqlcmd2 = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    foreach (int corpID in command.CorpID)
                    {
                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select *,BetAmountAct * BonusRate as BetBonus from (
select a.*, b.CorpID, b.ACNT, case when a.Payout>0 then c.BonusW else c.BonusL end as BonusRate
from (select ACTime, MemberID, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
where ACTime='{0:yyyy-MM-dd}' and CorpID={1}
group by ACTime, MemberID) a
left join Member b with(nolock) on a.MemberID=b.ID
left join MemberGroup c with(nolock) on b.CorpID=c.CorpID and b.GroupID=c.ID) x", command.ACTime, corpID))
                        {
                            MemberBetAmt_Update cmd = new MemberBetAmt_Update()
                            {
                                op_Insert = true,
                                ACTime1 = command.ACTime,
                                ACTime2 = command.ACTime,
                                LogType = BU.LogType.BetAmt,
                                UserType = BU.UserType.Member,
                                CorpID = r.GetInt32("CorpID"),
                                UserACNT = r.GetString("ACNT"),
                                UserID = r.GetInt32("MemberID"),
                                Amount = Math.Round(r.GetDecimal("BetBonus"), 2, MidpointRounding.AwayFromZero),
                                BetAmount1 = r.GetDecimal("BetAmount"),
                                BetAmount2 = r.GetDecimal("BetAmountAct"),
                                BetPayout = r.GetDecimal("Payout"),
                                Rate = r.GetDecimal("BonusRate"),
                            };
                            try { cmd.Execute(sqlcmd2, json_s, args); cnt++; }
                            catch { }
                        }
                    }
                }
                return cnt;
            }
        }
    }
    
    #endregion
    #region 代理洗碼

    public class AgentBetAmt_master : tran.Promo.BetAmtRowCommand.tran_master<AgentRow, AgentBetAmt_master.AgentBetAmt_Select, AgentBetAmt_master.AgentBetAmt_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_agent_betamt;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_agent_betamt_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentBetAmt_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentBetAmt_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentBetAmt_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentBetAmt_Update : tran.Promo.BetAmtRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 會員首存

    public class MemberFirstDeposit_master : tran.Promo.FirstDepositRowCommand.tran_master<MemberRow, MemberFirstDeposit_master.MemberFirstDeposit_Select, MemberFirstDeposit_master.MemberFirstDeposit_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_member_first_deposit;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_member_first_deposit_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(MemberFirstDeposit_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(MemberFirstDeposit_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberFirstDeposit_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MemberFirstDeposit_Update : tran.Promo.FirstDepositRowCommand, IRowCommand { }
    }
    
    #endregion
    #region 代理佣金

    public class AgentShare_master : tran.Promo.AgentShareRowCommand.tran_master<AgentRow, AgentShare_master.AgentShare_Select, AgentShare_master.AgentShare_Update>
    {
        protected override string link_key1 { get { return KEY1; } } public const string KEY1 = BU.Permissions.Code.promo_agent_share;
        protected override string link_key2 { get { return KEY2; } } public const string KEY2 = BU.Permissions.Code.promo_agent_share_hist;

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Read), Permissions(KEY2, Permissions.Flag.Read)]
        static object execute(AgentShare_Select command, string json_s, params object[] args) { return command.select(json_s, args); }

        [ObjectInvoke, Permissions(KEY1, Permissions.Flag.Write), Permissions(KEY2, Permissions.Flag.Write)]
        static object execute(AgentShare_Update command, string json_s, params object[] args) { return command.Execute(null, json_s, args); }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentShare_Select : SelectCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class AgentShare_Update : tran.Promo.AgentShareRowCommand, IRowCommand { }
    }
    
    #endregion
}