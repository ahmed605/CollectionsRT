using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace CollectionsRT.Details
{
    [Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal unsafe interface IIterable
    {
        internal object CreateInstance(void* iterable, bool addRef = false);
        internal object CreateInstance(void* iterable, Guid iid);
    }

    [Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal unsafe interface IIterator
    {
        internal object CreateInstance(void* iterator, bool addRef = false, IInspectable* iterable = null);
        internal object CreateInstance(void* iterator, Guid iid);
    }

    [Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal unsafe interface IVectorView
    {
        internal object CreateInstance(void* view, bool addRef = false);
        internal object CreateInstance(void* view, Guid iterableIID, bool addRef = false);
        internal object CreateInstance(void* view, Guid iid, Guid iterableIID);
        internal object CreateInstance(void* view, Guid iid);
    }

    [Guid("913337E9-11A1-4345-A3A2-4E7F956E222D")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal unsafe interface IVector
    {
        internal object CreateInstance(void* vector, bool addRef = false);
        internal object CreateInstance(void* vector, Guid iterableIID, bool addRef = false);
        internal object CreateInstance(void* vector, Guid iid, Guid iterableIID);
        internal object CreateInstance(void* vector, Guid iid);
    }

    internal unsafe interface ICollectionType
    {
        void* GetPointer();
    }
}
