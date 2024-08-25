using BookingApp.Serializer;
using System;
using System.Globalization;

namespace BookingApp.Domain.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TourId { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public bool Arrived { get; set; } = false;
        public string ArrivedPeakPoint { get; set; } = "";
        public DateTime ArrivedAt { get; set; } = new DateTime();
        public TourPersonInfo Person { get; set; } = new();
        public bool IsCounted { get; set; } = false;
        public string GetHeader() => "Id|TourId|TouristId|Arrived|ArrivedPeakPoint|ArrivedAt|Person|IsCounted";

        public TourReservation() { }

        public TourReservation(int tourId, int touristId, string firstName, string lastName, string email, int birthYear)
        {
            TourId = tourId;
            TouristId = touristId;
            Person.FirstName = firstName;
            Person.LastName = lastName;
            Person.Email = email;
            Person.BirthYear = birthYear;
        }

        public string[] ToCsv() => new[] { $"{Id}", $"{TourId}", $"{TouristId}", $"{Arrived}", ArrivedPeakPoint, $"{ArrivedAt:yyyy-MM-dd HH:mm:ss}", $"{Person}", $"{IsCounted}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TourId = Convert.ToInt32(values[i++]);
            TouristId = Convert.ToInt32(values[i++]);
            Arrived = bool.Parse(values[i++]);
            ArrivedPeakPoint = values[i++];
            ArrivedAt = DateTime.ParseExact(values[i++], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Person = TourPersonInfo.Parse(values[i++]);
            IsCounted = bool.Parse(values[i++]);
        }

        public override string ToString() => $"{Id}|{TourId}|{TouristId}|{Arrived}|{ArrivedPeakPoint}|{Person}|{IsCounted}";
    }
}