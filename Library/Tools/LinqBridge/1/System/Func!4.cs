﻿namespace System
{
    using System.Runtime.CompilerServices;

    public delegate TResult Func<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, T3 arg3);
}

