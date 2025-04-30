using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;

namespace CollectionsRT.Details
{
    internal static unsafe class MarshallingHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalVector<T>(void* ptr)
        {
            var value = (T)FormatterServices.GetUninitializedObject(typeof(T));
            return (T)((IVector)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalVectorView<T>(void* ptr)
        {
            var value = (T)FormatterServices.GetUninitializedObject(typeof(T));
            return (T)((IVectorView)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalIterable<T>(void* ptr)
        {
            var value = (T)FormatterServices.GetUninitializedObject(typeof(T));
            return (T)((IIterable)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalIterator<T>(void* ptr)
        {
            var value = (T)FormatterServices.GetUninitializedObject(typeof(T));
            return (T)((IIterator)value).CreateInstance(ptr, false);
        }

        internal static string MarshalString(void* ptr)
        {
            var str = WindowsRuntimeMarshal.PtrToStringHString((IntPtr)ptr);
            WindowsRuntimeMarshal.FreeHString((IntPtr)ptr);
            return str;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalValue<T>(void* ptr)
        {
            Type t = typeof(T);

            if (t == typeof(string))
            {
                return (T)(object)MarshalString(ptr);
            }
            else if (typeof(IVector).IsAssignableFrom(t))
            {
                return MarshalVector<T>(ptr);
            }
            else if (typeof(IVectorView).IsAssignableFrom(t))
            {
                return MarshalVectorView<T>(ptr);
            }
            else if (typeof(IIterable).IsAssignableFrom(t))
            {
                return MarshalIterable<T>(ptr);
            }
            else if (typeof(IIterator).IsAssignableFrom(t))
            {
                return MarshalIterator<T>(ptr);
            }

            return (T)Marshal.GetObjectForIUnknown((IntPtr)ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void* MarshalValue<T>(T value)
        {
            Type t = typeof(T);

            if (t == typeof(string))
            {
                return (void*)WindowsRuntimeMarshal.StringToHString((string)(object)value);
            }
            else if (typeof(ICollectionType).IsAssignableFrom(t))
            {
                return ((ICollectionType)value).GetPointer();
            }

            return (void*)Marshal.GetIUnknownForObject(value);
        }
    }
}
