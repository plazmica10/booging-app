using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class ReviewGuestDto
    {
        public string GuestName { get; set; } = "";
        public string AccommodationPhoto { get; set; } = "";
        public string AccommodationName { get; set; } = "";
        public string AccommodationLocation { get; set; } = "";
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
