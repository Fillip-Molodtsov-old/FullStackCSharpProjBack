using System.Collections.Generic;
using ZodiacBack.Core.Exceptions;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Core.CustomResponses
{
    
    public class PersonResponse: AbstractCustomResponse
    {
        public Person Person { get; set; }

        private ZodiacResponse ZodiacResponse { get; set; }

        public PersonResponse(Person person)
        {
            Person = person;
            ZodiacResponse = new ZodiacResponse(Person.Birthday);
            FillPerson();
            ErrorMessages = GetErrorMessages();
            SpecialMessages = GetSpecialMessages();
        }

        protected sealed override IEnumerable<string> GetErrorMessages()
        {
             var list = new List<string>(ZodiacResponse.ErrorMessages);
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
            return ZodiacResponse.SpecialMessages;
        }

        private void ValidateEmail()
        {
            if (Person.Email.Contains(".ru"))
            {
                throw new EmailDomainValidation("ru");
            }
        }

        private void FillPerson()
        {
            Person.EastSign = ZodiacResponse.InfoBirthday.EastSign;
            Person.WestSign = ZodiacResponse.InfoBirthday.WestSign;
            Person.IsAdult = ZodiacResponse.InfoBirthday.IsAdult;
            Person.IsBirthday = ZodiacResponse.InfoBirthday.IsBirthday;
            Person.Age = ZodiacResponse.InfoBirthday.Age;
        }
    }
}