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

namespace Xbox360Content.XDBF.GPD
{
    public class GPD : XDBF
    {
        /* Master Schema:
         * Header
         * 
         * Titles
         * Assets
         * Achievements
         * Settings
         * Images
         * Strings
         * Lists
         * SyncData
         * Freespace
         */

        #region Comparisons
        private static Comparison<FreeEntry> _free = new Comparison<FreeEntry>(delegate(FreeEntry fe1, FreeEntry fe2) { return fe1.specifier.CompareTo(fe2.specifier); });
        private static Comparison<Achievement> _achieve = new Comparison<Achievement>(delegate(Achievement fe1, Achievement fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); });
        private static Comparison<Setting> _sett = (new Comparison<Setting>(delegate(Setting fe1, Setting fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<Xbox360Content.XDBF.GPD.String> _str = (new Comparison<Xbox360Content.XDBF.GPD.String>(delegate(Xbox360Content.XDBF.GPD.String fe1, Xbox360Content.XDBF.GPD.String fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<Image> _img = (new Comparison<Image>(delegate(Image fe1, Image fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<SyncData> _data = (new Comparison<SyncData>(delegate(SyncData fe1, SyncData fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<SyncList> _list = (new Comparison<SyncList>(delegate(SyncList fe1, SyncList fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<Title> _title = (new Comparison<Title>(delegate(Title fe1, Title fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        private static Comparison<Avatar> _avatar = (new Comparison<Avatar>(delegate(Avatar fe1, Avatar fe2) { return fe1.Entry.specifier.CompareTo(fe2.Entry.specifier); }));
        #endregion

        #region Variable/Accessors
        public static GPDID GetDataType(Entry e)
        {
            return (GPDID)e._nameID;
        }
        public static GPDData GetIdType(Entry e)
        {
            return (GPDData)e.id;
        }

        public List<Achievement> Achievements = new List<Achievement>();
        public List<Setting> Settings = new List<Setting>();
        public List<Xbox360Content.XDBF.GPD.String> Strings = new List<String>();
        public List<Image> Images = new List<Image>();
        public List<SyncData> SyncData = new List<SyncData>();
        public List<SyncList> Lists = new List<SyncList>();
        public List<Title> Titles = new List<Title>();
        public List<Avatar> Assets = new List<Avatar>();
        public uint Length { get { return 0x18 + 
            (uint)((0x200 + ((
            Titles.Count + 
            Achievements.Count + 
            Settings.Count + 
            Strings.Count +  //so it would fit on my screen ^.^
            Images.Count + 
            Lists.Count + 
            SyncData.Count +
            Assets.Count) / 0x200) * 0x12) + 
            (uint)( (0x200 + 
            (freeentries.Count / 0x200)) * 0x8) + 
            pushOffsets());} }
        #endregion

        public GPD(IO io) : base(io) { readGPD(); }

        int getSyncList(ushort nameID)
        {
            for (int i = 0; i < Lists.Count; i++)
                if (Lists[i].Entry._nameID == nameID)
                    return i;
            return -1;
        }
        int getSyncData(ushort nameID)
        {
            for (int i = 0; i < SyncData.Count; i++)
                if (SyncData[i].Entry._nameID == nameID)
                    return i;
            return -1;
        }
        void sort()
        {
            //Sort
            freeentries.Sort(_free);
            for (int i = 1; i < freeentries.Count; i++)//Aranging Free Entries so that they are ajacent to one another
            {
                if (!((freeentries[i - 1].Offset + freeentries[i - 1].Length)
                    < freeentries[i].Offset)
                    && i != 0)
                    continue;
                freeentries[i - 1].length =
                    ((freeentries[i].Offset + freeentries[i].Length) -
                    freeentries[i - 1].Offset);
                freeentries.RemoveAt(i--);
            }
            Achievements.Sort(_achieve);
            Settings.Sort(_sett);
            Strings.Sort(_str);
            Images.Sort(_img);
            SyncData.Sort(_data);
            Lists.Sort(_list);
            Titles.Sort(_title);
            Assets.Sort(_avatar);
        }
        uint pushOffsets()
        {
            /* Master Schema:
             * Header
             * 
             * Titles
             * Assets
             * Achievements g
             * Settings 
             * Images g
             * Strings g
             * Lists g
             * SyncData g
             * Freespace
             */
            uint length = 0;
            sort();
            for (int i = 0; i < Titles.Count; i++)
            { Titles[i].Entry.specifier = length; length += Titles[i].Entry.Length; }

            for (int i = 0; i < Assets.Count; i++)
            { Assets[i].Entry.specifier = length; length += Assets[i].Entry.Length; }

            for (int i = 0; i < Achievements.Count; i++)
            { Achievements[i].Entry.specifier = length; length += Achievements[i].Entry.Length; }

            for (int i = 0; i < Settings.Count; i++)
            { Settings[i].Entry.specifier = length; length += Settings[i].Entry.Length; }

            for (int i = 0; i < Images.Count; i++)
            { Images[i].Entry.specifier = length; length += Images[i].Entry.Length; }

            for (int i = 0; i < Strings.Count; i++)
            { Strings[i].Entry.specifier = length; length += Strings[i].Entry.Length; }

            for (int i = 0; i < Lists.Count; i++)
            { Lists[i].Entry.specifier = length; length += Lists[i].Entry.Length; }

            for (int i = 0; i < SyncData.Count; i++)
            { SyncData[i].Entry.specifier = length; length += SyncData[i].Entry.Length; }

            for (int i = 0; i < freeentries.Count; i++)
            { freeentries[i].specifier = length; length += freeentries[i].Length; }

            foreach (var list in Lists.ToArray())
                list.SetLength();
            return length;
        }

        /// <summary>
        /// Updates the sync table
        /// </summary>
        /// <param name="updateServer">If set false, then it will update local, if you do not want to update local, or server, keep blank</param>
        /// <param name="xNS"></param>
        /// <param name="ID"></param>
        void updateEntrySync(bool updateServer, Entry s)
        {
            GPDID gpdID = (GPDID)s._nameID;

            if (gpdID == GPDID.AvatarAward || 
                gpdID == GPDID.Image || 
                gpdID == GPDID.String)
                return;
            
            if (getSyncList((ushort)gpdID) == -1)//If there is no sync list for this entry
            {
                var list = new SyncList();
                list.Entry = new Entry(xIO.Endianness == Endian.Big)
                {
                    _nameID = (ushort)gpdID,
                    length = (gpdID == GPDID.Achievement) ? (uint)(0x10 * Achievements.Count) : 0x10u,
                    specifier = 0,//will set in pushOffsets()
                    id = 0x100000000L
                };
                if (gpdID == GPDID.Achievement)
                {
                    for (int i = 0; i < Achievements.Count; i++)
                    {
                        var item = new SyncItem() { EntryID = Achievements[i].AchievementID, SyncID = i + 1 };
                        list.items.Add(item);
                    }
                }
                Lists.Add(list);
                pushOffsets();
            }
            if (getSyncData((ushort)gpdID) == -1)
            {
                var data = new Xbox360Content.XDBF.GPD.SyncData();
                data.Entry = new Entry(xIO.Endianness == Endian.Big)
                {
                    _nameID = (ushort)gpdID,
                    id = 0x200000000L,
                    length = 0x18,
                    specifier = 0//will set in pushOffsets()
                };

                if (gpdID == GPDID.Achievement)
                {
                    data.LastSyncID = 0;
                    data.NextSyncID = (ulong)(Achievements.Count + 1);
                }
                SyncData.Add(data);
                pushOffsets();
            }

            int x = getSyncList((ushort)gpdID);
            ulong len = SyncData[getSyncData((ushort)gpdID)].NextSyncID;
            for (int i = 0; i < Lists[x].items.Count; i++)
                if ((ulong)Lists[x].items[i].EntryID == s.id)
                    Lists[x].items.RemoveAt(i--);
            unchecked
            {
                ///
                /// This section was a pain, but i wanted to stick with the struct rather than a class
                ///
                if (updateServer)
                {
                    var itm = new SyncItem() { EntryID = (long)s.id, SyncID = (long)len++ };
                    Lists[getSyncList((ushort)gpdID)].items.Add(itm);
                }
                else
                {
                    var itm = new SyncItem() { EntryID = (long)s.id, SyncID = 0 };
                    Lists[x].items.Add(itm);
                }
            }
            SyncData sd = SyncData[x];
            sd.NextSyncID = (ulong)len;
            SyncData[x] = sd;
            this.pushOffsets();
            this.Update();
        }
        
        public void Update()
        {
            Log.Write("Updating Stream...");
            xIO.ReplaceStream((byte[])this);
        }
        public void SaveAs(string file, bool useDesktop)
        {
            ((byte[])this).MakeFile(file, useDesktop);
        }
        void readGPD()
        {
            Achievements.Clear();
            Settings.Clear();
            Strings.Clear();
            Images.Clear();
            SyncData.Clear();
            Lists.Clear();
            Assets.Clear();

            Log.Write("Loading Contents in file...");
            //try
            //{
                foreach (Entry e in Entries.ToArray())
                {
                    xIO.Position = GetOffset(e.Offset);

                    if (e.id == 0x100000000 || e.id == 1)//list
                    {
                        List<SyncItem> items = new List<SyncItem>();
                        for (int i = 0; i < e.length / 16; i++)
                            items.Add(new SyncItem(ref xIO));
                        Lists.Add(new SyncList(e, items));
                    }
                    else if (e.id == 0x200000000 || e.id == 2)//data
                        SyncData.Add(new SyncData(e, ref xIO));
                    else switch (GetDataType(e))
                        {
                            case GPDID.Achievement:
                                Achievements.Add(new Achievement(e, ref xIO));
                                break;
                            case GPDID.AvatarAward:
                                Assets.Add(new Avatar(ref xIO, e));
                                break;
                            case GPDID.Image:
                                Images.Add(new Image(e, ref xIO));
                                break;
                            case GPDID.Setting:
                                Settings.Add(new Setting(e, ref xIO));
                                break;
                            case GPDID.String:
                                Strings.Add(new String(e, ref xIO));
                                break;
                            case GPDID.Title:
                                Titles.Add(new Title(e, ref xIO));
                                break;
                        }
                    foreach (var s in Settings.ToArray())
                        ((byte[])s).MakeFile("C:\\users\\joe\\desktop\\settings\\" + DateTime.Now.AddMilliseconds(1d).Millisecond.ToString(), false);
                }
                Log.Write(string.Format("Achievements:{0}\n\t   Images:{1}\n\t   Settings:{2}\n\t   Strings:{3}\n\t   Titles:{4}\n\t   Synclists:{5}\n\t   SyncDatas:{6}\n\t   Entries:{7}\n\t   Assets:{8}\n\t   FreeEntries:{9}", Achievements.Count, Images.Count, Settings.Count, Strings.Count, Titles.Count, Lists.Count, SyncData.Count, Entries.Length, Assets.Count, FreeEntries.Length));
           // }
            //catch (Exception ex)
            //{
                /* Master Schema:
                 * Header
                 * 
                 * Titles
                 * Assets
                 * Achievements
                 * Settings
                 * Images
                 * Strings
                 * Lists
                 * SyncData
                 * Freespace
                 */
              //  Log.Write(string.Format("Achievements:{0}\n\t   Images:{1}\n\t   Settings:{2}\n\t   Strings:{3}\n\t   Titles:{4}\n\t   Synclists:{5}\n\t   SyncDatas:{6}\n\t   Entries:{7}\n\t   Assets:{8}\n\t   FreeEntries:{9}", Achievements.Count, Images.Count, Settings.Count, Strings.Count, Titles.Count, Lists.Count, SyncData.Count, Entries.Length, Assets.Count, FreeEntries.Length));
          
             //   foreach (var s in Settings.ToArray())
             //       ((byte[])s).MakeFile("C:\\users\\joe\\desktop\\stoopidsettings\\" + DateTime.Now.AddMilliseconds(1d).Millisecond.ToString(), false);
             //   Log.Write(string.Format("Exception occured reading @ 0x{0:x2}", xIO.Position), 2); xIO.Dispose(); throw ex; 
            //}
            pushOffsets();
            Log.Write("Finished Loading Contents.");
        }
        public void Close()
        {
            Log.Write("Closing XBDF File.");
            xIO.Dispose();
        }
        public static explicit operator byte[](GPD gpd)
        {
            gpd.pushOffsets();
            bool bigEndian = gpd.xIO.Endianness == Endian.Big;
            uint ecount = (uint)(gpd.Titles.Count + gpd.Achievements.Count + gpd.Settings.Count + gpd.Strings.Count + gpd.Images.Count + gpd.Lists.Count + gpd.SyncData.Count + gpd.Assets.Count),
                fcount = (uint)gpd.FreeEntries.LongLength;
            List<byte> bytes = new List<byte>();
            ///
            /// Header
            ///
            /*Xbox Official Calculation >.>*/
            //they enjoy 0x200 entries at a time, we don't want to make it easy for them to ban us, so we will write by 0x200 entries            
            bytes.AddRange(0x58444246u.ToBytes(bigEndian));
            bytes.AddRange(0x10000u.ToBytes(bigEndian));
            bytes.AddRange((0x200 + ((ecount / 0x200) * 0x200)).ToBytes(bigEndian));//length 
            bytes.AddRange(ecount.ToBytes(bigEndian));//count
            bytes.AddRange((0x200 + ((fcount / 0x200) * 0x200)).ToBytes(bigEndian));//length 
            bytes.AddRange(fcount.ToBytes(bigEndian));//count

            ///
            /// Entries Table
            ///
            for (int i = 0; i < gpd.Titles.Count; i++)
                bytes.AddRange((byte[])gpd.Titles[i].Entry);
            for (int i = 0; i < gpd.Assets.Count; i++)
                bytes.AddRange((byte[])gpd.Assets[i].Entry);
            for (int i = 0; i < gpd.Achievements.Count; i++)
                bytes.AddRange((byte[])gpd.Achievements[i].Entry);
            for (int i = 0; i < gpd.Settings.Count; i++)
                bytes.AddRange((byte[])gpd.Settings[i].Entry);
            for (int i = 0; i < gpd.Images.Count; i++)
                bytes.AddRange((byte[])gpd.Images[i].Entry);
            for (int i = 0; i < gpd.Strings.Count; i++)
                bytes.AddRange((byte[])gpd.Strings[i].Entry);
            for (int i = 0; i < gpd.Lists.Count; i++)
                bytes.AddRange((byte[])gpd.Lists[i].Entry);
            for (int i = 0; i < gpd.SyncData.Count; i++)
                bytes.AddRange((byte[])gpd.SyncData[i].Entry);
            bytes.AddRange(new byte[((0x200 + ((ecount / 0x200) * 0x200)) - ecount) * 0x12]);
            ///
            /// Free Entries
            ///
            for (int i = 0; i < gpd.freeentries.Count; i++)
                bytes.AddRange((byte[])gpd.freeentries[i]);
            bytes.AddRange(new byte[((0x200 + ((fcount / 0x200) * 0x200)) - fcount) * 0x8]);
            ///
            /// Data
            ///
            for (int i = 0; i < gpd.Titles.Count; i++)
                bytes.AddRange((byte[])gpd.Titles[i]);
            for (int i = 0; i < gpd.Assets.Count; i++)
                bytes.AddRange((byte[])gpd.Assets[i]);
            for (int i = 0; i < gpd.Achievements.Count; i++)
                bytes.AddRange((byte[])gpd.Achievements[i]);
            for (int i = 0; i < gpd.Settings.Count; i++)
                bytes.AddRange((byte[])gpd.Settings[i]);
            for (int i = 0; i < gpd.Images.Count; i++)
                bytes.AddRange((byte[])gpd.Images[i]);
            for (int i = 0; i < gpd.Strings.Count; i++)
                bytes.AddRange((byte[])gpd.Strings[i]);
            for (int i = 0; i < gpd.Lists.Count; i++)
                bytes.AddRange((byte[])gpd.Lists[i]);
            for (int i = 0; i < gpd.SyncData.Count; i++)
                bytes.AddRange((byte[])gpd.SyncData[i]);
            ///
            /// Free Space
            ///
            byte[] toReturn = bytes.ToArray(), toAdd = new byte[toReturn.Length + (long)gpd.freeentries[0].length];
            Array.Copy(toReturn, 0, toAdd, 0, toReturn.Length);
            return toAdd;
        }

        public void Inject(bool updateServer, Setting setting)
        {
            foreach (var settin in this.Settings.ToArray())
                if (settin.Entry.id == setting.Entry.id)
                    this.Settings.Remove(settin);
            this.Settings.Add(setting);
            updateEntrySync(updateServer, setting.Entry);
        }
        public void Inject(Image i)
        {
            foreach (var settin in this.Achievements.ToArray())
                if (settin.Entry.id == i.Entry.id)
                    this.Achievements.Remove(settin);
            this.Images.Add(i);
        }
        public void Inject(bool updateServer, Achievement a)
        {
            foreach (var settin in this.Achievements.ToArray())
                if (settin.Entry.id == a.Entry.id)
                    this.Achievements.Remove(settin);
            this.Achievements.Add(a);
            updateEntrySync(updateServer, a.Entry);
        }
        public void Inject(bool updateServer, Title t)
        {
            foreach (var settin in this.Titles.ToArray())
                if (settin.Entry.id == t.Entry.id)
                    this.Titles.Remove(settin);
            this.Titles.Add(t);
            updateEntrySync(updateServer, t.Entry);
        }
        public void Inject(String t)
        {
            foreach (var settin in this.Strings.ToArray())
                if (settin.Entry.id == t.Entry.id)
                    this.Strings.Remove(settin);
            this.Strings.Add(t);
        }

    }
}
