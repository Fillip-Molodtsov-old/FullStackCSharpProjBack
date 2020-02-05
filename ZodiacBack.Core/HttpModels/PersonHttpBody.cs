using System;
using Newtonsoft.Json;

namespace ZodiacBack.Core.HttpModels
{
    public class PersonHttpBody
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        [JsonConstructor]
        public PersonHttpBody(string name, string surname, string email, DateTime birthday)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Birthday = birthday;
        }
        
        public PersonHttpBody(string name, string surname, string email)
            : this(name,surname,email,DateTime.Parse("2000-01-01")) { }

        public PersonHttpBody(string name, string surname, DateTime birthday) 
            :this (name,surname,null,birthday){ }

        public PersonHttpBody()
        { }
    }
}