/*  Copyright (C) 2012 Joseph Bartelmo

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

/// <summary>
/// A lot of these methods are not proper, and they will be replaced in the future, but you are not meant to use them, they are for me in this DLL
/// </summary>
internal static class Extensions
{
    internal static byte[] Range(this byte[] bytes, int index, int length)
    {
        return bytes.Skip(index).Take(length).ToArray();
    }
    internal static void MakeFile(this byte[] bytes, string filename, bool useDesktop)
    {
        if (!useDesktop)
            using (var fs = System.IO.File.Create(filename))
                fs.Write(bytes, 0, bytes.Length);
        else
            using (var fs = System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + filename))
                fs.Write(bytes, 0, bytes.Length);
    }
    internal static byte[] SelectSection(this byte[] buffer, int offset, int length)
    {
        byte[] toReturn = new byte[length];
        for (int i = offset; i < offset + length; i++)
            toReturn[i - offset] = buffer[i];
        return toReturn;
    }
    internal static byte[] ToBytes(this string String, bool Unicode)
    {
        List<byte> bytes = new List<byte>();
        foreach (char a in String)
        {
            bytes.Add((byte)a);
            if (Unicode)
                bytes.Add(0);
        }
        return bytes.ToArray();
    }
    internal static string GetString(this byte[] Bytes, bool HexEditor = false)
    {
        StringBuilder String = new StringBuilder();
        foreach (byte a in Bytes)
        {
            if (HexEditor)
            {
                if (a == 0)
                    String.Append('.');
                else
                    String.Append((char)a);
            }
            else
                String.Append((char)a);
        }
        return String.ToString();//irony :)
    }
    internal static bool ContainsAny(this string str, char[] chars)
    {
        foreach (char a in chars)
        {
            if (str.Contains(a.ToString()))
                return true;
        }
        return false;
    }
    internal static string ToHex(this byte[] bytes, bool addspace = false)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte a in bytes)
        {
            if (addspace)
                sb.Append(" ");
            sb.Append(a.ToString("X2"));
        }
        return sb.ToString();
    }
    internal static string[] Split(this string a, int count, bool seperatebyspace = false)
    {
        if (!seperatebyspace)
        {
            List<string> returned = new List<string>();
            for (int i = 0; i < a.Length; i += count)
            {
                try
                { returned.Add(a.Substring(i, count)); }
                catch
                { returned.Add(a.Substring(i, (a.Length - i))); }
            }
            return returned.ToArray();
        }
        else
        {
            List<string> returned = new List<string>();
            int c = count;
            for (int i = 0; i < a.Length; i += c)
            {
                try
                {
                    char[] substring = a.Substring(i, count).ToCharArray();
                    int offset = -1;
                    for (int x = substring.Length; x > 0; x--)
                        if (substring[x] == ' ')
                            offset = x;
                    returned.Add(a.Substring(i, offset));
                    c = (offset + 1);
                }
                catch
                { returned.Add(a.Substring(i, (a.Length - i))); }
            }
            return returned.ToArray();
        }
    }
    internal static string ToHexString(this byte[] ByteArray)
    {
        string r = "";
        for (int i = 0; i < ByteArray.Length; i++)
            r += ByteArray[i].ToString("X2");
        return r;
    }
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
                ((i & (long)0xff00000000000000L) >> 56);//number too large >.> had to run an unchecked conversion
        }
    }
    internal static ulong SwapEndian(this ulong i)
    {
        //oh boy, i hope i counted right
        return (ulong)(
            ((i & 0x00000000000000ffL) << 56) |
            ((i & 0x000000000000ff00L) << 40) |
            ((i & 0x0000000000ff0000L)) << 24 |
            ((i & 0x00000000ff000000L) << 8) |
            ((i & 0x000000ff00000000L)) >> 8 |
            ((i & 0x0000ff0000000000L) >> 24) |
            ((i & 0x00ff000000000000L)) >> 40 |
            ((i & 0xff00000000000000L) >> 56));
    }
}
