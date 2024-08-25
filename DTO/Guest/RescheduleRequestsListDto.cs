using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Guest
{
    public class RescheduleRequestsListDto
    {
        public int RequestId { get; set; }
        public string AccommodationName { get; set; }
        public int GuestCount { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public  DateTime NewCheckIn { get; set; }
        public DateTime NewCheckOut { get; set; }
        public string Status { get; set; }
        public RescheduleRequestsListDto() { }
        public RescheduleRequestsListDto(int id,string accommodationName,int guests, DateTime checkIn, DateTime checkOut, DateTime newCheckIn,
            DateTime newCheckOut, string status)
        {
            RequestId = id;
            AccommodationName = accommodationName;
            GuestCount = guests;
            CheckIn = checkIn;
            CheckOut = checkOut;
            NewCheckIn = newCheckIn;
            NewCheckOut = newCheckOut;
            Status = status;
        }
    }
}
