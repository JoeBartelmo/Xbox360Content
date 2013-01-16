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
    internal class Header
    {
        License[] li;
        /// <summary>
        /// I don't recommend you touch these.
        /// </summary>
        internal License[] Licenses
        {
            get
            {
                return li;
            }
            set
            {
                if (value.Length != 10)
                    throw new IndexOutOfRangeException();
                else
                    li = value;
            }
        }
        internal byte[] hash;
        uint headerSize;
        int type, version;

        /// <summary>
        /// Userfriendly viewing of the type of content
        /// </summary>
        internal string ContentType
        {
            get
            {
                switch (type)
                {
                    case 0xD0000:	return "Arcade Title";
                    case 0x9000:	return "Avatar Item";
                    case 0x40000:	return "Cache File";
                    case 0x2000000:	return "Community Game";
                    case 0x80000:   return "Game Demo";
                    case 0x20000:	return "Gamer Picture";
                    case 0xA0000:	return "Game Title";
                    case 0xC0000:	return "Game Trailer";
                    case 0x400000:	return "Game Video";
                    case 0x4000:	return "Installed Game";
                    case 0xB0000:	return "Installer";
                    case 0x2000:	return "IPTV Pause Buffer";
                    case 0xF0000:	return "License Store";
                    case 0x2:	    return "Marketplace Content";
                    case 0x100000:	return "Movie";
                    case 0x300000:  return "Music Video";
                    case 0x500000:	return "Podcast Video";
                    case 0x10000:	return "Profile";
                    case 0x3:	    return "Publisher";
                    case 0x1:       return "Saved Game";
                    case 0x50000:   return "Storage Download";
                    case 0x30000:   return "Theme";
                    case 0x200000:  return "TV";
                    case 0x90000:   return "Video";
                    case 0x600000:	return "Viral Video";
                    case 0x70000:	return "Xbox Download";
                    case 0x60000:	return "Xbox Saved Game";
                    case 0x1000:	return "Xbox 360 Title";
                    case 0x5000:	return "Xbox Title";
                    case 0xE0000:	return "XNA";
                    default:        return null;
                }
            }
        }
        /// <summary>
        /// Identifies the Metadata Version
        /// </summary>
        internal bool Version2 { get { return (version == 2); } }

        public Header(ref IO io)
        {
            if (io.Position != 0x22c)
                io.Seek(0x22c);
            li = License.LoadLicenses(ref io);
            hash = io.ReadBytes(10, Endian.Little);
            headerSize = io.ReadUInt32();
            type = io.ReadInt32();
            version = io.ReadInt32();
        }
    }
}
