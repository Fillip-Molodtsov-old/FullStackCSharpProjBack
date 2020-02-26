using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZodiacBack.Core.CustomResponses;
using ZodiacBack.Core.Enums;
using ZodiacBack.Core.Tools;

namespace ZodiacBack.Core.Models
{
    public class People
    {
        private readonly List<Person> _peopleList;

        private readonly ManipulatingJson _manipulatingJson;

        public People()
        {
            Console.WriteLine(Directory.GetParent(Environment.CurrentDirectory));
            var pathToData = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).FullName,
                "ZodiacBack.Core\\Assets\\Data.json");
            _manipulatingJson = new ManipulatingJson(pathToData);
            _peopleList = _manipulatingJson.Read<List<Person>>();
        }

        public IEnumerable<Person> GetOrderedList(PersonProperties propertyToSort,
            bool descending,
            IEnumerable<Person> list = null)
        {
            list ??= _peopleList;
            
            if (descending)
                return list
                    .OrderByDescending(p => ReturnPropertyValue(p, propertyToSort));

            return list
                .OrderBy(p => ReturnPropertyValue(p, propertyToSort));
        }

        public IEnumerable<string> GetProperties()
        {
            return Enum
                .GetValues(typeof(PersonProperties))
                .Cast<PersonProperties>()
                .Select(e => e.ToString())
                .ToList();
        }

        public IEnumerable<Person> GetFilteredPeople(PersonProperties property, string value, bool isJustFilter)
        {
            var list =  _peopleList
                .Where(p => (ReturnPropertyValue(p, property).ToString()).ToLower().Contains(value.ToLower()));

            return !isJustFilter ? list : list.OrderBy(p => p.Surname);
        }

        public IEnumerable<Person> GetFilteredAndOrderedPeople(PersonProperties propertyToFilter, string value,
            PersonProperties propertyToSort, bool descending)
        {
            return GetOrderedList(propertyToSort, descending, GetFilteredPeople(propertyToFilter, value, false));
        }

        public PersonResponse AddPerson(Person p)
        {
            var personResponse = new PersonResponse(p, true);
            if (personResponse.ErrorMessages.Any()) return personResponse;
            _peopleList.Add(personResponse.Person);
            return personResponse;
        }

        public PersonResponse PutPerson(Person p)
        {
            var idx = _peopleList.FindIndex(per => per.Id == p.Id);
            var personResponse = new PersonResponse(p, false);
            if (idx == -1)
            {
                var errors = new List<string>(personResponse.ErrorMessages);
                errors.Add("Не знайдено елемента з таким id.");
                personResponse.ErrorMessages = errors;
            }

            if (personResponse.ErrorMessages.Any()) return personResponse;
            _peopleList[idx] = p;
            return personResponse;
        }

        public void DeletePerson(string id)
        {
            var delItem = _peopleList.Find(p => string.Equals(p.Id.ToString(), id));
            _peopleList.Remove(delItem);
        }

        public void SaveData()
        {
            _manipulatingJson.Write(_peopleList);
        }

        private dynamic ReturnPropertyValue(Person person, PersonProperties property)
        {
            return property switch
            {
                PersonProperties.Name => person.Name,
                PersonProperties.Surname => person.Surname,
                PersonProperties.Email => person.Email,
                PersonProperties.Birthday => person.Birthday,
                PersonProperties.WestSign => person.WestSign,
                PersonProperties.EastSign => person.EastSign,
                PersonProperties.IsBirthday => person.IsBirthday,
                PersonProperties.IsAdult => person.IsAdult,
                PersonProperties.Age => person.Age,
                _ => person.Surname
            };
        }
    }
}