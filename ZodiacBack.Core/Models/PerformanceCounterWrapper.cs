using System.Diagnostics;

namespace ZodiacBack.Core.Models
{
    public class PerformanceCounterWrapper
    {
        public PerformanceCounter PerformanceCounter { get; set; }
        public int Id { get; set; }
        public float Next { get; set; }
    }
}