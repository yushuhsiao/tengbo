using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using Tools;

namespace BU
{
    public class Permissions
    {
        [Flags]
        public enum Flag : int
        {
            Read = 0x01,
            Write = 0x02,
        }

        public static class Code
        {
            public const string admin1 = "admin1";
            public const string admin2 = "admin2";
            public const string admin3 = "admin3";
            public const string member1 = "member1";
            public const string member2 = "member2";
            public const string member3 = "member3";
            public const string agent1 = "agent1";
            public const string agent2 = "agent2";
            public const string agent3 = "agent3";
            public const string develover = "develover";
            public const string admins_list = "admins_list";
            public const string agents_list = "agents_list";
            public const string members_list = "members_list";
            public const string admingroup_list = "admingroup_list";
            public const string agentgroup_list = "agentgroup_list";
            public const string membergroup_list = "membergroup_list";
            public const string member_loginhist = "member_loginhist";
            public const string agent_loginhist = "agent_loginhist";
            public const string admin_loginhist = "admin_loginhist";
            public const string menu_edit = "menu_edit";
            public const string admingroup_permission = "admingroup_permission";
            public const string agentgroup_permission = "agentgroup_permission";
            public const string membergroup_permission = "membergroup_permission";
            public const string tran_member_d = "tran_member_d";
            public const string tran_member_w = "tran_member_w";
            public const string tran_game_d = "tran_game_d";
            public const string tran_game_w = "tran_game_w";
            public const string tran_promo = "tran_promo";
            public const string tran_bankcard = "tran_bankcard";
            public const string tran_banklist = "tran_banklist";
            public const string tran_bankcard_mgt = "tranmgt_bankcard";
            public const string tran_thirdpay_mgt = "tranmgt_thirdpay";
            public const string tran_dinpay_mgt = "tranmgt_dinpay";
            public const string tranhist_member_d = "tranhist_member_d";
            public const string tranhist_member_w = "tranhist_member_w";
            public const string tranhist_game_d = "tranhist_game_d";
            public const string tranhist_game_w = "tranhist_game_w";
            public const string tranhist_promo = "tranhist_promo";
            public const string m_anno = "m_anno";
            public const string m_billboard = "m_billboard";
            public const string config_edit = "config_edit";
            public const string corp_list = "corp_list";
            public const string game_list = "game_list";
            public const string gametype_list = "gametype_list";
            public const string m_workingnote0 = "m_workingnote0";
            public const string m_workingnote1 = "m_workingnote1";
            public const string m_workingnote2 = "m_workingnote2";
            public const string m_workingnote3 = "m_workingnote3";
            public const string m_workingnote4 = "m_workingnote4";
            public const string log_user_day = "log_user_day";
            //public const string log_000 = "log_000";
            //public const string log_001 = "log_001";
            public const string log_betamtdg = "log_betamtdg";
            public const string member_subacc = "member_subacc";
            public const string agent_subacc = "agent_subacc";
            public const string lang_edit = "lang_edit";
            public const string load_agent_balance = "load_agent_balance";
            public const string load_member_balance = "load_member_balance";

            public const string web_member = "web_member";

            public const string tran_thirdpay = "tran_thirdpay";
            public const string tran_thirdpay_hist = "tran_thirdpay_hist";
            public const string tran_thirdpay_config = "tran_thirdpay_config";

