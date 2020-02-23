using System;
using Newtonsoft.Json;

namespace ZodiacBack.Core.Models
{
    public class Person
    { 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }

        public string WestSign { get; set; }

        public string EastSign { get; set; }

        public bool IsBirthday { get; set; }

        public bool IsAdult { get; set; }

        public int Age { get; set; }

        public Guid Id { get; set; }

        [JsonConstructor]
        public Person(string name, string surname,
            string email, DateTime birthday,
            string westSign = null, string eastSign = null,
            bool isBirthday = false, bool isAdult = false, int age = -1)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Birthday = birthday;
            WestSign = westSign;
            EastSign = eastSign;
            IsBirthday = isBirthday;
            IsAdult = isAdult;
            Age = age;
        }

        public Person(string name, string surname, string email)
            : this(name, surname, email, DateTime.Parse("2000-01-01"))
        {
        }

        public Person(string name, string surname, DateTime birthday)
            : this(name, surname, null, birthday)
        {
        }

        public Person()
        {
        }
    }
}