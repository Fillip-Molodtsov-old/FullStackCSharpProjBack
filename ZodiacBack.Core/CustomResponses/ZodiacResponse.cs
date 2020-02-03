using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Core.CustomResponses
{
    public class ZodiacResponse : AbstractCustomResponse
    {
        [JsonPropertyName("ageInfo")]
        public InfoBirthday InfoBirthday { get; }


        public ZodiacResponse(string birthday)
        {
            try
            {
                InfoBirthday = new InfoBirthday(birthday);
            }
            catch (Exception e)
            {
                ErrorMessages = new List<string>() {e.Message};
                return;
            }

            ErrorMessages = GetErrorMessages();
            SpecialMessages = GetSpecialMessages();
        }


        protected sealed override IEnumerable<string> GetErrorMessages()
        {
            var list = new List<string>();
            if (InfoBirthday.Age > 135)
            {
                list.Add("Людина не може жити більше 135 років");
            }
            else if (InfoBirthday.Age <= 0)
            {
                list.Add("Схоже Ви народилися в майбутньому");
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
    }
}