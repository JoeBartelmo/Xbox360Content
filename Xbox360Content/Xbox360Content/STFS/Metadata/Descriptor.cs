/*  Copyright (C) 2012 Joseph Bartelmo

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

namespace Xbox360Content.STFS.Metadata
{
    /// <summary>
    /// Internal to prevent tampering, the descriptor
    /// is the most important set of bytes in the 
    /// file.
    /// </summary>
    internal class Descriptor
    {
        byte[] bytes;
        internal byte Reserved { get { return bytes[1]; } set { bytes[1] = value; } }
        internal byte BlockSeperation { get { return bytes[2]; } set { bytes[2] = value; } }
        internal short Blocks;
        int filetable;
        internal int FileTable
        {
            get
            {
                return filetable;
            }
            set
            {
                if (value > 0x800000)// 2^23, this is a signed int24
                    throw new ArgumentOutOfRangeException("Value must be 0 - 8388608");
                else
                    filetable = value;
            }
        }
        internal byte[] Hash;
        internal int Alloc, Unalloc;
        internal Descriptor(ref IO io)
        {
            if (io.Position != 0x379)
                io.Seek(0x379);
            bytes = io.ReadBytes(0x3, Endian.Little);
            Blocks = io.ReadInt16();
            filetable = io.ReadInt24();
            Hash = io.ReadBytes(0x4, Endian.Little);
            Alloc = io.ReadInt32();
            Unalloc = io.ReadInt32();
        }
        internal Descriptor()
        {
            bytes = new byte[0x24];
            bytes[0] = 0x24;
        }

        public static implicit operator byte[](Descriptor desc)
        {
            List<byte> bytes = new List<byte>(desc.bytes);
            bytes.AddRange(BitConverter.GetBytes(desc.Blocks));
            //bytes.AddRange(BitConverter.GetBytes(
            return null;
        }
    }
}
