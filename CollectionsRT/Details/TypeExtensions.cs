using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CollectionsRT.Details
{
    internal static class TypeExtensions
    {
        private static Dictionary<Type, bool> cachedTypes = [];

        public static bool IsBlittable
        (
#if NET
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
#endif
            this Type t
        )
        {
            var result = false;

            if (cachedTypes.TryGetValue(t, out result))
                return result;

            else if (t.IsPrimitive || t.IsPointer || t.IsEnum)
            {
                result = true;
            }
            else if (t.IsGenericType || !t.IsValueType)
            {
                result = false;
            }
            else
            {
                result = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic).All(x => x.FieldType.IsBlittable());
            }

            cachedTypes.Add(t, result);
            return result;
        }
    }
}
