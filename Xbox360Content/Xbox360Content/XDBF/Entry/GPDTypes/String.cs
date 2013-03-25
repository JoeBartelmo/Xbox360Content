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
    public struct String
    {
        public Entry Entry;
        bool bigEndian;
        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.Entry.length = (uint)((value.Length * 2) + 2);
                this.value = value;
            }
        }
        public String(Entry e, ref IO io)
        {
            this.Entry = e;
            bigEndian = io.Endianness == Endian.Big;
            value = io.ReadZString(true);
            this.Entry.length = (uint)((value.Length + 1) * 2);

            //testing
            //((byte[])this).MakeFile(DateTime.Now.Millisecond.ToString(), true);
            //System.Threading.Thread.Sleep(5000);
        }
        public static explicit operator byte[](String s)
        {
            List<byte> bytes = new List<byte>(s.Value.ToBytes(true, s.bigEndian));
            bytes.AddRange(new byte[2] { 0, 0 });
            return bytes.ToArray();
        }
    }
}
