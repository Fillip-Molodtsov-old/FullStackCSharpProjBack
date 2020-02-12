using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ZodiacBack.Core.Exceptions;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Core.CustomResponses
{
    public class ZodiacResponse : AbstractCustomResponse
    {
        [JsonPropertyName("ageInfo")]
        public InfoBirthday InfoBirthday { get; }


        public ZodiacResponse(DateTime birthday)
        {
            InfoBirthday = new InfoBirthday(birthday);
            ErrorMessages = GetErrorMessages();
            SpecialMessages = GetSpecialMessages();
        }


        protected sealed override IEnumerable<string> GetErrorMessages()
        {
            var list = new List<string>();
            try
            {
                ValidateAge();
            }
            catch (AgeValidationException ex)
            {
                list.Add(ex.Message);
            }

            return list;
        }

        protected sealed override IEnumerable<string> GetSpecialMessages()
        {
            if (ErrorMessages.Any()) return null;

            var messages = new List<string>();
            if (InfoBirthday.IsBirthday)
            {
                messages.Add("Друже, з днем народження!");
            }

            return messages;
        }

        private void ValidateAge()
        {
            if (InfoBirthday.Age > 135)
            {
               throw  new AgeValidationException(InfoBirthday.Age,"Людина не може жити більше 135 років");
            }
            else if (InfoBirthday.Age <= 0)
            {
                throw  new AgeValidationException(InfoBirthday.Age,"Схоже Ви народилися в майбутньому");
            }
        }
    }
}