using BookingApp.WPF.ViewModel.Tourist;

namespace BookingApp.DTO.Tourist
{
    public class ReservationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int BirthYear { get; set; }

        public ReservationDto(ItemReservationViewModel reservationViewModel)
        {
            FirstName = reservationViewModel.FirstName;
            LastName = reservationViewModel.LastName;
            Email = reservationViewModel.Email;
            BirthYear = reservationViewModel.BirthYear;
        }
    }
}