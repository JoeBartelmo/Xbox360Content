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

namespace Xbox360Content.XDBF.GPD
{
    public struct SyncItem
    {
        bool bigEndian;
        public long EntryID, SyncID;
        public SyncItem(ref IO io)
        {
            bigEndian = io.Endianness == Endian.Big;
            EntryID = io.ReadInt64();
            SyncID = io.ReadInt64();
        }

        public static explicit operator byte[](SyncItem s)
        {
            List<byte> bytes = new List<byte>(s.EntryID.ToBytes(s.bigEndian));
            bytes.AddRange(s.EntryID.ToBytes(s.bigEndian));
            return bytes.ToArray();
        }
    }

    public struct SyncList
    {
        public Entry Entry;
        internal List<SyncItem> items;
        public SyncItem[] Items { get { return items.ToArray(); } }
        public void SetLength()
        {
            Entry.length = (uint)(items.Count * 16);
        }
        public SyncList(Entry e, List<SyncItem> i)
        {
            Entry = e;
            items = i;
        }
        public static explicit operator byte[](SyncList list)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < list.Items.Length; i++)
                bytes.AddRange((byte[])list.Items[i]);
            return bytes.ToArray();
        }
    }

    public struct SyncData
    {
        public Entry Entry;
        bool bigEndian;
        public ulong NextSyncID, LastSyncID;
        private long time;
        public DateTime LastSyncTime { get { return DateTime.FromFileTime(time); } set { time = value.ToFileTime(); } }

        public SyncData(Entry e, ref IO io)
        {
            this.Entry = e;
            bigEndian = io.Endianness == Endian.Big;
            NextSyncID = io.ReadUInt64();
            LastSyncID = io.ReadUInt64();
            time = io.ReadInt64();
        }

        public static explicit operator byte[](SyncData s)
        {
            List<byte> bytes = new List<byte>(s.NextSyncID.ToBytes(s.bigEndian));
            bytes.AddRange(s.LastSyncID.ToBytes(s.bigEndian));
            bytes.AddRange(s.time.ToBytes(s.bigEndian));
            return bytes.ToArray();
        }
    }
}
