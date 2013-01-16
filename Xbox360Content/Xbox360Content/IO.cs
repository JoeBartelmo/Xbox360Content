/*  Copyright (C) 2013 Joseph Bartelmo

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Xbox360Content
{
    /// <summary>
    /// Enumeration for type of Endian (reversed bits or not)
    /// </summary>
    public enum Endian : byte
    {

        /// <summary>
        /// Standard for BitConverter, 0x938 would be 38 09
        /// </summary>
        Little = 0x0,
        /// <summary>
        /// Human terms, 0x938 would be 09 38
        /// </summary>
        Big = 0x1
    }

    /// <summary>
    /// Input/Output class; for reading and writing to files.
    /// </summary>
    public class IO : IDisposable
    {
        //Declare Variables
        private Stream _stream;
        private BinaryReader _in;
        private BinaryWriter _out;
        private ASCIIEncoding _ascii;
        private UnicodeEncoding _unicode;
        private byte[] temp;

        //Public Variables
        private Endian type = Endian.Little;
        /// <summary>
        /// Defines which way to read and write the
        /// stream
        /// </summary>
        public Endian Endianness { get { return type; } set { type = value; } }
        /// <summary>
        /// Displays the length in bytes of the 
        /// stream.
        /// </summary>
        public long Length { get { return _stream.Length; } }
        /// <summary>
        /// Displays the position of the stream
        /// </summary>
        public long Position { get { return _in.BaseStream.Position; } set { Seek(value); } }

        //.ctor
        private void _base(Stream stream)
        {
            if (stream != null)
            {
                _stream = stream;
                _in = new BinaryReader(_stream);
                _out = new BinaryWriter(_stream);
                _ascii = new ASCIIEncoding();
                _unicode = new UnicodeEncoding();
            }
            else
                throw new ArgumentNullException();
        }
        /// <summary>
        /// Allows for Input/Output of binary, assumes
        /// that this is a Little Endian file
        /// Streams
        /// </summary>
        /// <param name="stream">Predefined stream</param>
        public IO(Stream stream)
        {
            _base(stream);
        }
        /// <summary>
        /// Allows for Input/Output of binary
        /// Streams, gives user input on 
        /// endianess
        /// </summary>
        /// <param name="stream">Predefined stream</param>
        /// <param name="endian">Endianess of file</param>
        public IO(Stream stream, Endian endian)
        {
            Endianness = endian;
            _base(stream);
        }
        /// <summary>
        /// Allows for Input/Output of binary
        /// Streams
        /// </summary>
        /// <param name="bytes">Predefined byte array</param>
        public IO(byte[] bytes)
        {
            if (bytes != null)
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    _base(ms);
            }
            else
                throw new ArgumentNullException();
        }
        /// <summary>
        /// Allows for Input/Output of binary
        /// Streams
        /// </summary>
        /// <param name="bytes">Predefined byte array</param>
        /// <param name="endian">Endianess of file</param>
        public IO(byte[] bytes, Endian endian)
        {
            if (bytes != null)
            {
                Endianness = endian;
                using (MemoryStream ms = new MemoryStream(bytes))
                    _base(ms);
            }
            else
                throw new ArgumentNullException();
        }
        /// <summary>
        /// Allows for Input/Output of binary
        /// Streams.
        /// </summary>
        /// <param name="file">Location of a file on a system</param>
        public IO(string file)
        {
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                    _base(fs);
            }
            else
                throw new FileNotFoundException();
        }
        /// <summary>
        /// Allows for Input/Output of binary
        /// Streams.
        /// </summary>
        /// <param name="file">Location of a file on a system</param>
        /// <param name="endian">Endianess of file</param>
        public IO(string file, Endian endian)
        {
            if (File.Exists(file))
            {
                Endianness = endian;
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                    _base(fs);
            }
            else
                throw new FileNotFoundException();
        }

        //Methods
        /// <summary>
        /// Sets the position of the file
        /// </summary>
        /// <param name="position">Desired Position</param>
        public void Seek(long position)
        {
            _in.BaseStream.Position = _out.BaseStream.Position = position;
        }

        //Read
        /// <summary>
        /// Reads a byte array
        /// </summary>
        /// <param name="count">Length of array</param>
        /// <param name="endian">Endian of array</param>
        /// <returns>unsigned byte array</returns>
        public byte[] ReadBytes(int count, Endian endian)
        {
            temp = _in.ReadBytes(count);
            if (endian == Endian.Big)
                Array.Reverse(temp);
            Seek(_in.BaseStream.Position);
            return temp;
        }
        /// <summary>
        /// Reads a byte array
        /// </summary>
        /// <param name="count">Length of array</param>
        /// <returns>unsigned byte array</returns>
        public byte[] ReadBytes(int count)
        {
            return ReadBytes(count, this.Endianness);
        }
        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <returns>unsigned byte</returns>
        public byte ReadByte()
        {
            byte i = _in.ReadByte();
            _out.BaseStream.Position += 1;
            return i;
        }
        /// <summary>
        /// Reads a signed byte
        /// </summary>
        /// <returns>signed byte</returns>
        public sbyte ReadSByte()
        {
            sbyte i = _in.ReadSByte();
            _out.BaseStream.Position += 1;
            return i;
        }
        /// <summary>
        /// Reads a true or false statement
        /// </summary>
        /// <returns>boolean</returns>
        public bool ReadBool()
        {
            return (ReadByte() == 0x1);
        }
        /// <summary>
        /// Reads a 16 bit integer
        /// </summary>
        /// <param name="endian">Endianess of integer</param>
        /// <returns>signed short</returns>
        public short ReadInt16(Endian endian)
        {
            ReadBytes(0x2, endian);
            return BitConverter.ToInt16(temp, 0);
        }
        /// <summary>
        /// Reads a 16 bit integer
        /// </summary>
        /// <returns>signed short</returns>
        public short ReadInt16()
        {
            ReadBytes(0x2, Endianness);
            return BitConverter.ToInt16(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 16 bit integer
        /// </summary>
        /// <param name="endian">endianess of integer</param>
        /// <returns></returns>
        public ushort ReadUInt16(Endian endian)
        {
            ReadBytes(0x2, endian);
            return BitConverter.ToUInt16(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 16 bit integer
        /// </summary>
        /// <returns>unsigned uint 16</returns>
        public ushort ReadUInt16()
        {
            ReadBytes(0x2, Endianness);
            return BitConverter.ToUInt16(temp, 0);
        }
        /// <summary>
        /// Reads an signed 24 bit integer
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>signed int24</returns>
        public int ReadInt24(Endian endian)
        {
            List<byte> bytes = new List<byte>(ReadBytes(3, endian));
            bytes.Add(0);
            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }
        /// <summary>
        /// Reads an signed 24 bit integer
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>signed int24</returns>
        public int ReadInt24()
        {
            List<byte> bytes = new List<byte>(ReadBytes(3, this.type));
            bytes.Add(0);
            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }
        /// <summary>
        /// Reads a 32 bit integer
        /// </summary>
        /// <param name="endian">Endianess of integer</param>
        /// <returns>signed int 32</returns>
        public int ReadInt32(Endian endian)
        {
            ReadBytes(0x4, endian);
            return BitConverter.ToInt32(temp, 0);
        }
        /// <summary>
        /// Reads an signed 32 bit integer
        /// </summary>
        /// <returns>signed int 32</returns>
        public int ReadInt32()
        {
            ReadBytes(0x4, Endianness);
            return BitConverter.ToInt32(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 32 bit integer
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>unsigned uint 32</returns>
        public uint ReadUInt32(Endian endian)
        {
            ReadBytes(0x4, endian);
            return BitConverter.ToUInt32(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 32 bit integer
        /// </summary>
        /// <returns>unsigned uint 32</returns>
        public uint ReadUInt32()
        {
            ReadBytes(0x4, Endianness);
            return BitConverter.ToUInt32(temp, 0);
        }
        /// <summary>
        /// Reads a 64 bit integer
        /// </summary>
        /// <param name="endian">Endianess of integer</param>
        /// <returns>signed in 64</returns>
        public long ReadInt64(Endian endian)
        {
            ReadBytes(0x8, endian);
            return BitConverter.ToInt64(temp, 0);
        }
        /// <summary>
        /// Reads a 64 bit integer
        /// </summary>
        /// <returns>signed int 64</returns>
        public long ReadInt64()
        {
            ReadBytes(0x8, Endianness);
            return BitConverter.ToInt64(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 64 bit integer
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>unsigned uint 64</returns>
        public ulong ReadUInt64(Endian endian)
        {
            ReadBytes(0x8, endian);
            return BitConverter.ToUInt64(temp, 0);
        }
        /// <summary>
        /// Reads an unsigned 64 bit integer
        /// </summary>
        /// <returns>unsigned uint 64</returns>
        public ulong ReadUInt64()
        {
            ReadBytes(0x8, Endianness);
            return BitConverter.ToUInt64(temp, 0);
        }
        /// <summary>
        /// Read Float value
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>Float</returns>
        public float ReadFloat(Endian endian)
        {
            ReadBytes(0x4, endian);
            return BitConverter.ToSingle(temp, 0);
        }
        /// <summary>
        /// Read Float value
        /// </summary>
        /// <returns>Float</returns>
        public float ReadFloat()
        {
            ReadBytes(0x4, Endianness);
            return BitConverter.ToSingle(temp, 0);
        }
        /// <summary>
        /// Read Decimal value
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>Decimal</returns>
        public double ReadDecimal(Endian endian)
        {
            ReadBytes(0x4, endian);
            return BitConverter.ToDouble(temp, 0);
        }
        /// <summary>
        /// Read Decimal value
        /// </summary>
        /// <returns>Decimal</returns>
        public double ReadDecimal()
        {
            ReadBytes(0x4, Endianness);
            return BitConverter.ToDouble(temp, 0);
        }
        /// <summary>
        ///Read Char value
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>Char</returns>
        public char ReadChar(Endian endian)
        {
            return (char)ReadInt16(endian);
        }
        /// <summary>
        /// Read Char Value
        /// </summary>
        /// <returns>Char</returns>
        public char ReadChar()
        {
            return ReadChar(this.type);
        }
        /// <summary>
        /// Read String value
        /// </summary>
        /// <param name="endian"></param>
        /// <returns>String</returns>
        public string ReadString(int count, bool unicode, Endian endian)
        {
            if (unicode) ReadBytes((count * 2), endian);
            else ReadBytes(count, endian);
            return unicode ? _unicode.GetString(temp) : _ascii.GetString(temp);
        }
        /// <summary>
        /// Read String Value
        /// </summary>
        /// <returns>String</returns>
        public string ReadString(int count, bool unicode)
        {
            return ReadString(count, unicode, this.type);
        }
        /// <summary>
        /// Attempts to interpret the string type
        /// Only use for debugging to figure out what type of string it is,
        /// not recommended to use this in an actual program unless
        /// they are restricted to characters from 0x00 -> 0xFF (special
        /// symbols will not work
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string ReadString(int count, Endian endian)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException();
            //this function will work if there are no characters that are above the hex of 88 33 or whatever
            if ((this.ReadUInt16(endian) >> 8) > 0)
            {
                this.Position -= 2;
                return ReadString(count, true, endian);
            }
            else
            {
                this.Position -= 2;
                return ReadString(count, false, endian);
            }
        }
        /// <summary>
        /// Reads a Zero Terminating String
        /// (An ASCII string that ends in a 
        /// null byte)
        /// </summary>
        /// <param name="endian"></param>
        /// <returns></returns>
        public string ReadZString(Endian endian)
        {
            List<byte> bytes = new List<byte>();
            byte i;
            while (this.Position < this.Length)
            {
                i = this.ReadByte();
                if (i == (byte)0)
                    break;
                else bytes.Add(i);
            }
            if (endian == Endian.Big) bytes.Reverse();
            return _ascii.GetString(bytes.ToArray());
        }
        /// <summary>
        /// This funciton will determine the integer
        /// type of the variable that you input, 
        /// and read that integer (does not work with
        /// strings) it can read a byte array
        /// as long as length is declared (byte[5] 
        /// will read a byte array of 5 bytes)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="endian"></param>
        public void Read(ref object obj, Endian endian)
        {
            TypeCode type = Type.GetTypeCode(obj.GetType());
            if (obj is byte[])
            {
                try
                {
                    obj = ReadBytes((obj as byte[]).Length, endian);
                    return;
                }
                catch (Exception ex) { throw ex; }
            }
            switch (type)
            {
                case TypeCode.Boolean:
                    obj = ReadBool();
                    return;
                case TypeCode.Byte:
                    obj = ReadByte();
                    return;
                case TypeCode.Char:
                    obj = ReadChar(endian);
                    return;
                case TypeCode.Decimal:
                    obj = ReadDecimal(endian);
                    return;
                case TypeCode.Int16:
                    obj = ReadInt16(endian);
                    return;
                case TypeCode.Int32:
                    obj = ReadInt32(endian);
                    return;
                case TypeCode.Int64:
                    obj = ReadInt64(endian);
                    return;
                case TypeCode.SByte:
                    obj = ReadSByte();
                    return;
                case TypeCode.Single:
                    obj = ReadFloat(endian);
                    return;
                case TypeCode.UInt16:
                    obj = ReadUInt16(endian);
                    return;
                case TypeCode.UInt32:
                    obj = ReadUInt32(endian);
                    return;
                case TypeCode.UInt64:
                    obj = ReadUInt64(endian);
                    return;
                default:
                    throw new InvalidDataException();
            }
        }
        /// <summary>
        /// This funciton will determine the integer
        /// type of the variable that you input, 
        /// and read that integer (does not work with
        /// strings) it can read a byte array
        /// as long as length is declared (byte[5] 
        /// will read a byte array of 5 bytes)
        /// </summary>
        public void Read(ref object obj)
        {
            TypeCode type = Type.GetTypeCode(obj.GetType());
            if (obj is byte[])
            {
                obj = ReadBytes((obj as byte[]).Length, this.type);
                return;
            }
            switch (type)
            {
                case TypeCode.Boolean:
                    obj = ReadBool();
                    return;
                case TypeCode.Byte:
                    obj = ReadByte();
                    return;
                case TypeCode.Char:
                    obj = ReadChar(this.type);
                    return;
                case TypeCode.Decimal:
                    obj = ReadDecimal(this.type);
                    return;
                case TypeCode.Int16:
                    obj = ReadInt16(this.type);
                    return;
                case TypeCode.Int32:
                    obj = ReadInt32(this.type);
                    return;
                case TypeCode.Int64:
                    obj = ReadInt64(this.type);
                    return;
                case TypeCode.SByte:
                    obj = ReadSByte();
                    return;
                case TypeCode.Single:
                    obj = ReadFloat(this.type);
                    return;
                case TypeCode.UInt16:
                    obj = ReadUInt16(this.type);
                    return;
                case TypeCode.UInt32:
                    obj = ReadUInt32(this.type);
                    return;
                case TypeCode.UInt64:
                    obj = ReadUInt64(this.type);
                    return;
                default:
                    throw new InvalidDataException();
            }
        }

        //Write
        /// <summary>
        /// Writes a unsigned 8 bit integer
        /// </summary>
        public void Write(byte[] i, Endian endian)
        {
            if (endian == Endian.Big)
                Array.Reverse(i);
            _out.Write(i);
            Seek(_out.BaseStream.Position);
        }
        /// <summary>
        /// Writes a unsigned 8 bit integer
        /// </summary>
        public void Write(byte[] i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a unsigned 8 bit integer
        /// </summary>
        public void Write(byte i)
        {
            _out.Write(i);
            Seek(_out.BaseStream.Position);
        }
        /// <summary>
        /// Writes a signed 16 bit integer
        /// </summary>
        public void Write(short i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a signed 16 bit integer
        /// </summary>
        public void Write(short i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a unsigned 16 bit integer
        /// </summary>
        public void Write(ushort i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a unsigned 16 bit integer
        /// </summary>
        public void Write(ushort i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a signed 32 bit integer
        /// </summary>
        public void Write(int i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a signed 32 bit integer
        /// </summary>
        public void Write(int i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a unsigned 32 bit integer
        /// </summary>
        public void Write(uint i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a unsigned 32 bit integer
        /// </summary>
        public void Write(uint i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a signed 64 bit integer
        /// </summary>
        public void Write(long i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a signed 64 bit integer
        /// </summary>
        public void Write(long i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a unsigned 64 bit integer
        /// </summary>
        public void Write(ulong i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a unsigned 64 bit integer
        /// </summary>
        public void Write(ulong i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a decimal value
        /// </summary>
        public void Write(double i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes decimal value
        /// </summary>
        public void Write(double i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a float value
        /// </summary>
        public void Write(float i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes float value
        /// </summary>
        public void Write(float i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a char value
        /// </summary>
        public void Write(char i, Endian endian)
        {
            temp = BitConverter.GetBytes(i);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes char value
        /// </summary>
        public void Write(char i)
        {
            this.Write(i, this.type);
        }
        /// <summary>
        /// Writes a string to the stream
        /// </summary>
        public void Write(string s, bool unicode, Endian endian)
        {
            temp = unicode ? _unicode.GetBytes(s) : _ascii.GetBytes(s);
            this.Write(temp, endian);
        }
        /// <summary>
        /// Writes a string to the stream
        /// </summary>
        public void Write(string s, bool unicode)
        {
            this.Write(s, unicode, this.type);
        }
        /// <summary>
        /// Writes a zero-teriminating string
        /// </summary>
        public void WriteZString(string s, Endian endian)
        {
            List<byte> bytes = new List<byte>(_ascii.GetBytes(s));
            bytes.Add(0x00);
            this.Write(bytes.ToArray(), endian);
            bytes = null;
        }
        /// <summary>
        /// Writes a zero-terminating string
        /// </summary>
        /// <param name="s"></param>
        public void WriteZString(string s)
        {
            this.WriteZString(s, this.type);
        }
        /// <summary>
        /// Writes an int 24
        /// </summary>
        public void WriteInt24(int i, Endian endian)
        {
            if ((i >> 24) > 0)
                throw new InvalidCastException("Int24 value is too large");
            else
            {
                List<byte> bytes = new List<byte>(BitConverter.GetBytes(i));
                bytes.RemoveAt(3);
                temp = bytes.ToArray();
                bytes = null;
                this.Write(temp, endian);
            }
        }
        /// <summary>
        /// Writes an int 24
        /// </summary>
        public void WriteInt24(int i)
        {
            if ((i >> 24) > 0)
                throw new InvalidCastException("Int24 value is too large");
            else
            {
                List<byte> bytes = new List<byte>(BitConverter.GetBytes(i));
                bytes.RemoveAt(3);
                temp = bytes.ToArray();
                bytes = null;
                this.Write(temp, this.type);
            }
        }

        //IDisposable
        ~IO()
        {
            this.Dispose(false);
        }
        protected void Dispose(bool DisposeManaged)
        {
            if (DisposeManaged)
            {
                _in.Dispose();
                _out.Dispose();
                _stream.Dispose();
            }
            _ascii = null;
            _unicode = null;
            temp = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        /// <summary>
        /// Disposes the function, removes all
        /// managed dependences from stack and
        /// empties garbage collection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
