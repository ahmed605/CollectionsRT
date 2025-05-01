#include "pch.h"
#include "NonStaticTestClass.h"
#include "NonStaticTestClass.g.cpp"

namespace winrt::TestComponent::implementation
{
    hstring NonStaticTestClass::TheString()
    {
        return L"Hello!";
    }
}
