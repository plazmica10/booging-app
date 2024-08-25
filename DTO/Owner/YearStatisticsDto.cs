using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class YearStatisticsDto
    {
        public int Year { get; set; }
        public int Reservations { get; set; }
        public int Cancellations { get; set; }
        public int ReschedulingCount { get; set; }
        public int RenovationCount { get; set; }
    }
}
