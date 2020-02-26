using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZodiacBack.Core.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PersonProperties
    {
        Surname,
        Name,
        Email,
        Birthday,
        WestSign,
        EastSign,
        IsBirthday,
        IsAdult,
        Age
    }
}