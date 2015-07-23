using Karambit.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Karambit.IO
{
    public class ObjectWriter
    {
        #region Fields
        private BinaryWriter writer;
        private Encoding encoding = Encoding.UTF8;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding {
            get {
                return encoding;
            }
            set {
                this.encoding = value;
                this.writer = new BinaryWriter(writer.BaseStream, value);
            }
        }

        /// <summary>
        /// Gets the base stream.
        /// </summary>
        /// <value>The base stream.</value>
        public Stream BaseStream {
            get {
                return writer.BaseStream;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Writes a byte to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteByte(byte val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of bytes to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteBytes(byte[] vals) {
            writer.Write(vals);
        }

        /// <summary>
        /// Writes an sbyte to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteSByte(sbyte val) {
            writer.Write(val);
        }
        
        /// <summary>
        /// Writes an array of signed bytes to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteSBytes(sbyte[] vals) {
            foreach (sbyte val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes a short to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteShort(short val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of shorts to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteShorts(short[] vals) {
            foreach (short val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes an unsigned short to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteUShort(ushort val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of unsigned shorts to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteUShorts(ushort[] vals) {
            foreach (ushort val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes an int to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteInt(int val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of ints to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteInts(int[] vals) {
            foreach (int val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes an unsigned int to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteUInt(uint val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of unsigned ints to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteUInts(uint[] vals) {
            foreach (uint val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes a long to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteLong(long val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of longs to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteLongs(long[] vals) {
            foreach (long val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes an unsigned long to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteULong(ulong val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of unsigned longs to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteULongs(ulong[] vals) {
            foreach (ulong val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes a boolean to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteBool(bool val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of booleans to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteBools(bool[] vals) {
            foreach (bool val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes a float to the stream.
        /// </summary>
        /// <param name="val">The value.</param>
        public void WriteFloat(float val) {
            writer.Write(val);
        }

        /// <summary>
        /// Writes an array of floats to the stream.
        /// </summary>
        /// <param name="vals">The value.</param>
        public void WriteFloats(float[] vals) {
            foreach (float val in vals)
                writer.Write(val);
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="format">The format.</param>
        /// <param name="size">The size.</param>
        public void WriteString(string str, StringFormat format=StringFormat.LengthEncoded, int size=0) {
            if (format == StringFormat.FixedSize)
                WriteFixedString(str, size);
            else if (format == StringFormat.LengthEncoded)
                writer.Write(str);
            else if (format == StringFormat.NullTerminated)
                WriteTerminatedString(str);
            else
                WriteLengthString(str, format);
        }

        /// <summary>
        /// Writes a string with a prefixed length type.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="format">The format.</param>
        /// <exception cref="System.Exception">
        /// The string is too long for the provided length type
        /// or
        /// The string is too long for the provided length type
        /// or
        /// The string is too long for the provided length type
        /// or
        /// Invalid format type provided
        /// </exception>
        private void WriteLengthString(string str, StringFormat format) {
            // write length
            if (format == StringFormat.LengthTiny) {
                if (str.Length > byte.MaxValue)
                    throw new Exception("The string is too long for the provided length type");
                writer.Write((byte)str.Length);
            } else if (format == StringFormat.LengthMedium) {
                if (str.Length > ushort.MaxValue)
                    throw new Exception("The string is too long for the provided length type");
                writer.Write((ushort)str.Length);
            } else if (format == StringFormat.LengthLarge) {
                if (str.Length > int.MaxValue)
                    throw new Exception("The string is too long for the provided length type");
                writer.Write((int)str.Length);
            } else {
                throw new Exception("Invalid format type provided");
            }

            // write data
            writer.Write(encoding.GetBytes(str));
        }

        /// <summary>
        /// Writes a null-terminated string.
        /// </summary>
        /// <param name="str">The string.</param>
        private void WriteTerminatedString(string str) {
            // generate data
            byte[] strBytes = encoding.GetBytes(str + '\0');

            // write
            writer.Write(strBytes);
        }

        /// <summary>
        /// Writes a fixed length string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="System.InvalidOperationException">
        /// The provided string is larger than the allowed length
        /// or
        /// The selected encoding does not support fixed-length strings
        /// </exception>
        private void WriteFixedString(string str, int size) {
            // check length
            if (str.Length > size)
                throw new InvalidOperationException("The provided string is larger than the allowed length");

            // check encoding
            if (!encoding.IsSingleByte)
                throw new InvalidOperationException("The selected encoding does not support fixed-length strings");

            // get data
            byte[] data = new byte[size];
            byte[] strBytes = encoding.GetBytes(str);
            Array.Copy(strBytes, data, strBytes.Length);

            writer.Write(data);
        }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void WriteObject(object obj) {
            if (obj is byte)
                WriteByte((byte)obj);
            else if (obj is sbyte)
                WriteSByte((sbyte)obj);
            else if (obj is short)
                WriteShort((short)obj);
            else if (obj is ushort)
                WriteUShort((ushort)obj);
            else if (obj is int)
                WriteInt((int)obj);
            else if (obj is uint)
                WriteUInt((uint)obj);
            else if (obj is long)
                WriteLong((long)obj);
            else if (obj is ulong)
                WriteULong((ulong)obj);
            else if (obj is float)
                WriteFloat((float)obj);
            else if (obj is bool)
                WriteBool((bool)obj);
            else if (obj is string)
                WriteString((string)obj);
            else if (obj is ISerializable)
                ((ISerializable)obj).Serialize(this);
            else
                WriteObjectStructured(obj);
        }

        private void WriteObjectStructured(object obj) {
            // get type
            Type type = obj.GetType();
            Dictionary<string, object> members = new Dictionary<string, object>();

            // handle lists and dictionaries
            if (type is IDictionary) {
                IDictionary dict = (IDictionary)obj;
                WriteInt(dict.Count);

                foreach (KeyValuePair<string, object> kv in dict) {
                    WriteString(kv.Key);
                    WriteObject(kv.Value);
                }

                return;
            } else if (type is IList) {
                IList list = (IList)obj;
                WriteInt(list.Count);

                foreach (object v in list) {
                    WriteObject(v);
                }

                return;
            } 

            // get members
            foreach (FieldInfo info in type.GetFields())
                members.Add(info.Name, info.GetValue(obj));

            foreach (PropertyInfo info in type.GetProperties())
                members.Add(info.Name, info.GetValue(obj));

            // write
            foreach (KeyValuePair<string, object> member in members) {
                // key
                WriteString(member.Key);

                // value
                if (member.Value is IDictionary)
                    WriteObjectStructured(member.Value);
                else if (member.Value is IList)
                    WriteObjectStructured(member.Value);
                else
                    WriteObject(member.Value);
            }
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ObjectWriter(Stream stream) {
            this.writer = new BinaryWriter(stream, encoding);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        public ObjectWriter(Stream stream, Encoding encoding) {
            this.writer = new BinaryWriter(stream, encoding);
            this.encoding = encoding;
        }
        #endregion
    }
}
