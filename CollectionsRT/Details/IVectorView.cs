using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
    [StructLayout(LayoutKind.Sequential)]
    [Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
    internal unsafe struct IVectorView<T>
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
        internal int IndexOf(T value, uint* index, byte* found)
        {
            // FIXME: .NET FX does not support generic types in function pointers
            return ((delegate* unmanaged[Stdcall]<IUnknown*, T, uint*, byte*, int>)_vtbl[8])((IUnknown*)Unsafe.AsPointer(ref this), value, index, found);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetMany(uint startIndex, uint capacity, T* value, uint* actual)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, uint, void*, uint*, int>)_vtbl[9])((IUnknown*)Unsafe.AsPointer(ref this), startIndex, capacity, value, actual);
        }
    }
}
