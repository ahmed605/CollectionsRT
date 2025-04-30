using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
    [Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
    internal unsafe struct IIterable<T> where T : unmanaged
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
        internal int First(IIterator<T>** first)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, IUnknown**, int>)_vtbl[6])((IUnknown*)Unsafe.AsPointer(ref this), (IUnknown**)first);
        }
    }
}
