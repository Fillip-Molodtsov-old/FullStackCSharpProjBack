using System;
using System.Collections.Generic;

namespace ZodiacBack.Core.Models
{
    public class CustomProcess
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsResponding { get; set; }
        public double Cpu { get; set; }
        public double  Gpu { get; set; }
        public string Username { get; set; }
        public DateTime StartTime { get; set; }
        public string PathToFile { get; set; }
        public int ThreadsCount { get; set; }

        public List<CustomProcessModule> Modules { get; }
        public List<CustomProcessThreads> Threads { get; }

        public CustomProcess()
        {
            Modules = new List<CustomProcessModule>();
            Threads = new List<CustomProcessThreads>();
        }
        
    }
}