using CollectionsRT.Details;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CollectionsRT
{
    public unsafe class Iterator<T> : IIterator, IEnumerator<T>, ICollectionType
    {
        private IIterator<nint>* _iterator;
        private IIterable<nint>* _iterable;
        private bool _movedOnce = false;

        internal Iterator(void* iterator, bool addRef = false, IInspectable* iterable = null)
        {
            if (iterator is null)
                throw new ArgumentNullException($"{nameof(iterator)} cannot be null.");

            _iterator = (IIterator<nint>*)iterator;
            if (addRef) _iterator->AddRef();

            if (iterable is not null)
            {
                _iterable = (IIterable<nint>*)iterable;
                _iterable->AddRef();
            }
        }

        public Iterator(void* iterator, bool addRef = false) : this(iterator, addRef, null) { }

        public Iterator(void* iterator, Guid iid)
        {
            if (iterator is null)
                throw new ArgumentNullException($"{nameof(iterator)} cannot be null.");

            IIterator<nint>* ptr = default;
            Marshal.ThrowExceptionForHR(((IUnknown*)iterator)->QueryInterface(&iid, (void**)&ptr));
            _iterator = ptr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object IIterator.CreateInstance(void* iterator, bool addRef, IInspectable* iterable) => new Iterator<T>(iterable, addRef, iterable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        object IIterator.CreateInstance(void* iterable, Guid iid) => new Iterator<T>(iterable, iid);

        public T Current => GetCurrent();

        object IEnumerator.Current => GetCurrent();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T GetCurrent()
        {
            if (typeof(T).IsUnManaged())
            {
                T value = default;
                Marshal.ThrowExceptionForHR(((IIterator<T>*)_iterator)->get_Current(&value));
                return value;
            }
            else
            {
                void* value = default;
                Marshal.ThrowExceptionForHR((_iterator)->get_Current((nint*)&value));
                return MarshallingHelpers.MarshalValue<T>(value);
            }
        }

        public void Dispose()
        {
            if (_iterator != null)
            {
                _iterator->Release();
                _iterator = null;
            }

            if (_iterable != null)
            {
                _iterable->Release();
                _iterable = null;
            }
        }

        public bool MoveNext()
        {
            if (!_movedOnce)
            {
                _movedOnce = true;
                return true;
            }

            byte hasCurrent = 0;
            Marshal.ThrowExceptionForHR(_iterator->MoveNext(&hasCurrent));
            return hasCurrent != 0;
        }

        public void Reset()
        {
            if (_iterable is null)
                throw new NotSupportedException($"{nameof(Reset)} is not supported for this iterator.");

            IIterator<nint>* iterator = default;
            Marshal.ThrowExceptionForHR(_iterable->First(&iterator));

            if (_iterator is not null) _iterator->Release();
            _iterator = iterator;
            _movedOnce = false;
        }

        public void* GetPointer()
        {
            return _iterator;
        }
    }
}
