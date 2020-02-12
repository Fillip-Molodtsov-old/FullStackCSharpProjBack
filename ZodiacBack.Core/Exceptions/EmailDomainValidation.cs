using System;

namespace ZodiacBack.Core.Exceptions
{
    public class EmailDomainValidation: Exception
    {
        public EmailDomainValidation()
        { }

        public EmailDomainValidation(string domain)
        : base($"Імейли з доменом '{domain}' заборонені нашим сервісом.")
        { }
    }
}