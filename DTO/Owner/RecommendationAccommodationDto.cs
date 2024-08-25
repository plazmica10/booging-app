using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Owner
{
    public class RecommendationAccommodationDto
    {
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; } = "";
        public Location AccommodationLocation { get; set; } = new();
        public string AccommodationPhoto { get; set; } = "";
        public int ReservationsInLastYear { get; set; }
    }
}
