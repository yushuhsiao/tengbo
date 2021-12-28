using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace extAPI
{
    public abstract class _base<T> where T : _base<T>
    {
        static Dictionary<int, T> instances = new Dictionary<int, T>();

        //[DebuggerStepThrough]
        public static T GetInstance(int? corpID)
        {
            if (!corpID.HasValue)
                return null;
            lock (instances)
                if (instances.ContainsKey(corpID.Value))
                    return instances[corpID.Value];
                else
                    return instances[corpID.Value] = (T)Activator.CreateInstance(typeof(T), corpID.Value);
        }
        public readonly int CorpID;
        [DebuggerStepThrough]
        protected _base(int corpID) { this.CorpID = corpID; }
    }
}