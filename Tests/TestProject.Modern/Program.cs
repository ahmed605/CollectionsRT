using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using CollectionsRT;
using CollectionsRT.Interop;
using CollectionsRT.Marshallers;

namespace TestProject.Modern
{
    public unsafe class Program
    {
        [TreatAsWindowsRuntimeClass]
        [RuntimeClassName("TestComponent.NonStaticTestClass")]
        [RuntimeClassDefaultInterface(typeof(INonStaticTestClass))]
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("E085521E-4B9F-42D8-A99C-DF0A29F6D44A")]
        interface INonStaticTestClass
        {
            int GetIids(uint* iidCount, Guid** iids);
            int GetRuntimeClassName(void** className);
            int GetTrustLevel(int* trustLevel);

            String TheString { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(HStringMarshaller))] get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClass
        {
            int GetIids(uint* iidCount, Guid** iids);
            int GetRuntimeClassName(void** className);
            int GetTrustLevel(int* trustLevel);

            void* Numbers { get; }
            void* Strings { get; }
            void* NestedStrings { get; }
            void* PropertySets { get; }
            void* ModifiableStrings { get; }
            void* ModifiableNumbers { get; }
            void* Classes { get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClassAutoMarshalled
        {
            int GetIids(uint* iidCount, Guid** iids);
            int GetRuntimeClassName(void** className);
            int GetTrustLevel(int* trustLevel);

            VectorView<int> Numbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<int>))] get; }
            VectorView<string> Strings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<string>))] get; }
            VectorView<VectorView<string>> NestedStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<VectorView<string>>))] get; }
            VectorView<PropertySet> PropertySets { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<PropertySet>))] get; }
            Vector<string> ModifiableStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<string>))] get; }
            Vector<int> ModifiableNumbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<int>))] get; }
            Vector<INonStaticTestClass> Classes { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<INonStaticTestClass>))] get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClassDotNetTypes
        {
            int GetIids(uint* iidCount, Guid** iids);
            int GetRuntimeClassName(void** className);
            int GetTrustLevel(int* trustLevel);

            IReadOnlyList<int> Numbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<int>))] get; }
            IReadOnlyList<string> Strings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<string>))] get; }
            IReadOnlyList<IReadOnlyList<string>> NestedStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<VectorView<string>>))] get; }
            IReadOnlyList<PropertySet> PropertySets { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<PropertySet>))] get; }
            IList<string> ModifiableStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<string>))] get; }
            IList<int> ModifiableNumbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<int>))] get; }
            IList<INonStaticTestClass> Classes { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<INonStaticTestClass>))] get; }
        }

        [DllImport("TestComponent.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        static extern int DllGetActivationFactory(void* activatableClassId, void** factory);

        static void TestManualMarshal(void* factory)
        {
            Console.WriteLine("Manual Marshal Test");

            IStaticTestClass testClass = (IStaticTestClass)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var view = new VectorView<int>(numbers);
            var count = view.Count;
            foreach (int i in view)
            {
                Console.WriteLine(i);
            }

            var strings = testClass.Strings;

            var view2 = new VectorView<string>(strings);
            var count2 = view2.Count;
            foreach (string i in view2)
            {
                Console.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            var view3 = new VectorView<VectorView<string>>(nestedStrings);
            foreach (var i in view3)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            var view4 = new VectorView<PropertySet>(propertySets);
            foreach (var i in view4)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;
            var vector = new Vector<string>(modifiableStrings);

            var count3 = vector.Count;
            Console.WriteLine($"Count: {count3}");

            foreach (string i in vector)
            {
                Console.WriteLine(i);
            }

            vector.Add("Modifiable Four!!");

            var count4 = vector.Count;
            Console.WriteLine($"Count: {count4}");

            foreach (string i in vector)
            {
                Console.WriteLine(i);
            }

            vector[2] = "Modifiable Five!!";

            foreach (string i in vector)
            {
                Console.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;
            var vector2 = new Vector<int>(modifiableNumbers);

            foreach (int i in vector2)
            {
                Console.WriteLine(i);
            }

            vector2.Add(100);

            foreach (int i in vector2)
            {
                Console.WriteLine(i);
            }

            vector2[2] = 700;

            foreach (int i in vector2)
            {
                Console.WriteLine(i);
            }

            var classes = testClass.Classes;
            var vector3 = new Vector<INonStaticTestClass>(classes);
            foreach (INonStaticTestClass i in vector3)
            {
                Console.WriteLine(i.TheString);
            }
        }

        static void TestAutoMarshal(void* factory)
        {
            Console.WriteLine("Auto Marshal Test");

            IStaticTestClassAutoMarshalled testClass = (IStaticTestClassAutoMarshalled)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var count = numbers.Count;
            foreach (int i in numbers)
            {
                Console.WriteLine(i);
            }

            var strings = testClass.Strings;
            var count2 = strings.Count;
            foreach (string i in strings)
            {
                Console.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            foreach (var i in nestedStrings)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            foreach (var i in propertySets)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;

            var count3 = modifiableStrings.Count;
            Console.WriteLine($"Count: {count3}");

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            modifiableStrings.Add("Modifiable Four!!");

            var count4 = modifiableStrings.Count;
            Console.WriteLine($"Count: {count4}");

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            modifiableStrings[2] = "Modifiable Five!!";

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            modifiableNumbers.Add(100);

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            modifiableNumbers[2] = 700;

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            var classes = testClass.Classes;
            foreach (INonStaticTestClass i in classes)
            {
                Console.WriteLine(i.TheString);
            }
        }

        static void TestDotNetMarshal(void* factory)
        {
            Console.WriteLine(".NET Marshal Test");

            IStaticTestClassDotNetTypes testClass = (IStaticTestClassDotNetTypes)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var count = numbers.Count;
            foreach (int i in numbers)
            {
                Console.WriteLine(i);
            }

            var strings = testClass.Strings;
            var count2 = strings.Count;
            foreach (string i in strings)
            {
                Console.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            foreach (var i in nestedStrings)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            foreach (var i in propertySets)
            {
                Console.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Console.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;

            var count3 = modifiableStrings.Count;
            Console.WriteLine($"Count: {count3}");

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            modifiableStrings.Add("Modifiable Four!!");

            var count4 = modifiableStrings.Count;
            Console.WriteLine($"Count: {count4}");

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            modifiableStrings[2] = "Modifiable Five!!";

            foreach (string i in modifiableStrings)
            {
                Console.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            modifiableNumbers.Add(100);

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            modifiableNumbers[2] = 700;

            foreach (int i in modifiableNumbers)
            {
                Console.WriteLine(i);
            }

            var classes = testClass.Classes;
            foreach (INonStaticTestClass i in classes)
            {
                Console.WriteLine(i.TheString);
            }
        }

        public static void Main(string[] args)
        {
            void* factory = default;
            Marshal.ThrowExceptionForHR(DllGetActivationFactory((void*)WinRT.MarshalString.FromManaged("TestComponent.TestClass"), &factory));

            TestManualMarshal(factory);
            TestAutoMarshal(factory);
            TestDotNetMarshal(factory);
        }
    }

    internal unsafe class HStringMarshaller : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string cookie) => new HStringMarshaller();

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return WinRT.MarshalString.FromAbi(pNativeData);
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            return WinRT.MarshalString.FromManaged((string)managedObj);
        }

        public void CleanUpManagedData(object managedObj) { }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            WinRT.MarshalString.DisposeAbi(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return sizeof(void*);
        }
    }
}

