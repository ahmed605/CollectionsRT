using CollectionsRT.Details;
using CollectionsRT.Marshallers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CollectionsRT
{
    public unsafe class VectorView<T> : Iterable<T>, IReadOnlyList<T>, IVectorView, ICollectionType
    {
        private IVectorView<nint>* _view;

        public VectorView(void* view, Guid iterableIID, bool addRef = true) : base(view, iterableIID)
        {
            if (view is null)
                throw new ArgumentNullException($"{nameof(view)} cannot be null.");

            _view = (IVectorView<nint>*)view;
            if (addRef) _view->AddRef();
        }

        public VectorView(void* view, bool addRef = true) : this(view, GuidHelpers.CreateGuidForGenericType<Iterable<T>>(), addRef) { }

        public VectorView(void* view, Guid iid, Guid iterableIID) : base(view, iterableIID)
        {
            if (view is null)
                throw new ArgumentNullException($"{nameof(view)} cannot be null.");

            IVectorView<nint>* ptr = default;
            Marshal.ThrowExceptionForHR(((IUnknown*)view)->QueryInterface(&iid, (void**)&ptr));
            _view = ptr;
        }

        public VectorView(void* view, Guid iid) : this(view, iid, GuidHelpers.CreateGuidForGenericType<Iterable<T>>()) { }

        ~VectorView()
        {
            if (_view != null)
            {
                _view->Release();
                _view = null;
            }
        }

        object IVectorView.CreateInstance(void* view, Guid iterableIID, bool addRef) => new VectorView<T>(view, iterableIID, addRef);
        object IVectorView.CreateInstance(void* view, bool addRef) => new VectorView<T>(view, addRef);
        object IVectorView.CreateInstance(void* view, Guid iid, Guid iterableIID) => new VectorView<T>(view, iid, iterableIID);
        object IVectorView.CreateInstance(void* view, Guid iid) => new VectorView<T>(view, iid);

        public T this[int index]
        {
            get
            {
                if (typeof(T).IsUnManaged())
                {
                    T value = default;
                    Marshal.ThrowExceptionForHR(((IVectorView<T>*)_view)->GetAt((uint)index, &value));
                    return value;
                }
                else
                {
                    void* value = default;
                    Marshal.ThrowExceptionForHR((_view)->GetAt((uint)index, (nint*)&value));
                    return MarshallingHelpers.MarshalValue<T>(value);
                }
            }
        }

        public int Count
        {
            get
            {
                uint count = 0;
                Marshal.ThrowExceptionForHR(_view->get_Size(&count));
                return (int)count;
            }
        }

        void* ICollectionType.GetPointer()
        {
            return _view;
        }
    }
}
