using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZodiacBack.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProcessProperties
    {
        Name,
        Id,
        IsResponding,
        Cpu,
        Gpu,
        Username,
        StartTime,
        PathToFile,
        ThreadsCount
    }
}