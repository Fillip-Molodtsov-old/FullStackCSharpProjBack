using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ZodiacBack.Core.Models
{
    public class CustomProcesses
    {
        private ConcurrentBag<PerformanceCounterWrapper> _performanceCounters;
        private readonly List<CustomProcess> _processes;
        private static readonly IntPtr WtsCurrentServerHandle = IntPtr.Zero;
        private const int WtsUserName = 5;
        private static readonly object Locker = new object();
        private static long RamKb;

        public CustomProcesses()
        {
            GetPhysicallyInstalledSystemMemory(out RamKb);
            _processes = new List<CustomProcess>();
            var task = Task.Run(() =>
            {
                while (true)
                {
                    var processes = Process.GetProcesses();
                    _performanceCounters = new ConcurrentBag<PerformanceCounterWrapper>(processes
                        .AsParallel()
                        .AsOrdered()
                        .Select(proc =>
                        {
                            var pcProcess =
                                new PerformanceCounter("Process",
                                    "% Processor Time", proc.ProcessName);
                            return new PerformanceCounterWrapper()
                            {
                                Id = proc.Id,
                                Next = pcProcess.NextValue(),
                                PerformanceCounter = pcProcess
                            };
                        }));
                    Thread.Sleep(2000);
                    RenewCpuStats();
                    Thread.Sleep(2000);
                    RenewCpuStats();
                    Thread.Sleep(5000);
                }
            });
        }

        public IEnumerable<CustomProcess> GetResponseProcesses()
        {
            return GetCustomProcesses();
        }

        private void RenewCpuStats()
        {
            Parallel.ForEach(_performanceCounters,
                pc => pc.Next = pc.PerformanceCounter.NextValue());
        }

        private List<CustomProcess> GetCustomProcesses()
        {
            _processes.Clear();
            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                p.Refresh();
                try
                {
                    lock (Locker)
                    {
                        FillNewCustomProcess(out var customProcess, p, false);
                        _processes.Add(customProcess);
                    }
                }
                catch (Win32Exception)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }

            ;

            return _processes;
        }

        private void FillNewCustomProcess(out CustomProcess customProcess, Process p, bool addInfo)
        {
            var nextValue = _performanceCounters
                .ToList().Find(pc => pc.Id == p.Id).Next;
            customProcess = new CustomProcess()
            {
                Name = p.ProcessName,
                Id = p.Id,
                IsResponding = p.Responding,
                Cpu = Math.Round(nextValue, 2),
                Gpu = Math.Round(p.WorkingSet64 / (10.24 * RamKb), 2),
                StartTime = p.StartTime,
                PathToFile = p.MainModule?.FileName,
                Username = GetUserName(p)
            };

            if (!addInfo) return;

            foreach (ProcessModule module in p.Modules)
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
                if (!string.IsNullOrWhiteSpace(userName)) return userName;
            }

            return null;
        }

        [DllImport("Wtsapi32.dll")] // for getting username
        public static extern bool WTSQuerySessionInformationW(
            IntPtr hServer,
            int sessionId,
            int wtsInfoClass,
            out IntPtr ppBuffer,
            out IntPtr pBytesReturned);


        [DllImport("kernel32.dll")] // for getting total ram
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long totalMemoryInKilobytes);
    }
}