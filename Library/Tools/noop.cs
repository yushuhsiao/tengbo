using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Tools
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class _null
    {
        public static void noop() { }
        public static void noop<T>(T t) { }
        public static void noop<T1, T2>(T1 t1, T2 t2) { }
        public static readonly string[] strings = new string[0];
        public static readonly object[] objects = new object[0];
    }

    public static class _null<T> where T:new()
    {
        public static readonly T value = new T();
        public static readonly T[] array = new T[0]; 
    }

    public static class util
    {
        public static bool IsWeb
        {
            get { return !string.IsNullOrEmpty(HttpRuntime.AppDomainAppId); }
        }
    }
}