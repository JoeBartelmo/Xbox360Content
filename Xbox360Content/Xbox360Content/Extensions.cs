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

using System.Text;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/// <summary>
/// For use inside my DLL
/// </summary>
internal static class Extensions
{
    private static ASCIIEncoding ascii = new ASCIIEncoding();
    private static UnicodeEncoding unicode = new UnicodeEncoding();
    //good
    internal static void MakeFile(this byte[] bytes, string filename, bool useDesktop)
    {
        if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(useDesktop ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + filename : System.IO.Path.GetDirectoryName(filename))))
            System.IO.Directory.CreateDirectory(useDesktop ? (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + System.IO.Path.GetDirectoryName(filename)) : System.IO.Path.GetDirectoryName(filename));
        if (!useDesktop)
            using (var fs = System.IO.File.Create(filename))
                fs.Write(bytes, 0, bytes.Length);
        else
            using (var fs = System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + filename))
                fs.Write(bytes, 0, bytes.Length);
    }
    //good
    internal static byte[] SubArray(this byte[] buffer, int offset, int length)
    {
        byte[] toReturn = new byte[length];
        Array.Copy(buffer, offset, toReturn, 0, length);
        return toReturn;
    }
    //good
    internal static string GetString(this byte[] Bytes, bool Unicode)
    {
        return Unicode ? unicode.GetString(Bytes) : ascii.GetString(Bytes);
    }
    //good
    internal static bool ContainsAny(this string str, char[] chars)
    {
        foreach (char a in chars)
        {
            if (str.Contains(a.ToString()))
                return true;
        }
        return false;
    }
    //good
    internal static string ToHex(this byte[] ByteArray) { return BitConverter.ToString(ByteArray).Replace("-", ""); }
    
    //all good
    //we can increase load speeds with math here, better than converting to byte arr, then back to int
    //bitwise:
    //0x00000000
    //0x0000
    //0x0000000000000000
    //Oh, God forbid i mess up the int64...
    internal static int SwapEndian(this int i)
    {
        /*Positions
         * v1:31-24 @ 24 ->-24
         * v2:23-16 @ 16 ->-8
         * v3:17-8  @ 8 ->16
         * v4:8-0   @ 0 -> 24
         * 
         * SO:
         * 0x000000ff -> 0xff000000
         * 0x0000ff00 -> 0x00ff0000
         * 0x00ff0000 -> 0x0000ff00
         * 0xff000000 -> 0x000000ff
         */
        //first we get single byte, and then we shift over to new position
        return (int)(
            ((i & 0x000000ff) << 24) | 
            ((i & 0x0000ff00) << 8) | 
            ((i & 0x00ff0000) >> 8) | 
            ((int)(i & 0xff000000) >> 24));//for some reason the compiler was acting weird and treating this as a unsigned, throwing a warning, weird....
    }
    internal static uint SwapEndian(this uint i)
    {
        return (uint)(
            ((i & 0x000000ff) << 24) | 
            ((i & 0x0000ff00) << 8) | 
            ((i & 0x00ff0000) >> 8) | 
            ((i & 0xff000000) >> 24));
    }
    internal static short SwapEndian(this short i)
    {
        return (short)(((i & 0x00ff) << 8) | ((i & 0xff00) >> 8));
    }
    internal static ushort SwapEndian(this ushort i)
    {
        return (ushort)(((i & 0x00ff) << 8) | ((i & 0xff00) >> 8));
    }
    internal static long SwapEndian(this long i)
    {
        //oh boy, i hope i counted right
        unchecked
        {
            return (long)(
                ((i & 0x00000000000000ffL) << 56) |
                ((i & 0x000000000000ff00L) << 40) |
                ((i & 0x0000000000ff0000L) << 24) |
                ((i & 0x00000000ff000000L) << 8)) |
                ((i & 0x000000ff00000000L) >> 8) |
                ((i & 0x0000ff0000000000L) >> 24) |
                ((i & 0x00ff000000000000L) >> 40) |
                ((long)((ulong)i & 0xff00000000000000L) >> 56);//number too large >.> had to run a sloppy unchecked conversion
        }
    }
    internal static ulong SwapEndian(this ulong i)
    {
        //oh boy, i hope i counted right
        return (ulong)(
            ((i & 0x00000000000000ffuL) << 56) |
            ((i & 0x000000000000ff00uL) << 40) |
            ((i & 0x0000000000ff0000uL)) << 24 |
            ((i & 0x00000000ff000000uL) << 8) |
            ((i & 0x000000ff00000000uL)) >> 8 |
            ((i & 0x0000ff0000000000uL) >> 24) |
            ((i & 0x00ff000000000000uL)) >> 40 |
            ((i & 0xff00000000000000uL) >> 56));
    }
    internal static double SwapEndian(this double i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
    }
    internal static float SwapEndian(this float i)
    {
        byte[] bytes = BitConverter.GetBytes(i);
        Array.Reverse(bytes);
        return BitConverter.ToSingle(bytes, 0);
    }
    //all good
    internal static byte[] ToBytes(this float i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this double i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this int i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this uint i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this long i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this ulong i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this short i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this ushort i, bool BigEndian) { return BitConverter.GetBytes(BigEndian ? i.SwapEndian() : i); }
    internal static byte[] ToBytes(this string String, bool Unicode, bool bigEndian){

        if (!bigEndian)
            return Unicode ? unicode.GetBytes(String) : ascii.GetBytes(String);
        else
        {
            if (!Unicode) return ascii.GetBytes(String);
            unchecked
            {
                char[] chars = String.ToCharArray();
                ushort a = 0;
                for (int i = 0; i < chars.Length; i++)
                {
                    a = (ushort)((ushort)chars[i] >> 8);
                    a ^= (ushort)(((ushort)chars[i] & 0xff) << 8);
                    chars[i] = (char)a;
                }
                return (new string(chars)).ToBytes(true, false);
            }
        }
    }

    private static bool match(byte[] heystack, byte[] needle, int offset)
    {
        if (offset + needle.Length >= heystack.Length)
            return false;
        for (int i = 0; i < needle.Length; i++)
            if (needle[i] != heystack[i + offset])
                return false;
        return true;
    }
    static List<int> found = new List<int>();
    internal static int[] Find(this byte[] bytes, byte[] needle, byte threads = 1)
    {
        if (threads == 0)
            return null;
        if ((bytes.Length / threads) < needle.Length)
            throw new Exception("There are too many threads, the given heystack byte array is too small");
        Task[] tasks = new Task[threads];
        found.Clear();
        int splitLength = (bytes.Length / threads) + needle.Length;
        for (byte i = 1; i <= threads; i++)
        {
            if (i == threads)
                splitLength -= (bytes.Length % 2 == 0) ? needle.Length : (needle.Length - 1);
            tasks[i - 1] = Task.Factory.StartNew(new Action(() =>
            {
                //reinit all variables so we don't have cross threading
                //access
                byte[] target = needle,
                    heystack = bytes;
                int offset = i * splitLength;
                //run loop to find the needle
                for (int j = offset; j < offset + splitLength; j++)
                    lock (found)
                        if (match(heystack, target, j) && !found.Contains(j))
                            found.Add(j);
            }));
        }
        Task.WaitAll(tasks);
        return found.ToArray();
    }
}
