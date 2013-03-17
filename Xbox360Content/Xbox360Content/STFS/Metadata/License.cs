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

//Please note that this class is not a copy of the actual 
//copyright license, if you would like the copyright license
//go to the folder entitled: "License and About"

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360Content.STFS.Metadata
{
    /// <summary>
    /// Usually these are all null, except for the 
    /// few that don't like to be like everyone else
    /// </summary>
    public struct License
    {
        /// <summary>
        /// XUID, or PUID
        /// </summary>
        public long ID;
        /// <summary>
        /// Data stored for use by console.
        /// </summary>
        public int Information;//Didn't take the time to find out what each flag does, or what info stands for
        /// <summary>
        /// Pointers for the licenses
        /// </summary>
        public int Flags;

        public License(long id, int info, int flags)
        {
            ID = id; Information = info; Flags = flags;
        }
        public static License[] LoadLicenses(ref IO io)
        {
            List<License> licenses = new List<License>();
            if (io.Position != 0x22c)
                io.Seek(0x22c);
            while (io.Position < 0x32c)
                licenses.Add(new License(io.ReadInt64(), io.ReadInt32(), io.ReadInt32()));
            return licenses.ToArray();
        }
        /// <summary>
        /// Converts license to bytes (duh)
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        public static implicit operator byte[](License li)
        {
            List<byte> bytes = new List<byte>(BitConverter.GetBytes(li.ID.SwapEndian()));
            bytes.AddRange(BitConverter.GetBytes(li.Information.SwapEndian()));
            bytes.AddRange(BitConverter.GetBytes(li.Flags.SwapEndian()));
            return bytes.ToArray();
        }
    }
}
