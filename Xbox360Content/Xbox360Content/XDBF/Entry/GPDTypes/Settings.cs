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
                        return null;
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

        int? optional;
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
                        optional = ((string)value).Length;
                    if (DataInterop == typeof(byte[]))
                        optional = ((byte[])value).Length;
                    val = value;
                }
                else
                    throw new XDBFException("Invalid set data");
            }
        }

        public Setting(ref IO io)
        {
            optional = null;
            bigEndian = io.Endianness == Endian.Big;
            SettingID = io.ReadInt32();
            lastEdited = io.ReadUInt16();
            Unknown = io.ReadInt16();
            dataType = io.ReadByte();
            nulls = io.ReadBytes(7);
            switch(dataType)
            {
                case 0:
                    val = null;
                    break;
                case 1:
                    val = io.ReadInt32();
                    break;
                case 2:
                    val = io.ReadInt64();
                    break;
                case 3:
                    val = io.ReadDouble();
                    break;
                case 4:
                    optional = io.ReadInt32();
                    io.ReadInt32();
                    val = io.ReadString(optional.GetValueOrDefault(), true);
                    break;
                case 5:
                    val = io.ReadFloat();
                    break;
                case 6:
                    optional = io.ReadInt32();
                    io.ReadInt32();
                    val = io.ReadBytes(optional.GetValueOrDefault());
                    break;
                case 7:
                    val = DateTime.FromFileTime(io.ReadInt64());
                    break;
                case 255:
                    val = null;
                    break;
                default: throw new XDBFException("Could not determine data Type in setting");
            }
        }

        public static explicit operator byte[](Setting s)
        {
            List<byte> bytes = new List<byte>(s.SettingID.ToBytes(s.bigEndian));
            bytes.AddRange(s.lastEdited.ToBytes(s.bigEndian));
            bytes.AddRange(s.Unknown.ToBytes(s.bigEndian));
            bytes.Add(s.dataType);
            bytes.AddRange(s.nulls);
            if (s.optional != null)
            {
                bytes.AddRange(s.optional.GetValueOrDefault().ToBytes(s.bigEndian));
                bytes.AddRange(0.ToBytes(false));
                if (s.DataType == SettingTypes.UnicodeString)
                    bytes.AddRange(((string)s.val).ToBytes(true));
                else
                    bytes.AddRange((byte[])s.val);
            }
            switch (s.dataType)
            {
                case 0:
                case 0xFF:
                    break;
                case 1:
                    bytes.AddRange(((int)s.val).ToBytes(s.bigEndian));
                    break;
                case 2:
                    bytes.AddRange(((long)s.val).ToBytes(s.bigEndian));
                    break;
                case 3:
                    bytes.AddRange(((double)s.val).ToBytes(s.bigEndian));
                    break;
                case 4:
                    bytes.AddRange(s.optional.GetValueOrDefault().ToBytes(s.bigEndian));
                    bytes.AddRange(0.ToBytes(false));
                    bytes.AddRange(((string)s.val).ToBytes(true));
                    break;
                case 5:
                    bytes.AddRange(((float)s.val).ToBytes(s.bigEndian));
                    break;
                case 6:
                    bytes.AddRange(s.optional.GetValueOrDefault().ToBytes(s.bigEndian));
                    bytes.AddRange(0.ToBytes(false));
                    bytes.AddRange((byte[])s.val);
                    break;
                case 7:
                    bytes.AddRange(((DateTime)s.val).ToFileTime().ToBytes(s.bigEndian));
                    break;
                default: throw new XDBFException("Could not determine data Type in setting");
            }
            return bytes.ToArray();
        }
    }
}