            public const string tran_member_deposit = "tranm_deposit";
            public const string tran_member_deposit_hist = "tranm_deposit_hist";
            public const string tran_member_withdrawal = "tranm_withdrawal";
            public const string tran_member_withdrawal_hist = "tranm_withdrawal_hist";
            public const string tran_member_gamedeposit = "tranm_gamedeposit";
            public const string tran_member_gamedeposit_hist = "tranm_gamedeposit_hist";
            public const string tran_member_gamewithdrawal = "tranm_gamewithdrawal";
            public const string tran_member_gamewithdrawal_hist = "tranm_gamewithdrawal_hist";
            public const string tran_agent_deposit = "trana_deposit";
            public const string tran_agent_deposit_hist = "trana_deposit_hist";
            public const string tran_agent_withdrawal = "trana_withdrawal";
            public const string tran_agent_withdrawal_hist = "trana_withdrawal_hist";
            public const string tran_agent_gamedeposit = "trana_gamedeposit";
            public const string tran_agent_gamedeposit_hist = "trana_gamedeposit_hist";
            public const string tran_agent_gamewithdrawal = "trana_gamewithdrawal";
            public const string tran_agent_gamewithdrawal_hist = "trana_gamewithdrawal_hist";
            public const string promo_member_betamt = "promom_betamt";
            public const string promo_member_betamt_hist = "promom_betamt_hist";
            public const string promo_member_first_deposit = "promom_deposit1";
            public const string promo_member_first_deposit_hist = "promom_deposit1_hist";
            public const string promo_member_deposit = "promom_deposit";
            public const string promo_member_deposit_hist = "promom_deposit_hist";
            public const string promo_member_other = "promom_other";
            public const string promo_member_other_hist = "promom_other_hist";
            public const string promo_agent_betamt = "promoa_betamt";
            public const string promo_agent_betamt_hist = "promoa_betamt_hist";
            public const string promo_agent_share = "promoa_share";
            public const string promo_agent_share_hist = "promoa_share_hist";
            public const string promo_agent_other = "promoa_other";
            public const string promo_agent_other_hist = "promoa_other_hist";
        }

        Dictionary<string, Flag> m_Codes = new Dictionary<string, Flag>();
        Dictionary<string, Flag> Codes
        {
            get { return Interlocked.CompareExchange(ref this.m_Codes, null, null); }
            set { Interlocked.Exchange(ref this.m_Codes, value); }
        }

        int m_UserID;
        int UserID
        {
            get { return Interlocked.CompareExchange(ref this.m_UserID, 0, 0); }
            set { Interlocked.Exchange(ref this.m_UserID, value); }
        }

        //byte m_GroupID;
        //byte GroupID
        //{
        //    get { return Interlocked.CompareExchange(ref this.m_GroupID, 0, 0); }
        //    set { Interlocked.Exchange(ref this.m_GroupID, value); }
        //}

        public bool this[string code, Flag flag]
        {
            get
            {
                if (this.UserID == web._Global.RootAdminID)
                    return true;
                Flag f;
                if (this.Codes.TryGetValue(code, out f))
                    return (flag & f) != 0;
                return false;
            }
        }

        public bool this[string code]
        {
            get
            {
                if (this.UserID == web._Global.RootAdminID)
                    return true;
                Flag f; if (this.Codes.TryGetValue(code, out f)) return f != 0;
                return false;
            }
        }

        public bool IsNull
        {
            get { return this == _null<Permissions>.value; }
        }

        public int Load(SqlCmd sqlcmd, Guid? groupID, int userID)
        {
            if (this.IsNull)
                return 0;
            Dictionary<string, Flag> codes = new Dictionary<string, Flag>();
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"
select a.*, isnull(b.Flag,0) as Flag from Permission1 a with(nolock)
left join Permission2 b with(nolock) on a.ID=b.CodeID and b.GroupID='{0}'
select a.*, isnull(c.Flag,0) as Flag from Permission1 a with(nolock)
left join Permission3 c with(nolock) on a.ID=c.CodeID and c.UserID={1}", groupID, userID))
                {
                    string code = r.GetString("Code");
                    Flag f = (Flag)r.GetInt32("Flag");
                    if (codes.ContainsKey(code))
                        codes[code] |= f;
                    else
                        codes[code] = f;
                }
                this.UserID = userID;
                //this.GroupID = groupID;
                this.Codes = codes;
                return codes.Count;
            }
        }

