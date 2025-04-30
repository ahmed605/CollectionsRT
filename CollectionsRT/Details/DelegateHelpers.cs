using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CollectionsRT.Details
{
    internal static class DelegateHelpers
    {
        private static readonly Func<Type[], Type> MakeNewCustomDelegate = (Func<Type[], Type>)Delegate.CreateDelegate(
            typeof(Func<Type[], Type>),
            typeof(Expression).Assembly.GetType("System.Linq.Expressions.Compiler.DelegateHelpers").GetMethod(
            "MakeNewCustomDelegate",
            BindingFlags.NonPublic | BindingFlags.Static));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Type GetDelegateType(params Type[] parameters)
        {
            return MakeNewCustomDelegate(parameters);
        }
    }
}
