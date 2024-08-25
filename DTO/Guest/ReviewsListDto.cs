using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Guest
{
    public class ReviewsListDto
    {
        public int ReviewId { get; set; }
        public string OwnerName { get; set; }
        public string AccommodationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public double Rating { get; set; }

        public ReviewsListDto(int reviewId,string ownerName, string accommodationName, string city, string country,
            DateTime checkIn, DateTime checkOut, double rating)
        {
            ReviewId = reviewId;
            OwnerName = ownerName;
            AccommodationName = accommodationName;
            City = city;
            Country = country;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Rating = rating;
        }
    }
}
