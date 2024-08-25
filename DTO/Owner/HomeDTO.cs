using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public enum HomeDtoKind
    {
        CheckingOut,
        CurrentlyHosting,
        ArrivingSoon,
        PendingReview
    }

    public class HomeDTO
    {
        public int ReservationId { get; set; }
        public string GuestName { get; set; }
        public string GuestPhoto { get; set; }
        public string AccommodationName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public HomeDtoKind ReservationKind { get; set; }
    }
}
