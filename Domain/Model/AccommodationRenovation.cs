using System;
using System.Globalization;
using BookingApp.Application.UseCases;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum RenovationStatus { Pending, InProgress, Canceled, Completed }
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = "";
        public RenovationStatus Status { get; set; }
        public Accommodation? AccommodationForRenovation { get; set; } = new();
        public AccommodationReservation? Reservation { get; set; } = new();

        public string GetHeader() { return "Id|StartDate|EndDate|Description|Status|Accommodation|Reservation"; }

        public AccommodationRenovation() { }

        public AccommodationRenovation(int id, DateTime startDate, DateTime endDate, string description, RenovationStatus status, Accommodation accommodation, AccommodationReservation reservation)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            Status = status;
            AccommodationForRenovation = accommodation;
            Reservation = reservation;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), StartDate.ToString(CultureInfo.InvariantCulture), EndDate.ToString(CultureInfo.InvariantCulture),
                Description, Status.ToString(), AccommodationForRenovation.Id.ToString(), Reservation.Id.ToString()
            };
            return csvValues;
        }

        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _reservationService = new();

        public void FromCsv(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartDate = DateTime.ParseExact(values[1], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            EndDate = DateTime.ParseExact(values[2], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Description = values[3];
            Status = (RenovationStatus)Enum.Parse(typeof(RenovationStatus), values[4]);
            AccommodationForRenovation = _accommodationService.GetById(Convert.ToInt32(values[5]));
            Reservation = _reservationService.GetById(Convert.ToInt32(values[6]));
        }
    }
}
