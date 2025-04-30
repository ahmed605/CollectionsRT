using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
    [Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
    internal unsafe struct IIterator<T>
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
        internal int get_Current(T* current)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, void*, int>)_vtbl[6])((IUnknown*)Unsafe.AsPointer(ref this), current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int get_HasCurrent(byte* hasCurrent)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, byte*, int>)_vtbl[7])((IUnknown*)Unsafe.AsPointer(ref this), hasCurrent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int MoveNext(byte* hasCurrent)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, byte*, int>)_vtbl[8])((IUnknown*)Unsafe.AsPointer(ref this), hasCurrent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetMany(uint capacity, T* value, uint* actual)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint, void*, uint*, int>)_vtbl[9])((IUnknown*)Unsafe.AsPointer(ref this), capacity, value, actual);
        }
    }
}
