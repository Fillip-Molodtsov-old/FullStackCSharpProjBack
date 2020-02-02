using System.Collections.Generic;

namespace ZodiacBack.Core
{
    public abstract class AbstractCustomResponse
    {
        public IEnumerable<string> SpecialMessages { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }


        protected AbstractCustomResponse()
        {
            SpecialMessages = new List<string>();
            ErrorMessages = new List<string>();
        }

        protected abstract IEnumerable<string> GetErrorMessages();
        protected abstract IEnumerable<string> GetSpecialMessages();
    }
}