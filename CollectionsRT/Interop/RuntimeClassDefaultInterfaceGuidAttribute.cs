using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionsRT.Interop
{
    public class RuntimeClassDefaultInterfaceGuidAttribute : Attribute
    {
        public RuntimeClassDefaultInterfaceGuidAttribute(Guid guid)
        {
            Guid = guid;
        }

        public RuntimeClassDefaultInterfaceGuidAttribute(string guid) : this(new Guid(guid)) { }

        public Guid Guid { get; }
    }
}
