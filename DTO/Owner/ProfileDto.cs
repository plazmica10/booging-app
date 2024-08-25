using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Photo { get; set; } = "";
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public bool IsSuperUser { get; set; }
    }
}
