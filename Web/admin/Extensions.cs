using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace web
{
    public delegate object GetLocalResourceObjectHandler(string resourceKey);
    public delegate object GetGlobalResourceObjectHandler(string className, string resourceKey);

    public static partial class Shared
    {
        [DebuggerStepThrough]
        public static T GetValue<T>(this object[] args, int index)
        {
            if (args != null)
                if (args.Length > index)
                    if (args[index] is T)
                        return (T)args[index];
            return default(T);
        }

        public static class ConfigSection
        {
        }
    }
}