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
using Xbox360Content.XDBF.GPD.Enums;

namespace Xbox360Content.XDBF.GPD
{
    namespace Enums
    {
        public enum SettingTypes : byte
        {
            Context = 0,
            Int32 = 1,
            Int64 = 2,
            Double = 3,
            UnicodeString = 4,
            Float = 5,
            Binary = 6,
            DateTime = 7,
            Null = 255
        }
    }
    /// <summary>
    /// Structure for the Entry table in XBDF file
    /// </summary>
    public struct Setting
    {
        public Entry Entry;
        private bool bigEndian;

        internal int SettingID;
        private ushort lastEdited;
        private short Unknown;
        private byte dataType;
        private byte[] nulls;

        public DateTime LastEdited
        {
            get
            {
                int year = lastEdited >> 9,
                    month = (lastEdited >> 5) & 15,
                    day = lastEdited & 31;
                return DateTime.ParseExact(string.Format("{0}/{1}/{2}", month, day, year), "M/d/yyyy", new System.Globalization.CultureInfo("en-US"));
            }
            set
            {
                unchecked
                {
                    uint i = 0;
                    i ^= (uint)value.Day;
                    i ^= (uint)(value.Month << 5);
                    i ^= (uint)(value.Year << 9);
                    lastEdited = (ushort)i;
                }
            }
        }

        public SettingTypes DataType
        {
            get
            {
                if((dataType != 0xFF) && (dataType > 7))
                    throw new XDBFException("Invalid Setting Flag");
                return (SettingTypes)dataType;
            }
        }
        public Type DataInterop
        {
            get
            {
                switch (dataType)
                {
                    case 0:
                    case 1:
                        return typeof(int);
                    case 2:
                        return typeof(long);
                    case 3:
                        return typeof(double);
                    case 4:
                        return typeof(string);
                    case 5:
                        return typeof(float);
                    case 6:
                        return typeof(byte[]);
                    case 7:
                        return typeof(DateTime);
                    case 255:
                        return null;
                    default: throw new XDBFException("Could not Identify Type of Setting");
                }
            }
        }

        object val;
        public object Data
        {
            get { return val; }
            set
            {
                if (val == null)
                    return;
                else if (value.GetType() == DataInterop)
                {
                    if (DataInterop == typeof(string))
                    {
                        this.Entry.length -= (uint)((((string)val).Length * 2));
                        this.Entry.length += (uint)((((string)value).Length * 2));

                    }
                    if (DataInterop == typeof(byte[]))
                    {
                        this.Entry.length -= (uint)(((byte[])value).Length);
                        this.Entry.length += (uint)(((byte[])value).Length);
                    }
                    val = value;
                }
                else
                    throw new XDBFException("Invalid set data");
            }
        }

        public Setting(Entry e, ref IO io)
        {
            this.Entry = e;
            bigEndian = io.Endianness == Endian.Big;
            

            SettingID = io.ReadInt32();
            lastEdited = io.ReadUInt16();
            Unknown = io.ReadInt16();
            dataType = io.ReadByte();
            nulls = io.ReadBytes(7);

            switch(dataType)
            {
                case 0:
                case 1:
                    val = io.ReadInt32();
                    break;
                case 7:
                case 2:
                    val = io.ReadInt64();
                    break;
                case 3:
                    val = io.ReadDouble();
                    break;
                case 4:
                    int x = io.ReadInt24();
                    io.ReadBytes(5);
                    val = io.ReadBytes(x).GetString(true);
                    //Console.WriteLine(x.ToBytes(true).ToHex());
                    break;
                case 5:
                    val = io.ReadFloat();
                    break;
                case 6:
                    int y = io.ReadInt24();
                    io.ReadBytes(5);
                    val = io.ReadBytes(y);
                    //Console.WriteLine(y);
                    break;
                case 255:
                    val = null;
                    break;
                default:
                    Console.WriteLine("Er: 0x{0:X2}", io.Position - 16);
                    throw new XDBFException("Could not determine data Type in setting");
            }
            //Console.WriteLine("{2} ~> {1:X2} -> {0:X2}", ((byte[])this).Length, e.length, this.DataType);
            //testing
            //((byte[])this).MakeFile("namespaces\\" + e._nameID.ToString("x2"), true);
        }

        public static explicit operator byte[](Setting s)
        {
            List<byte> bytes = new List<byte>(s.SettingID.ToBytes(s.bigEndian));
            bytes.AddRange(s.lastEdited.ToBytes(s.bigEndian));
            bytes.AddRange(s.Unknown.ToBytes(s.bigEndian));
            bytes.Add(s.dataType);
            bytes.AddRange(s.nulls);
            byte[] b = new byte[4];
            switch (s.dataType)
            {
                case 0xFF:
                    break;
                case 0:
                case 1:
                    bytes.AddRange(((int)s.val).ToBytes(s.bigEndian));
                    bytes.AddRange(b);
                    break;
                case 7:
                case 2:
                    bytes.AddRange(((long)s.val).ToBytes(s.bigEndian));
                    break;
                case 3:
                    bytes.AddRange(((double)s.val).ToBytes(s.bigEndian));
                    break;
                case 4:
                    bytes.AddRange((((((string)s.val).Length) * 2)).ToBytes(s.bigEndian));
                    bytes.AddRange(b);
                    bytes.AddRange(((string)s.val).ToBytes(true, s.bigEndian));
                    break;
                case 5:
                    bytes.AddRange(((float)s.val).ToBytes(s.bigEndian));
                    bytes.AddRange(b);
                    break;
                case 6:
                    bytes.AddRange((((byte[])s.val).Length).ToBytes(s.bigEndian));
                    bytes.AddRange(b);
                    bytes.AddRange((byte[])s.val);
                    break;
                default: throw new XDBFException("Could not determine data Type in setting");
            }
            return bytes.ToArray();
        }
    }
}
