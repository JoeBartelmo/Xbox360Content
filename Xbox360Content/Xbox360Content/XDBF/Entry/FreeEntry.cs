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
using Xbox360Content.XDBF.Enums;

namespace Xbox360Content.XDBF
{
    /// <summary>
    /// Structure for the Entry table in XBDF file
    /// </summary>
    public class FreeEntry
    {
        bool bigEndian;
        internal uint length, specifier;

        public uint Offset { get { return specifier; } }
        public uint Length { get { return length; } }
        
        public FreeEntry(ref IO io)
        {
            bigEndian = io.Endianness == Endian.Big;
            specifier = io.ReadUInt32();
            length = io.ReadUInt32();
        }
        public static explicit operator byte[](FreeEntry fe)
        {
            List<byte> bytes = new List<byte>(fe.specifier.ToBytes(fe.bigEndian));
            bytes.AddRange(fe.length.ToBytes(fe.bigEndian));
            return bytes.ToArray();
        }
    }
}