#pragma once

#include "TestClass.g.h"

namespace winrt::TestComponent::implementation
{
    struct TestClass : TestClassT<TestClass>
    {
        TestClass() = default;

		static winrt::Windows::Foundation::Collections::IVectorView<int32_t> Numbers();
		static winrt::Windows::Foundation::Collections::IVectorView<hstring> Strings();
		static winrt::Windows::Foundation::Collections::IVectorView<winrt::Windows::Foundation::Collections::IVectorView<hstring>> NestedStrings();
        static winrt::Windows::Foundation::Collections::IVectorView<winrt::Windows::Foundation::Collections::PropertySet> PropertySets();
        static winrt::Windows::Foundation::Collections::IVector<hstring> ModifiableStrings();
        static winrt::Windows::Foundation::Collections::IVector<int32_t> ModifiableNumbers();
		static winrt::Windows::Foundation::Collections::IVector<winrt::TestComponent::NonStaticTestClass> Classes();
		static winrt::Windows::Foundation::Collections::IVector<bool> Booleans();
    };
}

namespace winrt::TestComponent::factory_implementation
{
    struct TestClass : TestClassT<TestClass, implementation::TestClass>
    {
    };
}
