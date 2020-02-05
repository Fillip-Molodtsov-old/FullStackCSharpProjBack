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
            ErrorMessages = GetErrorMessages();
            SpecialMessages = GetSpecialMessages();
        }

        protected sealed override IEnumerable<string> GetErrorMessages()
        {
            return Person.ZodiacResponse.ErrorMessages;
        }

        protected sealed override IEnumerable<string> GetSpecialMessages()
        {
            return Person.ZodiacResponse.SpecialMessages;
        }
    }
}