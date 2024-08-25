using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Guest
{
    public class RescheduleRequestsPageDto
    {
        public string AccommodationName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<string> Images { get; set; }

        public RescheduleRequestsPageDto(){}

        public RescheduleRequestsPageDto(string accommodationName, string city, string country, DateTime checkIn, DateTime checkOut,
            List<string> images)
        {
            AccommodationName = accommodationName;
            City = city;
            Country = country;
            CheckIn = checkIn;
            CheckOut = checkOut;
            Images = images;
        }
    }
}
