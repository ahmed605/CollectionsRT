using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using CollectionsRT;
using CollectionsRT.Interop;
using CollectionsRT.Marshallers;

namespace TestProject
{
    public unsafe class Program
    {
        [TreatAsWindowsRuntimeClass]
        [RuntimeClassName("TestComponent.NonStaticTestClass")]
        [RuntimeClassDefaultInterface(typeof(INonStaticTestClass))]
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("E085521E-4B9F-42D8-A99C-DF0A29F6D44A")]
        interface INonStaticTestClass
        {
            String TheString { [return: MarshalAs(UnmanagedType.HString)] get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClass
        {
            void* Numbers { get; }
            void* Strings { get; }
            void* NestedStrings { get; }
            void* PropertySets { get; }
            void* ModifiableStrings { get; }
            void* ModifiableNumbers { get; }
            void* Classes { get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClassAutoMarshalled
        {
            VectorView<int> Numbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<int>))]  get; }
            VectorView<string> Strings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<string>))] get; }
            VectorView<VectorView<string>> NestedStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<VectorView<string>>))] get; }
            VectorView<PropertySet> PropertySets { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<PropertySet>))] get; }
            Vector<string> ModifiableStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<string>))] get; }
            Vector<int> ModifiableNumbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<int>))] get; }
            Vector<INonStaticTestClass> Classes { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<INonStaticTestClass>))] get; }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("AE580A22-5CFC-4F16-9230-24EB36BA128C")]
        interface IStaticTestClassDotNetTypes
        {
            IReadOnlyList<int> Numbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<int>))] get; }
            IReadOnlyList<string> Strings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<string>))] get; }
            IReadOnlyList<IReadOnlyList<string>> NestedStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<VectorView<string>>))] get; }
            IReadOnlyList<PropertySet> PropertySets { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorViewMarshaller<PropertySet>))] get; }
            IList<string> ModifiableStrings { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<string>))] get; }
            IList<int> ModifiableNumbers { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<int>))] get; }
            IList<INonStaticTestClass> Classes { [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(VectorMarshaller<INonStaticTestClass>))] get; }
        }

        [DllImport("TestComponent.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true)]
        static extern int DllGetActivationFactory([MarshalAs(UnmanagedType.HString)] string activatableClassId, void** factory);

        static void TestManualMarshal(void* factory)
        {
            Debug.WriteLine("Manual Marshal Test");

            IStaticTestClass testClass = (IStaticTestClass)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var view = new VectorView<int>(numbers);
            var count = view.Count;
            foreach (int i in view)
            {
                Debug.WriteLine(i);
            }

            var strings = testClass.Strings;

            var view2 = new VectorView<string>(strings);
            var count2 = view2.Count;
            foreach (string i in view2)
            {
                Debug.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            var view3 = new VectorView<VectorView<string>>(nestedStrings);
            foreach (var i in view3)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            var view4 = new VectorView<PropertySet>(propertySets);
            foreach (var i in view4)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;
            var vector = new Vector<string>(modifiableStrings);

            var count3 = vector.Count;
            Debug.WriteLine($"Count: {count3}");

            foreach (string i in vector)
            {
                Debug.WriteLine(i);
            }

            vector.Add("Modifiable Four!!");

            var count4 = vector.Count;
            Debug.WriteLine($"Count: {count4}");

            foreach (string i in vector)
            {
                Debug.WriteLine(i);
            }

            vector[2] = "Modifiable Five!!";

            foreach (string i in vector)
            {
                Debug.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;
            var vector2 = new Vector<int>(modifiableNumbers);

            foreach (int i in vector2)
            {
                Debug.WriteLine(i);
            }

            vector2.Add(100);

            foreach (int i in vector2)
            {
                Debug.WriteLine(i);
            }

            vector2[2] = 700;

            foreach (int i in vector2)
            {
                Debug.WriteLine(i);
            }

            var classes = testClass.Classes;
            var vector3 = new Vector<INonStaticTestClass>(classes);
            foreach (INonStaticTestClass i in vector3)
            {
                Debug.WriteLine(i.TheString);
            }
        }

        static void TestAutoMarshal(void* factory)
        {
            Debug.WriteLine("Auto Marshal Test");

            IStaticTestClassAutoMarshalled testClass = (IStaticTestClassAutoMarshalled)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var count = numbers.Count;
            foreach (int i in numbers)
            {
                Debug.WriteLine(i);
            }

            var strings = testClass.Strings;
            var count2 = strings.Count;
            foreach (string i in strings)
            {
                Debug.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            foreach (var i in nestedStrings)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            foreach (var i in propertySets)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;

            var count3 = modifiableStrings.Count;
            Debug.WriteLine($"Count: {count3}");

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            modifiableStrings.Add("Modifiable Four!!");

            var count4 = modifiableStrings.Count;
            Debug.WriteLine($"Count: {count4}");

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            modifiableStrings[2] = "Modifiable Five!!";

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            modifiableNumbers.Add(100);

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            modifiableNumbers[2] = 700;

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            var classes = testClass.Classes;
            foreach (INonStaticTestClass i in classes)
            {
                Debug.WriteLine(i.TheString);
            }
        }

        static void TestDotNetMarshal(void* factory)
        {
            Debug.WriteLine(".NET Marshal Test");

            IStaticTestClassDotNetTypes testClass = (IStaticTestClassDotNetTypes)Marshal.GetObjectForIUnknown((IntPtr)factory);
            var numbers = testClass.Numbers;

            var count = numbers.Count;
            foreach (int i in numbers)
            {
                Debug.WriteLine(i);
            }

            var strings = testClass.Strings;
            var count2 = strings.Count;
            foreach (string i in strings)
            {
                Debug.WriteLine(i);
            }

            var nestedStrings = testClass.NestedStrings;
            foreach (var i in nestedStrings)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (string j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var propertySets = testClass.PropertySets;
            foreach (var i in propertySets)
            {
                Debug.WriteLine($"Count: {i.Count}");
                foreach (var j in i)
                {
                    Debug.WriteLine(j);
                }
            }

            var modifiableStrings = testClass.ModifiableStrings;

            var count3 = modifiableStrings.Count;
            Debug.WriteLine($"Count: {count3}");

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            modifiableStrings.Add("Modifiable Four!!");

            var count4 = modifiableStrings.Count;
            Debug.WriteLine($"Count: {count4}");

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            modifiableStrings[2] = "Modifiable Five!!";

            foreach (string i in modifiableStrings)
            {
                Debug.WriteLine(i);
            }

            var modifiableNumbers = testClass.ModifiableNumbers;

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            modifiableNumbers.Add(100);

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            modifiableNumbers[2] = 700;

            foreach (int i in modifiableNumbers)
            {
                Debug.WriteLine(i);
            }

            var classes = testClass.Classes;
            foreach (INonStaticTestClass i in classes)
            {
                Debug.WriteLine(i.TheString);
            }
        }

        public static void Main(string[] args)
        {
            void* factory = default;
            Marshal.ThrowExceptionForHR(DllGetActivationFactory("TestComponent.TestClass", &factory));

            TestManualMarshal(factory);
            TestAutoMarshal(factory);
            TestDotNetMarshal(factory);
        }
    }
}
