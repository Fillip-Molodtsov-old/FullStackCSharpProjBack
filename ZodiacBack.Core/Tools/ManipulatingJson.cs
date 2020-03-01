using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Core.Tools
{
    public class ManipulatingJson
    {
        private string FilePath { get; set; }


        public ManipulatingJson(string filePath)
        {
            FilePath = filePath;
            if (!File.Exists(FilePath))
            {
                Write(new List<Person>());
            }
        }

        public T Read<T>()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
        }

        public void Write<T>(T model)
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(model));
        }
    }
}