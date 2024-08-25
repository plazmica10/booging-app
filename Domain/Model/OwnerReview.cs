using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class OwnerReview : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int GuestId { get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
        public string Comment { get; set; }
        public  int OwnerRating { get; set; }
        public int Cleanliness { get; set; }
        public List<string> Photos { get; set; }
        public string GetHeader()
        {
            return "Id|AccommodationId|GuestId|OwnerId|ReservationId|Comment|OwnerRating|Cleanliness|Photos";
        }
        public OwnerReview() { }

        public OwnerReview(int id, int accommodationId,int guestId, int ownerId, int reservationId, string comment, int ownerRating,
            int cleanliness,List<string> photos)
        {
            Id = id;
            AccommodationId = accommodationId;
            GuestId = guestId;
            OwnerId = ownerId;
            ReservationId = reservationId;
            Comment = comment;
            OwnerRating = ownerRating;
            Cleanliness = cleanliness;
            Photos = photos;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                OwnerId.ToString(),
                ReservationId.ToString(),
                Comment,
                OwnerRating.ToString(),
                Cleanliness.ToString(),
                string.Join(",", Photos)
            };
            return csvValues;
        }

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            AccommodationId = Convert.ToInt32(values[i++]);
            GuestId = Convert.ToInt32(values[i++]);
            OwnerId = Convert.ToInt32(values[i++]);
            ReservationId = Convert.ToInt32(values[i++]);
            Comment = values[i++];
            OwnerRating = Convert.ToInt32(values[i++]);
            Cleanliness = Convert.ToInt32(values[i++]);
            Photos = values[i++].Split(',').ToList();
        }
    }
}
