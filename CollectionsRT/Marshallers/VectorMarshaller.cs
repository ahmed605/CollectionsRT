using System;
using System.Runtime.InteropServices;

#if NET
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT.Marshallers
{
    public unsafe class VectorMarshaller<T> : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string pstrCookie) => new VectorMarshaller<T>();

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

            if (ManagedObj is Vector<T> vector)
            {
                return (IntPtr)vector.GetPointer();
            }

            throw new ArgumentException($"Invalid type {ManagedObj.GetType()} for {nameof(Vector<T>)}");
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;

            return new Vector<T>((void*)pNativeData, false);
        }
    }

#if NET
    [CustomMarshaller(typeof(Vector<>), MarshalMode.Default, typeof(SourceGenVectorMarshaller<>))]
    public static unsafe class SourceGenVectorMarshaller<T>
    {
        public static void* ConvertToUnmanaged(Vector<T> managed)
        {
            if (managed is null)
                return null;

            return managed.GetPointer();
        }

        public static Vector<T> ConvertToManaged(void* unmanaged)
        {
            if (unmanaged == null)
                return null;

            return new Vector<T>(unmanaged, false);
        }

        public static void Free(void* unmanaged) { }
    }
#endif
}
