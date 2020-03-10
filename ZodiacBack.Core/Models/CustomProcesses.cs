using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ZodiacBack.Core.Enums;

namespace ZodiacBack.Core.Models
{
    public class CustomProcesses
    {
        private ConcurrentBag<PerformanceCounterWrapper> _performanceCounters;
        private ConcurrentBag<PerformanceCounterWrapper> _performanceCountersReadOnly;
        private readonly List<CustomProcess> _processes;
        private static readonly IntPtr WtsCurrentServerHandle = IntPtr.Zero;
        private const int WtsUserName = 5;
        private static readonly object Locker = new object();
        private static long _ramKb;

        public CustomProcesses()
        {
            GetPhysicallyInstalledSystemMemory(out _ramKb);
            _processes = new List<CustomProcess>();
            _performanceCounters = new ConcurrentBag<PerformanceCounterWrapper>();
            _performanceCountersReadOnly = new ConcurrentBag<PerformanceCounterWrapper>();
            Task.Run(LoopTask);
        }
        
        public IEnumerable<string> GetProperties()
        {
            return Enum
                .GetValues(typeof(ProcessProperties))
                .Cast<ProcessProperties>()
                .Select(e => e.ToString())
                .ToList();
        }

        public static void OpenDirectory(string path)
        {
            Process.Start("explorer.exe" , Path.GetDirectoryName(path));
        }

        public static void KillProcess(int id)
        {
            Process.GetProcessById(id).Kill();
        }

        public CustomProcess GetAddInfo(int id)
        {
            try
            {
                FillNewCustomProcess(out var customProcess, Process.GetProcessById(id), true);
                return customProcess;
            }
            catch (ArgumentException)
            { }
            catch (InvalidOperationException)
            { }

            return null;
        }

        public IEnumerable<CustomProcess> GetResponseProcesses(ProcessProperties property, bool desc)
        {
            var processes = GetCustomProcesses();
            Func<CustomProcess, dynamic> func = p => ReturnPropertyValue(p, property);
            return desc ? processes.OrderByDescending(func) : processes.OrderBy(func);
        }

        private void RenewCpuStats()
        {
            Parallel.ForEach(_performanceCounters,
                    pc => pc.Next = pc.PerformanceCounter.NextValue());

        }

        private void UpdateReadOnlyPerformanceCounter()
        {
            lock (Locker)
            {
                _performanceCountersReadOnly = _performanceCounters;
            }
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
                    FillNewCustomProcess(out var customProcess, p, false);
                        _processes.Add(customProcess);
                }
                catch (Win32Exception)
                {
                }
                catch (InvalidOperationException)
                {
                }
            }
            
            return _processes;
        }

        private void FillNewCustomProcess(out CustomProcess customProcess, Process p, bool addInfo)
        {
            var nextValue = 0f;
            lock (Locker)
            {
                nextValue = _performanceCountersReadOnly
                    .ToList().Find(pc => pc.Id == p.Id).Next;
            }
            customProcess = new CustomProcess()
            {
                Name = p.ProcessName,
                Id = p.Id,
                IsResponding = p.Responding,
                Cpu = Math.Round(nextValue, 2),
                Gpu = Math.Round(p.WorkingSet64 / (10.24 * _ramKb), 2),
                StartTime = p.StartTime,
                PathToFile = p.MainModule?.FileName,
                Username = GetUserName(p)
            };
            
            foreach (ProcessThread thread in p.Threads)
            {
                customProcess.ThreadsCount++;
                if (!addInfo) continue;
                customProcess.Threads.Add(new CustomProcessThreads()
                {
                    Id = thread.Id,
                    StartTime = thread.StartTime,
                    State = thread.ThreadState.ToString()
                });
            }

            if (!addInfo) return;

            foreach (ProcessModule module in p.Modules)
            {
                customProcess.Modules.Add(new CustomProcessModule()
                {
                    Name = module.ModuleName,
                    Path = module.FileName
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

        private void LoopTask()
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
                UpdateReadOnlyPerformanceCounter();
                Thread.Sleep(2000);
                RenewCpuStats();
                UpdateReadOnlyPerformanceCounter();
                Thread.Sleep(2000);
                RenewCpuStats();
                UpdateReadOnlyPerformanceCounter();
            }
        }
        
        private static dynamic ReturnPropertyValue(CustomProcess process, ProcessProperties property)
        {
            return property switch
            {
                ProcessProperties.Name => process.Name,
                ProcessProperties.Id => process.Id,
                ProcessProperties.IsResponding => process.IsResponding,
                ProcessProperties.Cpu => process.Cpu,
                ProcessProperties.Gpu => process.Gpu,
                ProcessProperties.Username => process.Username,
                ProcessProperties.StartTime => process.StartTime,
                ProcessProperties.PathToFile => process.PathToFile,
                ProcessProperties.ThreadsCount => process.ThreadsCount,
                _ => process.Name
            };
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