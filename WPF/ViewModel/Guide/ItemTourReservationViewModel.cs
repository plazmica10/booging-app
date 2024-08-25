using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemTourReservationViewModel : ViewModelBase
    {
        private readonly TourReservation _tourReservation;

        public int Id => _tourReservation.Id;
        public int TourId => _tourReservation.TourId;
        public int TouristId => _tourReservation.TouristId;
        public string FirstName => _tourReservation.Person.FirstName;
        public string LastName => _tourReservation.Person.LastName;
        public string Email => _tourReservation.Person.Email;
        public int BirthYear => _tourReservation.Person.BirthYear;
        private bool _arrived;
        public bool Arrived { get => _arrived; set { _arrived = value; OnPropertyChanged(nameof(Arrived)); } }
       
        private string _arrivedPeakPoint;
        public string ArrivedPeakPoint { get => _arrivedPeakPoint; set { _arrivedPeakPoint = value; OnPropertyChanged(nameof(ArrivedPeakPoint)); } }
       
        public ItemTourReservationViewModel(TourReservation tourReservation)
        {
            _tourReservation = tourReservation;
            _arrived = tourReservation.Arrived;
            _arrivedPeakPoint = tourReservation.ArrivedPeakPoint;
        }
    }
}