        public bool Check(MethodInfo m)
        {
            if (web._Global.DebugMode2 == true)
                return true;
            PermissionsAttribute[] a = (PermissionsAttribute[])m.GetCustomAttributes(typeof(PermissionsAttribute), false);
            if (a.Length == 0)
                a = (PermissionsAttribute[])m.DeclaringType.GetCustomAttributes(typeof(PermissionsAttribute), false);
            for (int i = 0; i < a.Length; i++)
                if (this[a[i].Code, a[i].Flag] == false)
                    return false;
            //foreach (PermissionsAttribute p in m.GetCustomAttributes(typeof(PermissionsAttribute), false))
            //    if (this[p.Code, p.Flag] == false)
            //        return false;
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionsAttribute : Attribute
    {
        public string Code { get; set; }
        public Permissions.Flag Flag { get; set; }
        public PermissionsAttribute(string code, Permissions.Flag flag) { this.Code = code; this.Flag = flag; }
    }

    //public class Permissions2
    //{
    //    [Flags]
    //    public enum Flag : int
    //    {
    //        None = 0
    //    }

    //    public static readonly Permissions2 Null = new Permissions2();

    //    public Permissions2()
    //    {
    //    }

    //    public int Import(SqlCmd sqlcmd, string format, params object[] args)
    //    {
    //        if (this == Null)
    //            return 0;
    //        Dictionary<int, Flag> dict = new Dictionary<int, Flag>();
    //        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(format, args))
    //            dict[r.GetInt32("Code")] = (Flag)r.GetInt32("Flag");
    //        Interlocked.Exchange(ref this._dict, dict);
    //        return dict.Count;
    //    }

    //    Dictionary<int, Flag> _dict = new Dictionary<int, Flag>();

    //    public bool this[int code]
    //    {
    //        get { return Interlocked.CompareExchange(ref this._dict, null, null).ContainsKey(code); }
    //    }
    //    public bool this[int code, Flag flag]
    //    {
    //        get
    //        {
    //            Flag f;
    //            if (Interlocked.CompareExchange(ref this._dict, null, null).TryGetValue(code, out f))
    //                return (f & flag) == f;
    //            return false;
    //        }
    //    }
    //    public bool this[Code code]
    //    {
    //        get { return this[(int)code]; }
    //    }
    //    public bool this[Code code, Flag flag]
    //    {
    //        get { return this[(int)code, flag]; }
    //    }

    //    public enum Code : int
    //    {
    //        CreateMember = 10001,
    //        CreateAdmin = 10002,
    //        DebugUser = 0x7fff,
    //    }
    //}

    //class PermissionList
    //{
    //    Dictionary<PermissionCode, string> list = new Dictionary<PermissionCode, string>();

    //    public virtual bool this[PermissionCode code]
    //    {
    //        get { return this.list.ContainsKey(code); }
    //    }
    //    public virtual bool this[PermissionCode code, char flag]
    //    {
    //        get
    //        {
    //            if (this.list.ContainsKey(code))
    //                return this.list[code].IndexOf(flag) > 0;
    //            return false;
    //        }
    //    }

    //    public static PermissionList GetPermissions()
    //    {
    //        return new PermissionList();
    //    }

    //    private PermissionList()
    //    {
    //    }

    //    public static readonly PermissionList Root = new root();
    //    public static readonly PermissionList Guest = new guest();

    //    class root : PermissionList
    //    {
    //    }
    //    class guest : PermissionList
    //    {
    //        public guest()
    //        {
    //            base.list.Add(PermissionCode.admin_login, "");
    //            base.list.Add(PermissionCode.agent_login, "");
    //            base.list.Add(PermissionCode.member_login, "");
    //        }
    //    }
    //}
    //enum PermissionCode : int
    //{
    //    Unknown = 0,
    //    admin_login = 1,
    //    agent_login = 2,
    //    member_login = 3,
    //    admin_create,
    //    admin_active,
    //    admin_lock,
    //    admin_unlock,
    //}
}