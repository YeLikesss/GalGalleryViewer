using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using GalleryFormat;

namespace ConsoleUnitTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====UnitTest-Start=====");
            UnitTest.Entry();
            Console.WriteLine("======UnitTest-End======");
            Console.Read();
        }
    }
}