using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

namespace web
{
    public class AdminGroupRowCommand
    {
        [JsonProperty]
        public virtual string ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual int? GroupID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }

        public AdminGroupRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                long? id = this.ID.ToInt64();
                int? corpID = text.GroupRowID_CorpID(id);
                byte? groupID = text.GroupRowID_GroupID(id);
                AdminGroupRow row = sqlcmd.GetRowEx<AdminGroupRow>(RowErrorCode.NotFound, "select * from AdminGroup nolock where CorpID={0} and GroupID={1}", corpID, groupID);
                sqltool s = new sqltool();
                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
                s[" ", "GroupID", "", row.GroupID, ""] = this.GroupID;
                s["N", "Name", "   ", row.Name, "   "] = text.ValidAsString * this.Name;
                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["_CorpID"] = corpID;
                s.Values["_GroupID"] = groupID;
                s.Values["CorpID"] = this.CorpID;
                s.Values["GroupID"] = this.GroupID;
                string sqlstr = s.BuildEx("update AdminGroup set ", sqltool._FieldValue, " where CorpID={_CorpID} and GroupID={_GroupID} select * from AdminGroup nolock where CorpID={CorpID} and GroupID={GroupID}");
                //string sqlstr = s.BuildEx(s.Values.ContainsKey("ACNT") ? 
                //    "update Corp set Corp.AdminGroupACNT={ACNT} from AdminGroup where Corp.ID=AdminGroup.CorpID and Corp.AdminGroupACNT=AdminGroup.ACNT and AdminGroup.ID={ID} " : "",
                //    "update AdminGroup set ", sqltool._FieldValue, " where ID={ID} select * from AdminGroup nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AdminGroupRow>(sqlstr);
            }
        }

        public AdminGroupRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "CorpID", "      "] = this.CorpID;
            s["* ", "GroupID", "     "] = this.GroupID;
            s["*N", "Name", "        "] = text.ValidAsString * this.Name;
            s["  ", "Locked", "      "] = this.Locked ?? BU.Locked.Active;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.Values["CorpID"] = (StringEx.sql_str)"ID";
            s.values["CorpID_"] = this.CorpID;
            string sqlstr = s.BuildEx(@"insert into AdminGroup (", sqltool._Fields, @")
select ", sqltool._Values, @" from Corp nolock where ID={CorpID_}
select * from AdminGroup nolock where CorpID={CorpID_} and GroupID={GroupID}");
            SqlCmd sqlcmd, _sqlcmd = args.GetValue<SqlCmd>(0);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
            {
                if (sqlcmd == _sqlcmd)
                    return sqlcmd.ToObject<AdminGroupRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<AdminGroupRow>(sqlstr);
            }
        }
    }

    public class AgentGroupRowCommand
    {
        [JsonProperty]
        public virtual string ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual int? GroupID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }

        public AgentGroupRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                long? id = this.ID.ToInt64();
                int? corpID = text.GroupRowID_CorpID(id);
                byte? groupID = text.GroupRowID_GroupID(id);
                AgentGroupRow row = sqlcmd.GetRowEx<AgentGroupRow>(RowErrorCode.NotFound, "select * from AgentGroup nolock where CorpID={0} and GroupID={1}", corpID, groupID);
                sqltool s = new sqltool();
                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
                s[" ", "GroupID", "", row.GroupID, ""] = this.GroupID;
                s["N", "Name", "   ", row.Name, "   "] = text.ValidAsString * this.Name;
                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["_CorpID"] = corpID;
                s.Values["_GroupID"] = groupID;
                s.Values["CorpID"] = this.CorpID;
                s.Values["GroupID"] = this.GroupID;
                string sqlstr = s.BuildEx("update AgentGroup set ", sqltool._FieldValue, " where CorpID={_CorpID} and GroupID={_GroupID} select * from AgentGroup nolock where CorpID={CorpID} and GroupID={GroupID}");
                //s.Values["ID"] = this.ID;
                //string sqlstr = s.BuildEx(s.Values.ContainsKey("ACNT") ?
                //    "update Corp set Corp.AgentGroupACNT={ACNT} from AgentGroup where Corp.ID=AgentGroup.CorpID and Corp.AgentGroupACNT=AgentGroup.ACNT and AgentGroup.ID={ID} " : "",
                //    "update AgentGroup set ", sqltool._FieldValue, " where ID={ID} select * from AgentGroup nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AgentGroupRow>(sqlstr);
            }
        }

        public AgentGroupRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "CorpID", "      "] = this.CorpID;
            s["* ", "GroupID", "     "] = this.GroupID;
            s["*N", "Name", "        "] = text.ValidAsString * this.Name;
            s["  ", "Locked", "      "] = this.Locked ?? BU.Locked.Active;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.Values["CorpID"] = (StringEx.sql_str)"ID";
            s.values["CorpID_"] = this.CorpID;
            string sqlstr = s.BuildEx(@"insert into AgentGroup (", sqltool._Fields, @")
select ", sqltool._Values, @" from Corp nolock where ID={CorpID_}
select * from AgentGroup nolock where CorpID={CorpID_} and GroupID={GroupID}");
            SqlCmd sqlcmd, _sqlcmd = args.GetValue<SqlCmd>(0);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
            {
                if (sqlcmd == _sqlcmd)
                    return sqlcmd.ToObject<AgentGroupRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<AgentGroupRow>(sqlstr);
            }
        }
    }

    public class MemberGroupRowCommand
    {
        [JsonProperty]
        public virtual string ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual int? GroupID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual decimal? BonusW { get; set; }
        [JsonProperty]
        public virtual decimal? BonusL { get; set; }

        public MemberGroupRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                long? id = this.ID.ToInt64();
                int? corpID = text.GroupRowID_CorpID(id);
                byte? groupID = text.GroupRowID_GroupID(id);
                MemberGroupRow row = sqlcmd.GetRowEx<MemberGroupRow>(RowErrorCode.NotFound, "select * from MemberGroup nolock where CorpID={0} and GroupID={1}", corpID, groupID);
                sqltool s = new sqltool();
                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
                s[" ", "GroupID", "", row.GroupID, ""] = this.GroupID;
                s["N", "Name", "   ", row.Name, "   "] = text.ValidAsString * this.Name;
                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
                s[" ", "BonusW", " ", row.BonusW, ""] = this.BonusW;
                s[" ", "BonusL", " ", row.BonusL, ""] = this.BonusL;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["_CorpID"] = corpID;
                s.Values["_GroupID"] = groupID;
                s.Values["CorpID"] = this.CorpID;
                s.Values["GroupID"] = this.GroupID;
                //s.Values["ID"] = this.ID;
                //string sqlstr = s.BuildEx(s.Values.ContainsKey("ACNT") ?
                //    "update Corp set Corp.MemberGroupACNT={ACNT} from MemberGroup where Corp.ID=MemberGroup.CorpID and Corp.MemberGroupACNT=MemberGroup.ACNT and MemberGroup.ID={ID} " : "",
                //    "update MemberGroup set ", sqltool._FieldValue, " where ID={ID} select * from MemberGroup nolock where ID={ID}");
                string sqlstr = s.BuildEx("update MemberGroup set ", sqltool._FieldValue, " where CorpID={_CorpID} and GroupID={_GroupID} select * from MemberGroup nolock where CorpID={CorpID} and GroupID={GroupID}");
                return sqlcmd.ExecuteEx<MemberGroupRow>(sqlstr);
            }
        }

        public MemberGroupRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "CorpID", "      "] = this.CorpID;
            s["* ", "GroupID", "     "] = this.GroupID;
            s["*N", "Name", "        "] = text.ValidAsString * this.Name;
            s["  ", "Locked", ""] = this.Locked ?? BU.Locked.Active;
            s["  ", "BonusW", ""] = this.BonusW ?? 0;
            s["  ", "BonusL", ""] = this.BonusL ?? 0;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.Values["CorpID"] = (StringEx.sql_str)"ID";
            s.values["CorpID_"] = this.CorpID;
            string sqlstr = s.BuildEx(@"insert into MemberGroup (", sqltool._Fields, @")
select ", sqltool._Values, @" from Corp nolock where ID={CorpID_}
select * from MemberGroup nolock where CorpID={CorpID_} and GroupID={GroupID}");
//            string sqlstr = s.BuildEx(@"declare @ID int exec alloc_UserID @ID output, @type='MemberGroup',@corpid={CorpID},@acnt={ACNT}
//insert into MemberGroup (ID,", sqltool._Fields, @")
//select @ID,", sqltool._Values, @" from Corp nolock where ID={CorpID}
//select * from MemberGroup nolock where ID=@ID");
            SqlCmd sqlcmd, _sqlcmd = args.GetValue<SqlCmd>(0);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
            {
                if (sqlcmd == _sqlcmd)
                    return sqlcmd.ToObject<MemberGroupRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<MemberGroupRow>(sqlstr);
            }
        }
    }
}