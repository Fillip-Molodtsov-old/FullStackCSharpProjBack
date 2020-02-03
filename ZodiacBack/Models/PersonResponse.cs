using System.Collections.Generic;
using ZodiacBack.Core;
using ZodiacBack.Models.HttpModels;

namespace ZodiacBack.Models
{
    
    public class PersonResponse: AbstractCustomResponse
    {
        public Person Person { get; }
        public InfoBirthday InfoBirthday { get; }

        public PersonResponse(PersonHttpBody person)
        {
            // var zodiacResponse = new ZodiacResponse(Person.Birthday);
            // InfoBirthday = zodiacResponse.InfoBirthday;
            // ErrorMessages = zodiacResponse.ErrorMessages;
            // SpecialMessages = zodiacResponse.SpecialMessages;
            // TODO create person class  and finish backend task
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