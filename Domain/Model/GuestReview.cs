using System;
using System.Collections.Generic;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class GuestReview : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int ReservationId { get; set; }
        public string Comment { get; set; }
        public int Cleanliness { get; set; }
        public int RuleFollowing { get; set; }
        public string GetHeader() { return "Id|AccommodationId|GuestId|ReservationId|Comment|Cleanliness|RuleFollowing"; }

        public GuestReview() { }

        public GuestReview(int id, int accommodationId, int guestId, int reservationId, string comment, int cleanliness, int ruleFollowing)
        {
            Id = id;
            AccommodationId = accommodationId;
            GuestId = guestId;
            ReservationId = reservationId;
            Comment = comment;
            Cleanliness = cleanliness;
            RuleFollowing = ruleFollowing;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), AccommodationId.ToString(), GuestId.ToString(), ReservationId.ToString(), Comment, Cleanliness.ToString(), RuleFollowing.ToString()
            };
            return csvValues;
        }

        public void FromCsv(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
            Comment = values[4];
            Cleanliness = Convert.ToInt32(values[5]);
            RuleFollowing = Convert.ToInt32(values[6]);
        }
        public double GetRating() { return Math.Round((double)(Cleanliness + RuleFollowing) / 2, 1); }
    }
}
