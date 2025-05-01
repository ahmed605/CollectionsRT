# CollectionsRT

.NET wrappers for WinRT generic collection types.

This library allows you to wrap WinRT generic collections' pointers and use them in .NET code as if they were normal .NET collections. It also provides custom marshallers for them so that they can be used in COM and P/Invoke interop scenarios without the need of having a WinMD containing these types.

### Supported  Types

- [x] `IIterable<T>` : wrapped as `Iterable<T>` implements `IEnumerable<T>`
- [x] `IIterator<T>` : wrapped as `Iterator<T>` implements `IEnumerator<T>`
- [x] `IVectorView<T>` : wrapped as `VectorView<T>` implements `IReadOnlyList<T>`
- [x] `IVector<T>` : wrapped as `Vector<T>` implements `IList<T>`
- [ ] `IMapView<K,V>` : wrapped as `MapView<K,V>` implements `IReadOnlyDictionary<K,V>`
- [ ] `IMap<K,V>` : wrapped as `Map<K,V>` implements `IDictionary<K,V>`
- [ ] `IObservableVector<T>` : wrapped as `ObservableVector<T>` implements `IList<T>`, `INotifyCollectionChanged`, `INotifyPropertyChanged`
- [ ] `IObservableMap<K,V>` : wrapped as `ObservableMap<K,V>` implements `IDictionary<K,V>`, `INotifyCollectionChanged`, `INotifyPropertyChanged`
- [ ] `IKeyValuePair<K,V>` : wrapped as [TBD]

### Supported Frameworks

- .NET Standard 2.0+
- .NET Framework 4.6.2+
- .NET Core 2.0+ (up to 3.1)
- .NET 9+ (`windows10.0.22621.0`+ TFMs)
- .NET Native AOT (w/ warnings)

> [!NOTE]  
> .NET Native (UWP) is not supported, however it should work on UWP's .NET Core (e.g. when compiling under Debug), they should also work fine for UWP on .NET 9+.

### Built-in marshallers (Classic COM & P/Invoke interop)

- `IterableMarshaller<T>`
- `IteratorMarshaller<T>`
- `VectorViewMarshaller<T>`
- `VectorMarshaller<T>`

### Built-in marshallers (Source Generated COM & P/Invoke interop)

- `SourceGenIterableMarshaller<T>`
- `SourceGenIteratorMarshaller<T>`
- `SourceGenVectorViewMarshaller<T>`
- `SourceGenVectorMarshaller<T>`

> [!NOTE]  
> The wrapper types are [annotated](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.marshalling.nativemarshallingattribute) with these marshaller types so they will get automatically marshalled when used in a **source generated** [COM](https://learn.microsoft.com/dotnet/standard/native-interop/comwrappers-source-generation) or [P/Invoke](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke-source-generation) context without the need of specifying the marshallers manually.