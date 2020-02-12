using System;

namespace ZodiacBack.Core.Exceptions
{
    public class AgeValidationException : Exception
    {
        public AgeValidationException() { }

        public AgeValidationException(int age, string customString)
            : base($"За данними Ваш вік {age} років. {customString}")
        { }
    }
}