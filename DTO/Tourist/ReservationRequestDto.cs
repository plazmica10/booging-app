using BookingApp.WPF.ViewModel.Tourist;
using System.Collections.Generic;

namespace BookingApp.DTO.Tourist
{
    public class ReservationRequestDto
    {
        public int TourId { get; set; }
        public int TouristId { get; set; }
        public int VoucherId { get; set; }
        public List<ReservationDto> Reservations { get; set; } = new();

        public ReservationRequestDto(int tourId, int touristId, int voucherId, List<ItemReservationViewModel> reservations)
        {
            TourId = tourId;
            TouristId = touristId;
            VoucherId = voucherId;
            foreach (var reservationViewModel in reservations)
            {
                Reservations.Add(new ReservationDto(reservationViewModel));
            }
        }
    }
}