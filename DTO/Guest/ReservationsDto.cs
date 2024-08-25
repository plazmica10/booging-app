using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Guest
{
    public class ReservationsDto
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<string> Images { get; set; }
        public User Owner { get; set; }
        public int GuestCount { get; set; }
        public bool HasGuestReviewed { get; set; }
        public ReservationsDto(int id, string accommodationName, string city, string country, DateTime checkIn, DateTime checkOut, List<string> images,int accommodationId,User owner, int guestCount,bool reviewed)
        {
            Id = id;
            AccommodationName = accommodationName;
            City = city;
            Country = country;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Images = images;
            AccommodationId = accommodationId;
            Owner = owner;
            GuestCount = guestCount;
            HasGuestReviewed = reviewed;
        }
    }
}
