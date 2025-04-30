using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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
}
