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
        public enum TitleFlags : byte
        {
            UnlockedOffline = 1,
            UnlockedNeedstoSync = 2,
            AwardUnlockedNeedsDownload = 0x10
        }
    }
    /// <summary>
    /// Structure for the Entry table in XBDF file
    /// </summary>
    public struct Title
    {
        bool bigEndian;
        uint titleID;
        int[] gscore;
        byte[] avatar;
        uint flags;
        long playtime;
        string name;

        public uint TitleID { get { return titleID; } }
        public int Achievements { get { return gscore[0]; } set { gscore[0] = value; } }
        public int AchievementsUnlocked { get { return gscore[1]; } set { gscore[1] = value; } }
        public int GamerScore { get { return gscore[2]; } set { gscore[2] = value; } }
        public int GamerScoreUnlocked { get { return gscore[3]; } set { gscore[3] = value; } }

        private byte Unknown { get { return avatar[0]; } }

        public byte AchievementUnlockedOnlineCount 
        { get { return avatar[1]; } set { avatar[1] = value; } }
        public byte AvatarAssetsEarned 
        { get { return avatar[2]; } set { avatar[2] = value; } }
        public byte AvatarAssetsMax 
        { get { return avatar[3]; } set { avatar[3] = value; } }
        public byte MaleAvatarAssetsEarned 
        { get { return avatar[4]; } set { avatar[4] = value; } }
        public byte MaleAvatarAssetsMax 
        { get { return avatar[5]; } set { avatar[5] = value; } }
        public byte FemaleAvatarAssetsEarned 
        { get { return avatar[6]; } set { avatar[6] = value; } }
        public byte FemaleAvatarAssetsMax 
        { get { return avatar[7]; } set { avatar[7] = value; } }

        public DateTime LastPlayedTime
        {
            get { return DateTime.FromFileTime(playtime); }
            set { playtime = value.ToFileTime(); }
        }

        public Title(ref IO io)
        {
            bigEndian = io.Endianness == Endian.Big;
            titleID = io.ReadUInt32();
            gscore = new int[4];
            for (int i = 0; i < 4; i++)
                gscore[i] = io.ReadInt32();
            avatar = io.ReadBytes(8);
            flags = io.ReadUInt32();
            playtime = io.ReadInt64();
            name = io.ReadZString(true);
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
