using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ZodiacBack.Core.Models
{
    public class CustomProcesses
    {
        private readonly List<CustomProcess> _processes;
        private static readonly IntPtr WtsCurrentServerHandle = IntPtr.Zero;
        private static readonly int WtsUserName = 5;
        
        public CustomProcesses()
        {
            _processes = new List<CustomProcess>();
            var task = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine("uy");
                    Thread.Sleep(2000);
                }
            });
        }

        public IEnumerable<CustomProcess> RefreshProcesses()
        {
            _processes.Clear(); // todo delete
            var processes = Process.GetProcesses();
            
            CustomProcess customProcess;
            foreach (var p in processes)
            {
                try
                {
                    // using var pcProcess = 
                    //     new PerformanceCounter("Process",
                    //         "% Processor Time", p.ProcessName);
                    // pcProcess.NextValue();
                    customProcess = new CustomProcess()
                    {
                        Name = p.ProcessName,
                        Id = p.Id,
                        IsResponding = p.Responding,
                        Cpu = 0,
                        Gpu = p.WorkingSet64 / 1048576, //megabytes
                        StartTime = p.StartTime,
                        PathToFile = p.MainModule?.FileName,
                        Username = GetUserName(p)
                    };
                    
                    foreach(ProcessModule module in  p.Modules)
                    {
                        customProcess.Modules.Add(new CustomProcessModule()
                        {
                            Name = module.ModuleName,
                            Path = module.FileName
                        });
                    }

                    foreach (ProcessThread thread in p.Threads)
                    {
                        customProcess.Threads.Add(new CustomProcessThreads()
                        {
                            Id = thread.Id,
                            StartTime = thread.StartTime,
                            State = thread.ThreadState.ToString()
                        });
                    }
                    _processes.Add(customProcess);
                }
                catch (Win32Exception e)
                {
                    // Console.WriteLine(e);
                    continue;
                }
                catch (InvalidOperationException e)
                {
                    // Console.WriteLine(e);
                    continue;
                }

            }
            return _processes;
        }

        private static string GetUserName(Process proc)
        {
            //access to the Idle process is restricted, so don't try to access it.
            if (proc.ProcessName == "Idle") return null;
            if (WTSQuerySessionInformationW(WtsCurrentServerHandle,
                proc.SessionId,
                WtsUserName,
                out var answerBytes,
                out _))
            {
                var userName = Marshal.PtrToStringUni(answerBytes);
                if(!string.IsNullOrWhiteSpace(userName)) return userName;
            }
            return null;
        }
        
        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformationW(
            IntPtr hServer,
            int SessionId,
            int WTSInfoClass,
            out IntPtr ppBuffer,
            out IntPtr pBytesReturned);
    }
}