#include "pch.h"
#include "TestClass.h"
#include "TestClass.g.cpp"
#include "NonStaticTestClass.h"

namespace winrt::TestComponent::implementation
{
	winrt::Windows::Foundation::Collections::IVectorView<int32_t> TestClass::Numbers()
	{
		auto numbers = single_threaded_vector<int32_t>();
		numbers.Append(1);
		numbers.Append(2);
		numbers.Append(3);
		return numbers.GetView();
	}

	winrt::Windows::Foundation::Collections::IVectorView<hstring> TestClass::Strings()
	{
		auto strings = single_threaded_vector<hstring>();
		strings.Append(L"One");
		strings.Append(L"Two");
		strings.Append(L"Three");
		return strings.GetView();
	}

	winrt::Windows::Foundation::Collections::IVectorView<winrt::Windows::Foundation::Collections::IVectorView<hstring>> TestClass::NestedStrings()
	{
		auto nestedStrings = single_threaded_vector<winrt::Windows::Foundation::Collections::IVectorView<hstring>>();
		auto strings1 = single_threaded_vector<hstring>();
		strings1.Append(L"Nested [1] - One");
		strings1.Append(L"Nested [1] - Two");
		strings1.Append(L"Nested [1] - Three");
		nestedStrings.Append(strings1.GetView());

		auto strings2 = single_threaded_vector<hstring>();
		strings2.Append(L"Nested [2] - One");
		strings2.Append(L"Nested [2] - Two");
		strings2.Append(L"Nested [2] - Three");
		nestedStrings.Append(strings2.GetView());

		return nestedStrings.GetView();
	}

	winrt::Windows::Foundation::Collections::IVectorView<winrt::Windows::Foundation::Collections::PropertySet> TestClass::PropertySets()
	{
		auto propertySets = single_threaded_vector<winrt::Windows::Foundation::Collections::PropertySet>();
		auto propertySet1 = winrt::Windows::Foundation::Collections::PropertySet();
		propertySet1.Insert(L"Key1", box_value(1));
		propertySet1.Insert(L"Key2", box_value(L"Value2"));
		propertySets.Append(propertySet1);

		auto propertySet2 = winrt::Windows::Foundation::Collections::PropertySet();
		propertySet2.Insert(L"KeyA", box_value(10));
		propertySet2.Insert(L"KeyB", box_value(L"ValueB"));
		propertySets.Append(propertySet2);

		return propertySets.GetView();
	}

	winrt::Windows::Foundation::Collections::IVector<hstring> TestClass::ModifiableStrings()
	{
		auto modifiableStrings = single_threaded_vector<hstring>();
		modifiableStrings.Append(L"Modifiable One");
		modifiableStrings.Append(L"Modifiable Two");
		modifiableStrings.Append(L"Modifiable Three");
		return modifiableStrings;
	}

	winrt::Windows::Foundation::Collections::IVector<int32_t> TestClass::ModifiableNumbers()
	{
		auto modifiableNumbers = single_threaded_vector<int32_t>();
		modifiableNumbers.Append(10);
		modifiableNumbers.Append(20);
		modifiableNumbers.Append(30);
		return modifiableNumbers;
	}

	winrt::Windows::Foundation::Collections::IVector<winrt::TestComponent::NonStaticTestClass> TestClass::Classes()
	{
		auto classes = single_threaded_vector<winrt::TestComponent::NonStaticTestClass>();
		classes.Append(winrt::make<winrt::TestComponent::implementation::NonStaticTestClass>());
		return classes;
	}

	winrt::Windows::Foundation::Collections::IVector<bool> TestClass::Booleans()
	{
		auto booleans = single_threaded_vector<bool>();
		booleans.Append(true);
		booleans.Append(false);
		booleans.Append(false);
		booleans.Append(true);
		return booleans;
	}
}
