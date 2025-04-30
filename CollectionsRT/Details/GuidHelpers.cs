using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CollectionsRT.Details
{
    internal static class GuidHelpers
    {
        private readonly static byte[] interface_namespace = [0x11, 0xf4, 0x7a, 0xd5, 0x7b, 0x73, 0x42, 0xc0, 0xab, 0xae, 0x87, 0x8b, 0x1e, 0x16, 0xad, 0xee];

        private static Dictionary<Type, Guid> _cachedGuids = new Dictionary<Type, Guid>();

        private static bool TryGetDefaultInterfaceTypeForRuntimeClassType(Type type, out Type iface)
        {
            if (type.GetInterfaces()?.FirstOrDefault() is Type t)
            {
                iface = t;
                return true;
            }

            iface = null;
            return false;
        }

        private static Guid GetParameterizedIID(Type type)
        {
            if (typeof(IVector).IsAssignableFrom(type))
            {
                return typeof(IVector).GUID;
            }
            else if (typeof(IVectorView).IsAssignableFrom(type))
            {
                return typeof(IVectorView).GUID;
            }
            else if (typeof(IIterable).IsAssignableFrom(type))
            {
                return typeof(IIterable).GUID;
            }
            else if (typeof(IIterator).IsAssignableFrom(type))
            {
                return typeof(IIterator).GUID;
            }
            else
            {
                return type.GUID;
            }
        }

        private static string GetSignature(Type type)
        {
            if (type == typeof(object))
            {
                return "cinterface(IInspectable)";
            }

            if (type == typeof(string))
            {
                return "string";
            }

            if (type.IsValueType)
            {
                switch (type.Name)
                {
                    case "SByte": return "i1";
                    case "Byte": return "u1";
                    case "Int16": return "i2";
                    case "UInt16": return "u2";
                    case "Int32": return "i4";
                    case "UInt32": return "u4";
                    case "Int64": return "i8";
                    case "UInt64": return "u8";
                    case "Single": return "f4";
                    case "Double": return "f8";
                    case "Boolean": return "b1";
                    case "Char": return "c2";
                    case "Guid": return "g16";
                    default:
                        {
                            if (type.IsEnum)
                            {
                                var isFlags = type.IsDefined(typeof(FlagsAttribute));
                                return "enum(" + type.FullName + ";" + (isFlags ? "u4" : "i4") + ")";
                            }
                            if (!type.IsPrimitive)
                            {
                                var args = type.GetFields(BindingFlags.Instance | BindingFlags.Public).Select(fi => GetSignature(fi.FieldType));
                                return "struct(" + type.FullName + ";" + String.Join(";", args) + ")";
                            }
                            throw new InvalidOperationException("unsupported value type");
                        }
                }
            }

            if (type.IsGenericType)
            {
                var args = type.GetGenericArguments().Select(GetSignature);
                return "pinterface({" + GetParameterizedIID(type) + "};" + String.Join(";", args) + ")";
            }

            if (typeof(Delegate).IsAssignableFrom(type))
            {
                return "delegate({" + type.GUID + "})";
            }

            if (type.IsClass && TryGetDefaultInterfaceTypeForRuntimeClassType(type, out Type iface))
            {
                return "rc(" + type.FullName + ";" + GetSignature(iface) + ")";
            }

            return "{" + type.GUID.ToString() + "}";
        }

        private static Guid EncodeGuid(Span<byte> data)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte t = data[0];
                data[0] = data[3];
                data[3] = t;
                t = data[1];
                data[1] = data[2];
                data[2] = t;
                t = data[4];
                data[4] = data[5];
                data[5] = t;
                t = data[6];
                data[6] = data[7];
                data[7] = (byte)((t & 0x0f) | (5 << 4));
                data[8] = (byte)((data[8] & 0x3f) | 0x80);
            }

            return new Guid(data.Slice(0, 16).ToArray());
        }

        internal static Guid CreateGuidForGenericType<T>()
        {
            if (_cachedGuids.TryGetValue(typeof(T), out Guid guid))
            {
                return guid;
            }

            var sig = Encoding.UTF8.GetBytes(GetSignature(typeof(T)));
            var data = new byte[sig.Length + 16];
            interface_namespace.CopyTo(data, 0);
            sig.CopyTo(data, 16);
            using SHA1 sha = new SHA1CryptoServiceProvider();
            var encodedGuid = EncodeGuid(sha.ComputeHash(data));
            _cachedGuids.Add(typeof(T), encodedGuid);
            return encodedGuid;
        }
    }
}
