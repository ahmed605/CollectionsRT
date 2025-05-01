using System;
using System.Runtime.InteropServices;

#if NET
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT.Marshallers
{
    public unsafe class VectorViewMarshaller<T> : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string pstrCookie) => new VectorViewMarshaller<T>();

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

            if (ManagedObj is VectorView<T> vectorView)
            {
                return (IntPtr)vectorView.GetPointer();
            }

            throw new ArgumentException($"Invalid type {ManagedObj.GetType()} for {nameof(VectorView<T>)}");
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
                return null;

            return new VectorView<T>((void*)pNativeData, false);
        }
    }

#if NET
    [CustomMarshaller(typeof(VectorView<>), MarshalMode.Default, typeof(SourceGenVectorViewMarshaller<>))]
    public static unsafe class SourceGenVectorViewMarshaller<T>
    {
        public static void* ConvertToUnmanaged(VectorView<T> managed)
        {
            if (managed is null)
                return null;

            return managed.GetPointer();
        }

        public static VectorView<T> ConvertToManaged(void* unmanaged)
        {
            if (unmanaged == null)
                return null;

            return new VectorView<T>(unmanaged, false);
        }

        public static void Free(void* unmanaged) { }
    }
#endif
}
