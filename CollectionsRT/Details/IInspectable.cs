using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
    [StructLayout(LayoutKind.Sequential)]
    [Guid("AF86E2E0-B12D-4C6A-9C5A-D7AA65101E90")]
    internal unsafe struct IInspectable
    {
        private IUnknown _base;

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
            return ((delegate* unmanaged[Stdcall]<IInspectable*, uint*, Guid**, int>)_vtbl[3])((IInspectable*)Unsafe.AsPointer(ref this), iidCount, iids);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetRuntimeClassName(void** className)
        {
            return ((delegate* unmanaged[Stdcall]<IInspectable*, void**, int>)_vtbl[4])((IInspectable*)Unsafe.AsPointer(ref this), className);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetTrustLevel(int* trustLevel)
        {
            return ((delegate* unmanaged[Stdcall]<IInspectable*, int*, int>)_vtbl[5])((IInspectable*)Unsafe.AsPointer(ref this), trustLevel);
        }
    }
}
