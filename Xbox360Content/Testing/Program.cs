using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360Content;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //IO test
            using (IO io = new IO(@"C:\Users\Joe\Desktop\asdf", Endian.Big))
            {
                Console.WriteLine(io.ReadInt32().ToString("X2"));
                Console.WriteLine(io.ReadInt32().ToString("X2"));
                Console.WriteLine(io.ReadInt64().ToString("X2"));
                Console.WriteLine(io.Position.ToString("X2"));
                Console.WriteLine(io.ReadZString(Endian.Big));
            }
            Console.ReadLine();
        }
    }
}
