using System;
namespace CollectionsRT.Interop
{
    public class RuntimeClassNameAttribute : Attribute
    {
        public RuntimeClassNameAttribute(string runtimeClassName)
        {
            RuntimeClassName = runtimeClassName;
        }

        public string RuntimeClassName { get; }
    }
}
