using CollectionsRT;
using CollectionsRT.Details;
using CollectionsRT.Marshallers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace TestProject
{
    public unsafe class Program
    {
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
        }

        public static void Main(string[] args)
        {
            void* factory = default;
            Marshal.ThrowExceptionForHR(DllGetActivationFactory("TestComponent.TestClass", &factory));

            TestManualMarshal(factory);
            TestAutoMarshal(factory);
        }
    }
}
