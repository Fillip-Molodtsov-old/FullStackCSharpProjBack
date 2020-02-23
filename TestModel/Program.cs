using System;
using ZodiacBack.Core.Models;

namespace TestModel
{
    class Program
    {
        public static void Main(string[] args)
        {
            var p = new Person();
            foreach(var field in p.GetType().GetProperties())
            {
                Console.WriteLine();
            }
        }
    }
}