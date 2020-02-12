using System.Collections.Generic;
using ZodiacBack.Core.Exceptions;
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
             var list = new List<string>(Person.ZodiacResponse.ErrorMessages);
             try
             {
                ValidateEmail();
             }
             catch (EmailDomainValidation ex)
             {
                 list.Add(ex.Message);
             }

             return list;
        }

        protected sealed override IEnumerable<string> GetSpecialMessages()
        {
            return Person.ZodiacResponse.SpecialMessages;
        }

        private void ValidateEmail()
        {
            if (Person.PersonalInfo.Email.Contains(".ru"))
            {
                throw new EmailDomainValidation("ru");
            }
        }
    }
}