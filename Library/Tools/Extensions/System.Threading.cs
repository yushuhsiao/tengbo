using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace System.Threading
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    public static partial class Extensions
    {
        [_DebuggerStepThrough]
        public static MethodBase GetCallingMethod(this Thread thread, int stackLevel)
        {
            if (stackLevel >= 0)
            {
                MethodBase m1 = MethodBase.GetCurrentMethod();
                StackTrace s = new StackTrace(thread, false);
                for (int i = 0; i < s.FrameCount; i++)
                {
                    MethodBase m2 = s.GetFrame(i).GetMethod();
                    if (m1 != m2) continue;
                    for (int n1 = 0, n2 = i + 1; (n1 <= stackLevel) && (n2 < s.FrameCount); n1++, n2++)
                        m2 = s.GetFrame(n2).GetMethod();
                    return m2;
                }
            }
            return null;
        }
    }
}