using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if !NET
using System.Runtime.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;
#else
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT.Details
{
    internal static unsafe class MarshallingHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object GetUninitializedObject(Type type)
        {
#if NET
            return RuntimeHelpers.GetUninitializedObject(type);
#else
            return FormatterServices.GetUninitializedObject(type);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalVector<T>(void* ptr)
        {
            var value = (T)GetUninitializedObject(typeof(T));
            return (T)((IVector)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalVectorView<T>(void* ptr)
        {
            var value = (T)GetUninitializedObject(typeof(T));
            return (T)((IVectorView)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalIterable<T>(void* ptr)
        {
            var value = (T)GetUninitializedObject(typeof(T));
            return (T)((IIterable)value).CreateInstance(ptr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T MarshalIterator<T>(void* ptr)
        {
            var value = (T)GetUninitializedObject(typeof(T));
            return (T)((IIterator)value).CreateInstance(ptr, false);
        }

        internal static string MarshalString(void* ptr)
        {
#if !NET
            var str = WindowsRuntimeMarshal.PtrToStringHString((IntPtr)ptr);
            WindowsRuntimeMarshal.FreeHString((IntPtr)ptr);
            return str;
#else
            var str = WinRT.MarshalString.FromAbi((nint)ptr);
            WinRT.MarshalString.DisposeAbi((nint)ptr);
            return str;
#endif
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

#if !NET
            return (T)Marshal.GetObjectForIUnknown((IntPtr)ptr);
#else
            try
            {
                return WinRT.MarshalInspectable<T>.FromAbi((nint)ptr);
            }
            catch
            {
                try
                {
                    return ComInterfaceMarshaller<T>.ConvertToManaged(ptr);
                }
                catch
                {
                    return (T)Marshal.GetObjectForIUnknown((IntPtr)ptr);
                }
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void* MarshalValue<T>(T value)
        {
            Type t = typeof(T);

            if (t == typeof(string))
            {
#if !NET
                return (void*)WindowsRuntimeMarshal.StringToHString((string)(object)value);
#else
                return (void*)WinRT.MarshalString.FromManaged((string)(object)value);
#endif
            }
            else if (typeof(ICollectionType).IsAssignableFrom(t))
            {
                return ((ICollectionType)value).GetPointer();
            }

#if !NET
            return (void*)Marshal.GetIUnknownForObject(value);
#else
            try
            {
                return (void*)((WinRT.IWinRTObject)value).NativeObject.ThisPtr;
            }
            catch
            {
                try
                {
                    return ComInterfaceMarshaller<T>.ConvertToUnmanaged((T)value);
                }
                catch
                {
                    return (void*)Marshal.GetIUnknownForObject(value);
                }
            }
#endif
        }
    }
}
