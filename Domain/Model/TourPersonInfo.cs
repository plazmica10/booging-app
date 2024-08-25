using System;

namespace BookingApp.Domain.Model
{
    public class TourPersonInfo
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public int BirthYear { get; set; } = -1;

        public TourPersonInfo() { }

        public TourPersonInfo(string firstName, string lastName, string email, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthYear = birthYear;
        }

        public static TourPersonInfo Parse(string value)
        {
            string[] values = value.Split(";");
            int i = 0;
            return new TourPersonInfo(values[i++], values[i++], values[i++], Convert.ToInt32(values[i++]));
        }

        public override string ToString() => $"{FirstName};{LastName};{Email};{BirthYear}";
    }
}
