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
    public struct Image
    {
        public Entry Entry;
        System.IO.MemoryStream ms;
        public System.Drawing.Image Png
        {
            get
            {
                return System.Drawing.Image.FromStream(ms);
            }
            set
            {
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                Entry.length = (uint)ms.Length;
            }
        }

        public static explicit operator byte[](Image i)
        {
            return i.ms.ToArray();
        }

        public Image(Entry e, ref IO io)
        {
            this.Entry = e;
            if (e.length > int.MaxValue)
                throw new IndexOutOfRangeException();
            this.ms = new System.IO.MemoryStream(io.ReadBytes((int)Entry.length));
            Entry.length = (uint)ms.Length;

            //testing
            //Png.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + DateTime.Now.Millisecond +".png");
            //System.Threading.Thread.Sleep(5000);
        }
        public static System.Drawing.Image LoadImage(long offset, long length, ref Xbox360Content.IO io)
        {
            try
            {
                io.Position = offset;
                if (length > int.MaxValue)
                    throw new IndexOutOfRangeException();
                return System.Drawing.Image.FromStream(new System.IO.MemoryStream(io.ReadBytes((int)length)));
            }
            catch (Exception ex) { Log.Write(string.Format("Problem loading Image @ 0x{0:x2",io.Position - length), 2); throw ex; }
        }
    }
}
