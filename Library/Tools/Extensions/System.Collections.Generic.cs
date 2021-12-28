using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Collections.Generic
{
    public class ObjectDictionary : Dictionary<string, object>
    {
        public T GetValue<T>(string key)
        {
            object result;
            if (base.TryGetValue(key, out result))
                if (result is T)
                    return (T)result;
            return default(T);
        }
    }
    //    [_DebuggerStepThrough]
    //    public static class Extensions
    //    {
    //        public static void AddNoDuplicate<T>(this List<T> list, T item)
    //        {
    //            if (list != null)
    //                if (!list.Contains(item))
    //                    list.Add(item);
    //        }

    //        public static void RemoveAt<T>(this List<T> list, T item)
    //        {
    //            if (list != null)
    //                while (list.Remove(item)) { }
    //        }
    //    }
}