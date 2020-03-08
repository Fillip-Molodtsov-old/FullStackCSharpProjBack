using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TestModel
{
    class Program
    {

        // public static void Main(string[] args)
        // {
        //     var processes = Process.GetProcesses();
        //     
        //     foreach (var p in processes)
        //     {
        //         try
        //         {
        //             Console.WriteLine(p.MainModule?.FileName);
        //             Console.WriteLine(p.Id);
        //         }
        //         catch (System.ComponentModel.Win32Exception e)
        //         {
        //             continue;
        //         }
        //         catch (System.InvalidOperationException e)
        //         {
        //             continue;
        //         }
        //     
        //     }
        //     // Process.Start("explorer.exe" , @"C:\Users");
        // }
    }

    class Program2
    {
        static void Main(string[] args)
        {
            Process[] processes = Process.GetProcesses();

            var counters = new List<PerformanceCounter>();
            
            foreach (Process process in processes)
            {
                var counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                counter.NextValue();
                counters.Add(counter);
            }
            
            int i = 0;
            
            Thread.Sleep(1000);
            
            foreach (var counter in counters)
            {
                Console.WriteLine(processes[i].ProcessName + "       | CPU% " + (Math.Round(counter.NextValue(), 1)));
                ++i;
            }
        }
    }
}