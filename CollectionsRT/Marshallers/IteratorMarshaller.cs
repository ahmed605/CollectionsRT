using System;
using System.Runtime.InteropServices;

#if NET
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT.Marshallers
{
    public unsafe class IteratorMarshaller<T> : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string pstrCookie) => new IteratorMarshaller<T>();

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

            if (ManagedObj is Iterator<T> iterator)
            {
                return (IntPtr)iterator.GetPointer();
            }

            throw new ArgumentException($"Invalid type {ManagedObj.GetType()} for {nameof(Iterator<T>)}");
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;

            return new Iterator<T>((void*)pNativeData, false);
        }
    }

#if NET
    [CustomMarshaller(typeof(Iterator<>), MarshalMode.Default, typeof(SourceGenIteratorMarshaller<>))]
    public static unsafe class SourceGenIteratorMarshaller<T>
    {
        public static void* ConvertToUnmanaged(Iterator<T> managed)
        {
            if (managed is null)
                return null;

            return managed.GetPointer();
        }

        public static Iterator<T> ConvertToManaged(void* unmanaged)
        {
            if (unmanaged == null)
                return null;

            return new Iterator<T>(unmanaged, false);
        }

        public static void Free(void* unmanaged) { }
    }
#endif
}
