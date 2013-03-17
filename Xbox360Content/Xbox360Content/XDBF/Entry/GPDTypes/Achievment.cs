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
        public enum AchievementTypes : byte
        {
            Completion = 1,
            Leveling = 2,
            Unlock = 3,
            Event = 4,
            Tournament = 5,
            Checkpoint = 6,
            Other = 7
        }
    }
    /// <summary>
    /// Structure for the Entry table in XBDF file
    /// </summary>
    public struct Achievement
    {
        private bool endian;
        private uint[] data;
        private int score;
        private string[] sdata;
        private long unlock;

        public DateTime UnlockTime { get { return DateTime.FromFileTime(unlock); } set { unlock = value.ToFileTime(); } }
        public uint StructSize { get { return data[0]; } }
        public uint AchievementID { get { return data[1]; } }
        public uint ImageID { get { return data[2]; } }
        public int Gamerscore { get { return score; } }
        private uint Flags { get { return data[3]; } set{data[3] = value;} }

        public string Name { get { return sdata[0]; } }
        public string UnlockedDescription { get { return sdata[1]; } set { sdata[1] = value; } }
        public string LockedDescription { get { return sdata[2]; } set { sdata[2] = value; } }

        public AchievementTypes AchievementType
        {
            get
            {
                return (AchievementTypes)(Flags & 7);
            }
            set
            {
                if ((byte)value > 7)
                    throw new XDBFException("Bits must be between 0 and 7");
                Flags &= 0xFFFFFFF8;//clear 3 bits for use with algo
                Flags ^= (byte)value;
            }
        }
        public bool ShowUnachieved
        {
            get
            {
                return (Flags & 8) == 8;
            }
            set
            {
                if (value != ShowUnachieved)
                    Flags ^= 8;
            }
        }
        /// <summary>
        /// Sets whether or not achievement was earned online or offline (bit 16)
        /// </summary>
        public bool AchievementEarnedOnline
        {
            get
            {
                return (Flags & 0x10000) == 0x10000;
            }
            set
            {
                if (value != AchievementEarnedOnline)
                    Flags ^= 0x10000;
            }
        }
        /// <summary>
        /// Sets whether or not the achievement was earned (bit 17)
        /// </summary>
        public bool AchievementEarned
        {
            get
            {
                return (Flags & 0x20000) == 0x20000;//bit 17
            }
            set
            {
                if (value != AchievementEarned)
                    Flags ^= 131072;
            }
        }
        /// <summary>
        /// Sets whether or not the Achievement has been edited (bit 20)
        /// </summary>
        public bool Edited
        {
            get
            {
                return (Flags & 0x100000) == 0x100000;//bit 20
            }
            set
            {
                if (value != Edited)
                    Flags ^= 0x100000;
            }
        }

        public Achievement(ref IO io)
        {
            endian = io.Endianness == Endian.Big;
            data = new uint[4];
            sdata = new string[3];
            for (int i = 0; i < 3; i++)
                data[i] = io.ReadUInt32();
            score = io.ReadInt32();
            data[3] = io.ReadUInt32();
            unlock = io.ReadInt64();
            for (int i = 0; i < sdata.Length; i++)
                sdata[i] = io.ReadZString(true);
        }

        public static explicit operator byte[](Achievement cheevo)
        {
            List<byte> cheev = new List<byte>(cheevo.data[0].ToBytes(cheevo.endian));
            cheev.AddRange(cheevo.data[1].ToBytes(cheevo.endian));
            cheev.AddRange(cheevo.data[2].ToBytes(cheevo.endian));
            cheev.AddRange(cheevo.score.ToBytes(cheevo.endian));
            cheev.AddRange(cheevo.data[3].ToBytes(cheevo.endian));
            cheev.AddRange(cheevo.sdata[0].ToBytes(cheevo.endian));
            cheev.AddRange(new byte[2] { 0, 0 });
            cheev.AddRange(cheevo.sdata[1].ToBytes(cheevo.endian));
            cheev.AddRange(new byte[2] { 0, 0 });
            cheev.AddRange(cheevo.sdata[2].ToBytes(cheevo.endian));
            cheev.AddRange(new byte[2] { 0, 0 });
            return cheev.ToArray();
        }
    }
}
