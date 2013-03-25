using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360Content;

namespace Xbox360Content.XDBF
{
    /// <summary>
    /// Entry Point
    /// </summary>
    public class XDBF
    {
        public IO xIO;
        uint[] dataHeader = new uint[5];

        protected internal uint EntryCount { get { return dataHeader[2]; } set { dataHeader[2] = value; } }
        protected internal uint EntryTableLength { get { return dataHeader[1]; } set { dataHeader[1] = value; } }
        protected internal uint FreeSpaceLength { get { return dataHeader[3]; } set { dataHeader[3] = value; } }
        protected internal uint FreeSpaceCount { get { return dataHeader[4]; } set { dataHeader[4] = value; } }
        
        protected List<Entry> entries = new List<Entry>();
        protected List<FreeEntry> freeentries = new List<FreeEntry>();
        public Entry[] Entries { get { return entries.ToArray(); } }
        public FreeEntry[] FreeEntries { get { return freeentries.ToArray(); } }

        /// <summary>
        /// Get's the offset of the entry data
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal long GetOffset(uint offset)
        {
            return ((((EntryTableLength * 18) +
                (FreeSpaceLength * 8))
                + 24)
                + offset);
        }
        public byte[] GetRawData(Entry entry)
        {
            xIO.Position = this.GetOffset(entry.Offset);
            if (entry.Length > int.MaxValue)
            {
                List<byte> bytes = new List<byte>();
                bytes.AddRange(xIO.ReadBytes(int.MaxValue));
                bytes.AddRange(xIO.ReadBytes((int)(entry.Length - int.MaxValue)));
                return bytes.ToArray();
            }
            else return xIO.ReadBytes((int)entry.Length);
        }
        public byte[] GetRawData(FreeEntry entry)
        {
            xIO.Position = this.GetOffset(entry.Offset);
            if (entry.Length > int.MaxValue)
            {
                List<byte> bytes = new List<byte>();
                bytes.AddRange(xIO.ReadBytes(int.MaxValue));
                bytes.AddRange(xIO.ReadBytes((int)(entry.Length - int.MaxValue)));
                return bytes.ToArray();
            }
            else return xIO.ReadBytes((int)entry.Length);
        }

        public XDBF(IO io)
        {
            xIO = io;
            xRead();
        }
        
        protected void xRead()
        {
            entries.Clear();
            freeentries.Clear();
            try
            {
                Log.Write("############## XDBF ##############");
                Log.Write("Reading XDBF Header...");
                xIO.Position = 0;
                uint type = xIO.ReadUInt32(Endian.Big);
                if (type != 0x58444246u && type != 0x46424458u)
                    throw new XDBFException("Invalid Magic (first four bytes is not valid)");
                xIO.Endianness = (type == 0x58444246) ? Endian.Big : Endian.Little;
                for (int i = 0; i < dataHeader.Length; i++)
                    dataHeader[i] = xIO.ReadUInt32();//Reads header information
                for (int i = 0; i < this.EntryCount; i++)
                    entries.Add(new Entry(ref xIO));
                xIO.Seek((EntryTableLength * 0x12) + 18);
                for (int i = 0; i < this.FreeSpaceCount; i++)
                    freeentries.Add(new FreeEntry(ref xIO));
                Log.Write("Free Entries, and Entries have been loaded in the XDBF File.");
            }
            catch (Exception ex) { Log.Write("Error occured while loading header, is this a proper XDBF File?"); throw ex; }
        }
        public Entry SearchByID(ulong ID)
        {
            for (int i = 0; i < Entries.Length; i++)
                if (Entries[i].id == ID)
                    return Entries[i];
            throw new EntryPointNotFoundException();
        }
        public static explicit operator byte[](XDBF xbdfPackage)
        {
            return null;
        }
    }
}
