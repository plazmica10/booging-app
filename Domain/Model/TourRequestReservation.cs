using BookingApp.Serializer;
using System;

namespace BookingApp.Domain.Model
{
    public class TourRequestReservation : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TourRequestId { get; set; } = -1;
        public TourPersonInfo Person { get; set; } = new();
        public string GetHeader() => "Id|TourRequestId|Person";

        public TourRequestReservation() { }

        public TourRequestReservation(int tourRequestId, string firstName, string lastName, string email, int birthYear)
        {
            TourRequestId = tourRequestId;
            Person.FirstName = firstName;
            Person.LastName = lastName;
            Person.Email = email;
            Person.BirthYear = birthYear;
        }

        public string[] ToCsv() => new[] { $"{Id}", $"{TourRequestId}", $"{Person}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TourRequestId = Convert.ToInt32(values[i++]);
            Person = TourPersonInfo.Parse(values[i++]);
        }

        public override string ToString() => $"{Id}|{TourRequestId}|{Person}";
    }
}