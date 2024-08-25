using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AnywhereSearchViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService = new AccommodationService();
        private UserService _userService = new UserService();
        private AccommodationReservationService _reservationService = new AccommodationReservationService();
        public NavigationService NavigationService;
        public AnywhereSearchViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
        }

        #region bindings
        public event Action Done;
        public User CurrentUser;

        private ObservableCollection<AccommodationsSearchDto> _accommodations;
        public ObservableCollection<AccommodationsSearchDto> Accommodations { get => _accommodations; set { _accommodations = value; OnPropertyChanged(nameof(Accommodations)); } }

        public ICommand SearchCommand => new FunctionCommand(SearchAccommodations);
        public ICommand ReserveCommand => new FunctionCommand(ReserveAccommodation);

        private DateTime _checkIn = DateTime.Today.AddDays(1);
        public DateTime CheckIn { get => _checkIn; set { _checkIn = value; OnPropertyChanged(nameof(CheckIn)); } }
        private DateTime _checkOut =  DateTime.Today.AddDays(2);
        public DateTime CheckOut { get => _checkOut; set { _checkOut = value; OnPropertyChanged(nameof(CheckOut)); } }
        private int _guestCount=1;
        public int GuestCount { get => _guestCount; set { _guestCount = value; OnPropertyChanged(nameof(GuestCount)); } }
        private int _stayLength=1;
        public int StayLength { get => _stayLength; set { _stayLength = value; OnPropertyChanged(nameof(StayLength)); } }

        private AccommodationsSearchDto _selectedAccommodation;
        public AccommodationsSearchDto SelectedAccommodation { get => _selectedAccommodation; set { _selectedAccommodation = value; OnPropertyChanged(nameof(SelectedAccommodation)); } }
        #endregion
        public void SearchAccommodations()
        {
            Accommodations = new ObservableCollection<AccommodationsSearchDto>(_accommodationService.GetAllAvailableAccommodations(CheckIn,CheckOut,GuestCount,StayLength));
            if (Accommodations.Count == 0)
            {
                MessageBox.Show("No available accommodations for selected dates.");
            }
        }

        public void ReserveAccommodation()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Haven't finished reservation process.");
                return;
            }
            var accommodation = _accommodationService.GetById(SelectedAccommodation.Id);
            if (!AccommodationUtils.IsReservationValid(new Tuple<DateTime, DateTime>(CheckIn, CheckOut), GuestCount, accommodation.MaxGuests)) return;
            var user = _userService.GetById(CurrentUser.Id);
            if (user.BonusPoints >= 1)
                user.BonusPoints--;
            _userService.Update(user);
            _reservationService.Save(new AccommodationReservation {AccommodationId = accommodation.Id, GuestId = CurrentUser.Id, OwnerId = accommodation.OwnerId, CheckIn = CheckIn, CheckOut = CheckOut, Status = ReservationStatus.Reserved, GuestCount = GuestCount });
            ShowDialogue(accommodation.Name);
            Done?.Invoke();
        }

        public void ShowDialogue(string accommodationName)
        {
            var result = MessageBox.Show($"Successful reservation for {accommodationName}! Would you like to view the reservation?", "Reservation Successful", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ReservationsDto reservation = _accommodationService.GetReservationsDto(CurrentUser.Id, false).Last();
                NavigationService.Navigate(new ReservationSummaryView(CurrentUser, reservation, NavigationService));
            }
        }
        

    }
}
