using System;

namespace CollectionsRT.Interop
{
    public class RuntimeClassDefaultInterfaceAttribute : Attribute
    {
        public RuntimeClassDefaultInterfaceAttribute(Type defaultInterface)
        {
            DefaultInterface = defaultInterface;
        }

        public Type DefaultInterface { get; }
    }
}
