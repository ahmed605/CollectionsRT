using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT.Details
{
    [StructLayout(LayoutKind.Sequential)]
    [Guid("00000000-0000-0000-C000-000000000046")]
    internal unsafe struct IUnknown
    {
        internal readonly void** _vtbl;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int QueryInterface(Guid* riid, void** ppvObject)
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, Guid*, void**, int>)_vtbl[0])((IUnknown*)Unsafe.AsPointer(ref this), riid, ppvObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint AddRef()
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint>)_vtbl[1])((IUnknown*)Unsafe.AsPointer(ref this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint Release()
        {
            return ((delegate* unmanaged[Stdcall]<IUnknown*, uint>)_vtbl[2])((IUnknown*)Unsafe.AsPointer(ref this));
        }
    }
}
