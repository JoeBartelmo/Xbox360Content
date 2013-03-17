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

        internal uint EntryCount { get { return dataHeader[2]; } set { dataHeader[2] = value; } }
        internal uint EntryTableLength { get { return dataHeader[1]; } set { dataHeader[1] = value; } }
        internal uint FreeSpaceLength { get { return dataHeader[3]; } set { dataHeader[3] = value; } }
        internal uint FreeSpaceCount { get { return dataHeader[4]; } set { dataHeader[4] = value; } }
        
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
        internal byte[] GetData(Entry entry)
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

        
        void xRead()
        {
            Log.Write("Reading XDBF Header...");
            uint type = xIO.ReadUInt32(Endian.Big);
            if (type != 0x58444246 || type != 0x46424458)
                throw new XDBFException("Invalid Magic (first four bytes is not valid)");
            xIO.Endianness = (type == 0x58444246) ? Endian.Big : Endian.Little;
            for (int i = 0; i < dataHeader.Length; i++)
                dataHeader[i] = xIO.ReadUInt32();//Reads header information
            

        }

        public static explicit operator byte[](XDBF xbdfPackage)
        {
            return null;
        }
    }
}
