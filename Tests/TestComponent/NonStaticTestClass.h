#pragma once
#include "NonStaticTestClass.g.h"

namespace winrt::TestComponent::implementation
{
    struct NonStaticTestClass : NonStaticTestClassT<NonStaticTestClass>
    {
        NonStaticTestClass() = default;

        hstring TheString();
    };
}
