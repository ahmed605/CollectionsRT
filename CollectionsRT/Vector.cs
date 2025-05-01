using CollectionsRT.Details;
using CollectionsRT.Marshallers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if NET
using System.Runtime.InteropServices.Marshalling;
#endif

namespace CollectionsRT
{
#if NET
    [NativeMarshalling(typeof(SourceGenVectorMarshaller<>))]
#endif
    public unsafe class Vector<T> : Iterable<T>, IList<T>, IVector, ICollectionType
    {
        private IVector<nint>* _vector;

        public Vector(void* vector, Guid iterableIID, bool addRef = false) : base(vector, iterableIID)
        {
            if (vector is null)
                throw new ArgumentNullException($"{nameof(vector)} cannot be null.");

            _vector = (IVector<nint>*)vector;
            if (addRef) _vector->AddRef();
        }

        public Vector(void* vector, bool addRef = false) : this(vector, GuidHelpers.CreateGuidForGenericType<Iterable<T>>(), addRef) { }

        public Vector(void* vector, Guid iid, Guid iterableIID) : base(vector, iterableIID)
        {
            if (vector is null)
                throw new ArgumentNullException($"{nameof(vector)} cannot be null.");

            IVector<nint>* ptr = default;
            Marshal.ThrowExceptionForHR(((IUnknown*)vector)->QueryInterface(&iid, (void**)&ptr));
            _vector = ptr;
        }

        public Vector(void* vector, Guid iid) : this(vector, iid, GuidHelpers.CreateGuidForGenericType<Iterable<T>>()) { }

        ~Vector()
        {
            if (_vector != null)
            {
                _vector->Release();
                _vector = null;
            }
        }

        object IVector.CreateInstance(void* vector, Guid iterableIID, bool addRef) => new Vector<T>(vector, iterableIID, addRef);
        object IVector.CreateInstance(void* vector, bool addRef) => new Vector<T>(vector, addRef);
        object IVector.CreateInstance(void* vector, Guid iid, Guid iterableIID) => new Vector<T>(vector, iid, iterableIID);
        object IVector.CreateInstance(void* vector, Guid iid) => new Vector<T>(vector, iid);

        public T this[int index] 
        { 
            get
            {
                if (typeof(T).IsBlittable())
                {
                    T value = default;
                    Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->GetAt((uint)index, &value));
                    return value;
                }
                else
                {
                    void* value = default;
                    Marshal.ThrowExceptionForHR(_vector->GetAt((uint)index, (nint*)&value));
                    return MarshallingHelpers.MarshalValue<T>(value);
                }
            }

            set
            {
                if (typeof(T).IsBlittable())
                {
                    Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->SetValueAt((uint)index, value));
                }
                else
                {
                    Marshal.ThrowExceptionForHR(_vector->SetReferenceAt((uint)index, MarshallingHelpers.MarshalValue(value)));
                }
            }
        }

        public int Count
        {
            get
            {
                uint size = 0;
                Marshal.ThrowExceptionForHR(_vector->get_Size(&size));
                return (int)size;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (typeof(T).IsBlittable())
            {
                Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->AppendValue(item));
            }
            else
            {
                Marshal.ThrowExceptionForHR(_vector->AppendReference(MarshallingHelpers.MarshalValue(item)));
            }
        }

        public void Clear()
        {
            Marshal.ThrowExceptionForHR(_vector->Clear());
        }

        public bool Contains(T item)
        {
            if (typeof(T).IsBlittable())
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->IndexOfValue(item, &index, &found));
                return found != 0;
            }
            else
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(_vector->IndexOfReference(MarshallingHelpers.MarshalValue(item), &index, &found));
                return found != 0;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException($"{nameof(array)} cannot be null.");

            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException($"{nameof(arrayIndex)} is out of range.");

            if (typeof(T).IsBlittable())
            {
                fixed (T* pArray = array)
                {
                    uint count = 0;
                    uint capacity = (uint)Math.Min(array.Length - arrayIndex, Count);
                    Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->GetMany(0, capacity, &pArray[arrayIndex], &count));
                }
            }
            else
            {
                var count = Count;
                for (int i = 0; i < count; i++)
                {
                    array[arrayIndex + i] = this[i];
                }
            }
        }

        public int IndexOf(T item)
        {
            if (typeof(T).IsBlittable())
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->IndexOfValue(item, &index, &found));
                return found != 0 ? (int)index : -1;
            }
            else
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(_vector->IndexOfReference(MarshallingHelpers.MarshalValue(item), &index, &found));
                return found != 0 ? (int)index : -1;
            }
        }

        public void Insert(int index, T item)
        {
            if (typeof(T).IsBlittable())
            {
                Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->InsertValueAt((uint)index, item));
            }
            else
            {
                Marshal.ThrowExceptionForHR(_vector->InsertReferenceAt((uint)index, MarshallingHelpers.MarshalValue(item)));
            }
        }

        public bool Remove(T item)
        {
            if (typeof(T).IsBlittable())
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->IndexOfValue(item, &index, &found));
                if (found != 0)
                {
                    Marshal.ThrowExceptionForHR(((IVector<T>*)_vector)->RemoveAt(index));
                    return true;
                }
            }
            else
            {
                byte found = 0;
                uint index = 0;
                Marshal.ThrowExceptionForHR(_vector->IndexOfReference(MarshallingHelpers.MarshalValue(item), &index, &found));
                if (found != 0)
                {
                    Marshal.ThrowExceptionForHR(_vector->RemoveAt(index));
                    return true;
                }
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException($"{nameof(index)} is out of range.");

            Marshal.ThrowExceptionForHR(_vector->RemoveAt((uint)index));
        }

        void* ICollectionType.GetPointer()
        {
            return _vector;
        }
    }
}
