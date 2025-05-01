using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
#if !NET
    file static class VectorDelegates
    {
        internal static readonly Dictionary<Type, Type> IndexOf_Delegates = [];
        internal static readonly Dictionary<Type, Type> SetOrInsertAt_Delegates = [];
        internal static readonly Dictionary<Type, Type> Append_Delegates = [];
    }
#endif

    [StructLayout(LayoutKind.Sequential)]
    [Guid("913337E9-11A1-4345-A3A2-4E7F956E222D")]
    internal unsafe struct IVector<T>
    {
        private IInspectable _base;

        internal readonly void** _vtbl
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _base._vtbl;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int QueryInterface(Guid* riid, void** ppvObject)
        {
            return _base.QueryInterface(riid, ppvObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint AddRef()
        {
            return _base.AddRef();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint Release()
        {
            return _base.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetIids(uint* iidCount, Guid** iids)
        {
            return _base.GetIids(iidCount, iids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetRuntimeClassName(void** className)
        {
            return _base.GetRuntimeClassName(className);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetTrustLevel(int* trustLevel)
        {
            return _base.GetTrustLevel(trustLevel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetAt(uint index, T* item)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, void*, int>)_vtbl[6])((IUnknown*)Unsafe.AsPointer(ref this), index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int get_Size(uint* size)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint*, int>)_vtbl[7])((IUnknown*)Unsafe.AsPointer(ref this), size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetView(IVectorView<T>** view)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, IUnknown**, int>)_vtbl[8])((IUnknown*)Unsafe.AsPointer(ref this), (IUnknown**)view);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int IndexOf(T value, uint* index, byte* found)
        {
            // FIXME: .NET FX does not support generic types in function pointers
            return ((delegate* unmanaged[Stdcall]<IUnknown*, T, uint*, byte*, int>)_vtbl[9])((IUnknown*)Unsafe.AsPointer(ref this), value, index, found);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int IndexOfValue(T value, uint* index, byte* found)
        {
#if NET
            return IndexOf(value, index, found);
#else

            Type fn_delegate = null;
            if(!VectorDelegates.IndexOf_Delegates.TryGetValue(typeof(T), out fn_delegate))
            {
                fn_delegate = DelegateHelpers.GetDelegateType(typeof(nint), typeof(T), typeof(nint), typeof(nint), typeof(int));
                VectorDelegates.IndexOf_Delegates.Add(typeof(T), fn_delegate);
            }

            return (int)Marshal.GetDelegateForFunctionPointer((IntPtr)_vtbl[9], fn_delegate).DynamicInvoke([(nint)Unsafe.AsPointer(ref this), value, (nint)index, (nint)found]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int IndexOfReference(void* value, uint* index, byte* found)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, void*, uint*, byte*, int>)_vtbl[9])((IUnknown*)Unsafe.AsPointer(ref this), value, index, found);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int SetAt(uint index, T item)
        {
            // FIXME: .NET FX does not support generic types in function pointers
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, T, int>)_vtbl[10])((IUnknown*)Unsafe.AsPointer(ref this), index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int SetValueAt(uint index, T item)
        {
#if NET
            return SetAt(index, item);
#else
            Type fn_delegate = null;
            if (!VectorDelegates.SetOrInsertAt_Delegates.TryGetValue(typeof(T), out fn_delegate))
            {
                fn_delegate = DelegateHelpers.GetDelegateType(typeof(nint), typeof(uint), typeof(T), typeof(int));
                VectorDelegates.SetOrInsertAt_Delegates.Add(typeof(T), fn_delegate);
            }

            return (int)Marshal.GetDelegateForFunctionPointer((IntPtr)_vtbl[10], fn_delegate).DynamicInvoke([(nint)Unsafe.AsPointer(ref this), index, item]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int SetReferenceAt(uint index, void* item)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, void*, int>)_vtbl[10])((IUnknown*)Unsafe.AsPointer(ref this), index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int InsertAt(uint index, T item)
        {
            // FIXME: .NET FX does not support generic types in function pointers
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, T, int>)_vtbl[11])((IUnknown*)Unsafe.AsPointer(ref this), index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int InsertValueAt(uint index, T item)
        {
#if NET
            return InsertAt(index, item);
#else
            Type fn_delegate = null;
            if (!VectorDelegates.SetOrInsertAt_Delegates.TryGetValue(typeof(T), out fn_delegate))
            {
                fn_delegate = DelegateHelpers.GetDelegateType(typeof(nint), typeof(uint), typeof(T), typeof(int));
                VectorDelegates.SetOrInsertAt_Delegates.Add(typeof(T), fn_delegate);
            }

            return (int)Marshal.GetDelegateForFunctionPointer((IntPtr)_vtbl[11], fn_delegate).DynamicInvoke([(nint)Unsafe.AsPointer(ref this), index, item]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int InsertReferenceAt(uint index, void* item)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, void*, int>)_vtbl[11])((IUnknown*)Unsafe.AsPointer(ref this), index, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int RemoveAt(uint index)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, int>)_vtbl[12])((IUnknown*)Unsafe.AsPointer(ref this), index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Append(T item)
        {
            // FIXME: .NET FX does not support generic types in function pointers
            return ((delegate* unmanaged[Stdcall]<IUnknown*, T, int>)_vtbl[13])((IUnknown*)Unsafe.AsPointer(ref this), item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int AppendValue(T item)
        {
#if NET
            return Append(item);
#else
            Type fn_delegate = null;
            if (!VectorDelegates.Append_Delegates.TryGetValue(typeof(T), out fn_delegate))
            {
                fn_delegate = DelegateHelpers.GetDelegateType(typeof(nint), typeof(T), typeof(int));
                VectorDelegates.Append_Delegates.Add(typeof(T), fn_delegate);
            }

            return (int)Marshal.GetDelegateForFunctionPointer((IntPtr)_vtbl[13], fn_delegate).DynamicInvoke([(nint)Unsafe.AsPointer(ref this), item]);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int AppendReference(void* item)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, void*, int>)_vtbl[13])((IUnknown*)Unsafe.AsPointer(ref this), item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int RemoveAtEnd()
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, int>)_vtbl[14])((IUnknown*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Clear()
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, int>)_vtbl[15])((IUnknown*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetMany(uint startIndex, uint capacity, T* items, uint* actual)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, uint, void*, uint*, int>)_vtbl[16])((IUnknown*)Unsafe.AsPointer(ref this), startIndex, capacity, items, actual);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int ReplaceAll(uint count, T* items)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, void*, int>)_vtbl[17])((IUnknown*)Unsafe.AsPointer(ref this), count, items);
        }
    }
}
