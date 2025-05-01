using System;
using System.Runtime.InteropServices;

#if NET
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT.Marshallers
{
    public unsafe class IterableMarshaller<T> : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string pstrCookie) => new IterableMarshaller<T>();

        public void CleanUpManagedData(object ManagedObj) { }
        public void CleanUpNativeData(IntPtr pNativeData) { }

        public int GetNativeDataSize()
        {
            return sizeof(void*);
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj is null)
                return IntPtr.Zero;

            if (ManagedObj is Iterable<T> iterable)
            {
                return (IntPtr)iterable.GetPointer();
            }

            throw new ArgumentException($"Invalid type {ManagedObj.GetType()} for {nameof(Iterable<T>)}");
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;

            return new Iterable<T>((void*)pNativeData, false);
        }
    }

#if NET
    [CustomMarshaller(typeof(Iterable<>), MarshalMode.Default, typeof(SourceGenIterableMarshaller<>))]
    public static unsafe class SourceGenIterableMarshaller<T>
    {
        public static void* ConvertToUnmanaged(Iterable<T> managed)
        {
            if (managed is null)
                return null;

            return managed.GetPointer();
        }

        public static Iterable<T> ConvertToManaged(void* unmanaged)
        {
            if (unmanaged == null)
                return null;

            return new Iterable<T>(unmanaged, false);
        }

        public static void Free(void* unmanaged) { }
    }
#endif
}
