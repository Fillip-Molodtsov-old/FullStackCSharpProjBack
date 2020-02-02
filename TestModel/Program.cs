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
            foreach (var v in zr.ErrorMessages)
            {
                Console.WriteLine(v);
            }

            Console.WriteLine(zr.InfoBirthday.Age);
        }
    }
}