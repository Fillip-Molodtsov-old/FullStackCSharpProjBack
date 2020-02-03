using ZodiacBack.Core.CustomResponses;
using ZodiacBack.Core.HttpModels;

namespace ZodiacBack.Core.Models
{
    public class Person
    {
        public PersonHttpBody PersonalInfo { get; set; }
        public ZodiacResponse ZodiacResponse { get; set; }

        public Person(PersonHttpBody personHttpBody)
        {
            PersonalInfo = personHttpBody;
            ZodiacResponse = new ZodiacResponse(PersonalInfo.Birthday);
        }
    }
}