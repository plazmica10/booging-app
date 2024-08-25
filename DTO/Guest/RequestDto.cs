using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Guest
{
    public class RequestDto
    {
        public string  Owner { get; set; }
        public string AccommodationName { get; set; } = "";
        public  string City { get; set; } = "";
        public  string Country { get; set; } = "";
        public int GuestCount { get; set; }
        public DateTime OldStartDate { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public string OwnersComment { get; set; } = "";
        public string Status { get; set; }
        public  List<string> Images { get; set; }

        public RequestDto() { }

        public RequestDto(string owner, string accommodationName, string city, string country, int guestCount,
            DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate,
            string ownersComment, string status, List<string> images)
        {
            Owner = owner;
            AccommodationName = accommodationName;
            City = city;
            Country = country;
            GuestCount = guestCount;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            OwnersComment = ownersComment;
            Status = status;
            Images = images;
        }

    }
}
