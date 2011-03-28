using System;
using System.IO;
using System.Text;
using System.Reflection;

namespace Undelete
{
    /// <summary>
    /// Represents a stream activator, which acts like the <see cref="Activator"/> class, but with object information contained in streams.
    /// </summary>
    public sealed class StreamActivator
    {
        /// <summary>
        /// Creates an instance of the specified type contained in the specified stream.
        /// </summary>
        public static object CreateInstance(Type type, Stream stream)
        {
            BinaryReader br = new BinaryReader(stream, Encoding.ASCII);
            object obj = Activator.CreateInstance(type);
            foreach(FieldInfo field in type.GetFields())
            {
                if((!field.IsPublic) || (field.IsStatic))
                    continue;
                if(field.FieldType == typeof(Boolean))
                    field.SetValue(obj, br.ReadBoolean());
                else if(field.FieldType == typeof(Byte))
                    field.SetValue(obj, br.ReadByte());
                else if(field.FieldType == typeof(Char))
                    field.SetValue(obj, br.ReadChar());
                else if(field.FieldType == typeof(Decimal))
                    field.SetValue(obj, br.ReadDecimal());
                else if(field.FieldType == typeof(Double))
                    field.SetValue(obj, br.ReadDouble());
                else if(field.FieldType == typeof(Int16))
                    field.SetValue(obj, br.ReadInt16());
                else if(field.FieldType == typeof(Int32))
                    field.SetValue(obj, br.ReadInt32());
                else if(field.FieldType == typeof(Int64))
                    field.SetValue(obj, br.ReadInt64());
                else if(field.FieldType == typeof(SByte))
                    field.SetValue(obj, br.ReadSByte());
                else if(field.FieldType == typeof(Single))
                    field.SetValue(obj, br.ReadSingle());
                else if(field.FieldType == typeof(UInt16))
                    field.SetValue(obj, br.ReadUInt16());
                else if(field.FieldType == typeof(UInt32))
                    field.SetValue(obj, br.ReadUInt32());
                else if(field.FieldType == typeof(UInt64))
                    field.SetValue(obj, br.ReadUInt64());
                else if(field.FieldType == typeof(Byte[]))
                {
                    ArrayLengthAttribute[] attrs = (ArrayLengthAttribute[])field.GetCustomAttributes(typeof(ArrayLengthAttribute), true);
                    if(attrs.Length != 1)
                        throw new InvalidOperationException("Required a single specified length given by the ArrayLength Attribute");
                    field.SetValue(obj, br.ReadBytes(attrs[0].ArrayLength));
                }
                else if(field.FieldType == typeof(Char[]))
                {
                    ArrayLengthAttribute[] attrs = (ArrayLengthAttribute[])field.GetCustomAttributes(typeof(ArrayLengthAttribute), true);
                    if(attrs.Length != 1)
                        throw new InvalidOperationException("Required a single specified length given by the ArrayLength Attribute");
                    field.SetValue(obj, br.ReadChars(attrs[0].ArrayLength));
                }
                else if(field.FieldType == typeof(String))
                {
                    ArrayLengthAttribute[] attrs = (ArrayLengthAttribute[])field.GetCustomAttributes(typeof(ArrayLengthAttribute), true);
                    if(attrs.Length == 1)
                        field.SetValue(obj, new string(br.ReadChars(attrs[0].ArrayLength)));
                    else
                        field.SetValue(obj, br.ReadString());
                }
                else
                    throw new NotSupportedException("Type not supported in read");
            }
            return obj;
        }

        /// <summary>
        /// Creates an instance of the specified type contained in the specified bytes.
        /// </summary>
        public static object CreateInstance(Type type, byte[] data)
        {
            return CreateInstance(type, new MemoryStream(data, 0, data.Length, false));
        }

        /// <summary>
        /// Creates an instance of the specified type contained in the specified range of bytes.
        /// </summary>
        public static object CreateInstance(Type type, byte[] data, int start)
        {
            return CreateInstance(type, new MemoryStream(data, start, data.Length - start, false));
        }
    }
}
