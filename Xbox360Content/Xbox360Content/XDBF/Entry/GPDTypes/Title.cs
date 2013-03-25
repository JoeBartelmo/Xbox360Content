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
        public enum ImageType
        {
            Icon,
            LargeBoxArt,
            SmallBoxArt,
            Banner,
            MarketPlace
        }
    }
    /// <summary>
    /// Structure for the Entry table in XBDF file
    /// </summary>
    public struct Title
    {
        public Entry Entry;
        bool bigEndian;
        uint titleID;
        int[] gscore;
        byte[] avatar;
        byte[] flags;
        long playtime;

        /// <summary>
        /// Downloads Image from the internet
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public System.Drawing.Image GetImage(ImageType type)
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                switch (type)
                {
                    case ImageType.Banner:
                        if (System.IO.File.Exists(About.Directory + "banner_t" + titleID.ToString("x2")))
                            return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "banner_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                        wc.DownloadData("http://avatar.xboxlive.com/global/t." + titleID.ToString("x8") + "/marketplace/0/1").MakeFile(About.Directory + "banner_t" + titleID.ToString("x2"), false);
                        return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "banner_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                    case ImageType.Icon:
                        if (System.IO.File.Exists(About.Directory + "icon_t" + titleID.ToString("x2")))
                            return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "icon_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                        wc.DownloadData("http://image.xboxlive.com/global/t." + titleID.ToString("x8") + "/icon/0/8000").MakeFile(About.Directory + "icon_t" + titleID.ToString("x2"), false);
                        return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "icon_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                    case ImageType.LargeBoxArt:
                        if (System.IO.File.Exists(About.Directory + "large_t" + titleID.ToString("x2")))
                            return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "large_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                        wc.DownloadData("http://tiles.xbox.com/consoleAssets/" + titleID.ToString("x8") + "/en-GB/largeboxart.jpg").MakeFile(About.Directory + "large_t" + titleID.ToString("x2"), false);
                        return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "large_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                    case ImageType.MarketPlace:
                        if (System.IO.File.Exists(About.Directory + "market_t" + titleID.ToString("x2")))
                            return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "market_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                        wc.DownloadData("http://marketplace.xbox.com/en-US/Title/" + titleID).MakeFile(About.Directory + "market_t" + titleID.ToString("x2"), false);
                        return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "market_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                    case ImageType.SmallBoxArt:
                        if (System.IO.File.Exists(About.Directory + "small_t" + titleID.ToString("x2")))
                            return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "small_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                        wc.DownloadData("http://tiles.xbox.com/consoleAssets/" + titleID.ToString("x8") + "/en-GB/smallboxart.jpg").MakeFile(About.Directory + "small_t" + titleID.ToString("x2"), false);
                        return System.Drawing.Image.FromStream(new System.IO.FileStream(About.Directory + "small_t" + titleID.ToString("x2"), System.IO.FileMode.Open));
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

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

        public Title(Entry e, ref IO io)
        {
            this.Entry = e;
            bigEndian = io.Endianness == Endian.Big;
            titleID = io.ReadUInt32();
            gscore = new int[4];
            for (int i = 0; i < 4; i++)
                gscore[i] = io.ReadInt32();
            avatar = io.ReadBytes(8);
            flags = io.ReadBytes(4);
            playtime = io.ReadInt64();
            name = io.ReadZString(true);
        }
        string name;
        public string Name { get { return name; }
            set
            {
                this.Entry.length -= (uint)((name.Length + 1) * 2);
                this.Entry.length += (uint)((value.Length + 1) * 2);
                name = value;
            } 
        }

        public static explicit operator byte[](Title t)
        {
            List<byte> bytes = new List<byte>(t.TitleID.ToBytes(t.bigEndian));
            for (int i = 0; i < t.gscore.Length; i++)
                bytes.AddRange(t.gscore[i].ToBytes(t.bigEndian));
            bytes.AddRange(t.avatar);
            bytes.AddRange(t.flags);
            bytes.AddRange(t.playtime.ToBytes(t.bigEndian));
            bytes.AddRange(t.Name.ToBytes(true, t.bigEndian));
            bytes.AddRange(new byte[2] { 0, 0 });
            return bytes.ToArray();
        }
    }
}
