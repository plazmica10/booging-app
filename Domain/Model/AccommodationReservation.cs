using System;
using System.Globalization;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum ReservationStatus { Canceled, Reserved, Completed, InProgress, Reviewed, PendingReview, Renovation, RenovationOver }
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int OwnerId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestCount { get; set; }
        public ReservationStatus Status { get; set; }
        public bool HasGuestReviewed { get; set; }

        public string GetHeader() { return "Id|AccommodationId|GuestId|OwnerId|CheckIn|CheckOut|Status|GuestCount|HasGuestReviewed"; }

        public AccommodationReservation() { }

        public AccommodationReservation(int id, int accommodationId, int guestId, int ownerId, DateTime checkIn, DateTime checkOut, ReservationStatus status, int guestCount,bool hasGuestReviewed)
        {
            Id = id;
            AccommodationId = accommodationId;
            GuestId = guestId;
            OwnerId = ownerId;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Status = status;
            GuestCount = guestCount;
            HasGuestReviewed = hasGuestReviewed;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), AccommodationId.ToString(), GuestId.ToString(), OwnerId.ToString(),
                CheckIn.ToString(CultureInfo.InvariantCulture), CheckOut.ToString(CultureInfo.InvariantCulture),
                Status.ToString(), GuestCount.ToString(),HasGuestReviewed.ToString()
            };
            return csvValues;
        }

        public void FromCsv(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            OwnerId = Convert.ToInt32(values[3]);
            CheckIn = DateTime.ParseExact(values[4], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            CheckOut = DateTime.ParseExact(values[5], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Status = (ReservationStatus)Enum.Parse(typeof(ReservationStatus), values[6]);
            GuestCount = Convert.ToInt32(values[7]);
            HasGuestReviewed = Convert.ToBoolean(values[8]);
        }
    }
}