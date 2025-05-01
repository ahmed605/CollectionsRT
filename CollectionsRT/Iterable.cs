using CollectionsRT.Details;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT
{
    public unsafe class Iterable<T> : IIterable, System.Collections.Generic.IEnumerable<T>, ICollectionType
    {
        private IIterable<nint>* _iterable;

        public Iterable(void* iterable, bool addRef = false)
        {
            if (iterable is null)
                throw new ArgumentNullException($"{nameof(iterable)} cannot be null.");

            _iterable = (IIterable<nint>*)iterable;
            if (addRef) _iterable->AddRef();
        }

        public Iterable(void* iterable, Guid iid)
        {
            if (iterable is null)
                throw new ArgumentNullException($"{nameof(iterable)} cannot be null.");

            IIterable<nint>* ptr = default;
            Marshal.ThrowExceptionForHR(((IUnknown*)iterable)->QueryInterface(&iid, (void**)&ptr));
            _iterable = ptr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object IIterable.CreateInstance(void* iterable, bool addRef) => new Iterable<T>(iterable, addRef);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object IIterable.CreateInstance(void* iterable, Guid iid) => new Iterable<T>(iterable, iid);

        ~Iterable()
        {
            if (_iterable != null)
            {
                _iterable->Release();
                _iterable = null;
            }
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            IIterator<nint>* iterator = default;
            Marshal.ThrowExceptionForHR(_iterable->First(&iterator));
            return new Iterator<T>(iterator, false, (IInspectable*)_iterable);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            IIterator<nint>* iterator = default;
            Marshal.ThrowExceptionForHR(_iterable->First(&iterator));
            return new Iterator<T>(iterator, false, (IInspectable*)_iterable);
        }

        public void* GetPointer()
        {
            return _iterable;
        }
    }
}
