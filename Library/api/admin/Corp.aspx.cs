using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Tools;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CorpRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport, JsonProperty]
        public CurrencyCode? Currency;
        [DbImport, JsonProperty]
        public string AdminACNT;
        [DbImport, JsonProperty]
        public string AgentACNT;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        [SqlSetting]
        public string prefix
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.ID.Value); }
        }

        public class Cache : WebTools.ListCache<Cache, CorpRow>
        {
            //Dictionary<int, string> _names;
            //public Dictionary<int, string> names
            //{
            //    get { return Interlocked.CompareExchange(ref this._names, null, null); }
            //    set { Interlocked.Exchange(ref this._names, value); }
            //}

            [SqlSetting("Cache", "Corps"), DefaultValue(30000.0)]
            public override double LifeTime
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
                set { }
            }

            //protected override bool CheckUpdate(string key, params object[] args)
            //{
            //    return this.names == null;
            //}

            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
                {
                    //Dictionary<int, string> names = new Dictionary<int, string>();
                    List<CorpRow> rows = new List<CorpRow>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Corp nolock"))
                    {
                        CorpRow row = r.ToObject<CorpRow>();
                        //names[row.ID.Value] = row.ACNT;
                        rows.Add(row);
                    }
                    base.Rows = rows;
                    //this.names = names;
                }
            }

            public CorpRow GetCorp(int? corpID)
            {
                foreach (CorpRow row in this.Rows)
                    if (row.ID == corpID)
                        return row;
                return _null<CorpRow>.value;
            }

            public CorpRow GetCorp(string acnt)
            {
                foreach (CorpRow row in this.Rows)
                    if (string.Compare(row.ACNT, acnt, true) == 0)
                        return row;
                return _null<CorpRow>.value;
            }
        }
    }
}

//namespace web
//{
//    public class CorpList : WebTools.ListCache<CorpList, CorpRow>
//    {
//        Dictionary<int, string> _names;
//        public Dictionary<int, string> names
//        {
//            get { return Interlocked.CompareExchange(ref this._names, null, null); }
//            set { Interlocked.Exchange(ref this._names, value); }
//        }

//        [SqlSetting("Cache", "Corps"), DefaultValue(30000.0)]
//        public override double LifeTime
//        {
//            get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
//            set { }
//        }

//        protected override bool CheckUpdate(string key, params object[] args)
//        {
//            return this.names == null;
//        }

//        public override void Update(string key, params object[] args)
//        {
//            SqlCmd sqlcmd;
//            using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, args.GetValue<SqlCmd>(0)))
//            {
//                Dictionary<int, string> names = new Dictionary<int, string>();
//                List<CorpRow> rows = new List<CorpRow>();
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Corp nolock"))
//                {
//                    CorpRow row = r.ToObject<CorpRow>();
//                    names[row.ID.Value] = row.ACNT;
//                    rows.Add(row);
//                }
//                base.Rows = rows;
//                this.names = names;
//            }
//        }

//        public CorpRow GetCorp(int? corpID)
//        {
//            foreach (CorpRow row in this.Rows)
//                if (row.ID == corpID)
//                    return row;
//            return _null<CorpRow>.value;
//        }

//        public CorpRow GetCorp(string acnt)
//        {
//            foreach (CorpRow row in this.Rows)
//                if (string.Compare(row.ACNT, acnt, true) == 0)
//                    return row;
//            return _null<CorpRow>.value;
//        }

//        //public string GetCorpACNT(int? corpID)
//        //{
//        //    if (corpID.HasValue)
//        //    {
//        //        Dictionary<int, string> acnts = this.acnts;
//        //        string acnt;
//        //        if (acnts.TryGetValue(corpID.Value, out acnt))
//        //            return acnt;
//        //    }
//        //    return null;
//        //}

//        //public int? GetCorpID(string corpName)
//        //{
//        //    if (corpName != null)
//        //        foreach (KeyValuePair<int, string> p in this.acnts)
//        //            if (string.Compare(corpName, p.Value, true) == 0)
//        //                return p.Key;
//        //    return null;
//        //}

//        //public bool GetCorpID(string corpName, out int corpID)
//        //{
//        //    int? id = this.GetCorpID(corpName);
//        //    corpID = id ?? 0;
//        //    return id.HasValue;
//        //}
//    }
//}

