using System;
using ZodiacBack.Models;

namespace TestModel
{
    class Program
    {
        static void Main(string[] args)
        {
            ZodiacResponse zr = new ZodiacResponse("2000-01-25");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(zr.WestSign);
            Console.WriteLine(zr.IsBirthday);
            Console.WriteLine(zr.EastSign);
            Console.WriteLine(zr.SpecialMessages);
        }
    }
}