using System.Collections.Generic;
using ZodiacBack.Core.HttpModels;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Core.CustomResponses
{
    
    public class PersonResponse: AbstractCustomResponse
    {
        public Person Person { get; }

        public PersonResponse(PersonHttpBody person)
        {
            Person = new Person(person);
            ErrorMessages = Person.ZodiacResponse.ErrorMessages;
            SpecialMessages = Person.ZodiacResponse.SpecialMessages;
        }

        protected sealed override IEnumerable<string> GetErrorMessages()
        {
            return new List<string>();
        }

        protected sealed override IEnumerable<string> GetSpecialMessages()
        {
            return new List<string>();
        }
    }
}