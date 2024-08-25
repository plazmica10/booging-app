using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class RequestDto
    {
        public int Id { get; set; }
        public string AccommodationName { get; set; } = "";
        public DateTime OldStartDate { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public string OwnersComment { get; set; } = "";
        public RequestStatus Status { get; set; }
        public bool Available { get; set; }
    }
}
