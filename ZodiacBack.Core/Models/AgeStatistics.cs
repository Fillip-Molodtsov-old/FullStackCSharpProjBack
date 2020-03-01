using System;

namespace ZodiacBack.Core.Models
{
    public class AgeStatistics
    {
        private DateTime BirthdayDate { get; }

        public bool IsBirthday => CheckIfBirthday();

        public bool IsAdult => CheckIfAdult();

        public int Age => CalculateAge();

        public AgeStatistics(DateTime birthday)
        {
            BirthdayDate = birthday;
        }

        public bool IsFromFuture()
        {
            var now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var dob = int.Parse(BirthdayDate.ToString("yyyyMMdd"));
            return now - dob < 0;
        }

        private int CalculateAge()
        {
            var now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            var dob = int.Parse(BirthdayDate.ToString("yyyyMMdd"));
            return (now - dob) / 10000;
        }

        private bool CheckIfBirthday()
        {
            DateTime now = DateTime.Now;
            return BirthdayDate.Day == now.Day && BirthdayDate.Month == now.Month;
        }

        private bool CheckIfAdult()
        {
            return Age >= 18;
        }
    }
}