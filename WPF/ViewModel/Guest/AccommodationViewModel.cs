using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using MenuNavigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AccommodationViewModel: ViewModelBase
    {
        private AccommodationReservationService _reservationService = new();
        private UserService _userService = new();
        private AccommodationService _accommodationService = new();
        public NavigationService NavigationService;
        public User CurrentUser { get; set; }

        public AccommodationViewModel(User user, Accommodation accommodation, NavigationService navService)
        {

            CurrentUser = user;
            NavigationService = navService;
            Accommodation = accommodation;
            var owner = new UserService().GetById(accommodation.OwnerId);
            OwnerName = owner.FirstName + " " + owner.LastName;
            SortedImages = new ObservableCollection<string>(Accommodation.Photos.OrderBy(x => x));
            CurrentImage = SortedImages[0];
            SearchCommand = new FunctionCommand(SearchAvailableDates_Click);
            ReserveCommand = new FunctionCommand(MakeReservation_Click);
            PreviousCommand = new FunctionCommand(PreviousImage);
            NextCommand = new FunctionCommand(NextImage);

        }
        #region bindings and commands
        public ICommand SearchCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public int ImageCount { get { return SortedImages?.Count ?? 0; } }
        private string _ownerName { get; set; }
        public string OwnerName { get => _ownerName; set { _ownerName = value; OnPropertyChanged(nameof(OwnerName)); } }
        public Tuple<DateTime, DateTime>? SelectedDate { get; set; }

        private Accommodation _accommodation;
        public Accommodation Accommodation { get => _accommodation; set { _accommodation = value; OnPropertyChanged(nameof(Accommodation)); } }
        private ObservableCollection<Tuple<DateTime, DateTime>> _searchedDates;
        public ObservableCollection<Tuple<DateTime, DateTime>> SearchedDates { get => _searchedDates; set { _searchedDates = value; OnPropertyChanged(nameof(SearchedDates)); } }
        public ReservationDto Reservation { get; set; } = new ReservationDto();
        private string _currentImage;
        public string CurrentImage { get => _currentImage; set { _currentImage = value; OnPropertyChanged(nameof(CurrentImage)); } }
        public ObservableCollection<string> SortedImages { get; set; }
        #endregion
        private void MakeReservation_Click()
        {
            if (!AccommodationUtils.IsReservationValid(SelectedDate, Reservation.GuestCount, Accommodation.MaxGuests)) return;
            var user = _userService.GetById(CurrentUser.Id);
            if(user.BonusPoints >= 1)
                user.BonusPoints--;
            _userService.Update(user);
            _reservationService.Save(new AccommodationReservation { AccommodationId = Accommodation.Id,GuestId = CurrentUser.Id,OwnerId = Accommodation.OwnerId,CheckIn = SelectedDate.Item1,CheckOut = SelectedDate.Item2, Status = ReservationStatus.Reserved,GuestCount = Reservation.GuestCount });
            showDialogue(Accommodation.Name);
        }

        public void showDialogue(string accommodationName)
        {
            var result = MessageBox.Show($"{TranslationSource.Instance["SuccessfulReservation"]} {accommodationName}! {TranslationSource.Instance["ViewReservationPrompt"]}", TranslationSource.Instance["ReservationSuccessful"], MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ReservationsDto reservation = _accommodationService.GetReservationsDto(CurrentUser.Id, false).Last();
                NavigationService.Navigate(new ReservationSummaryView(CurrentUser, reservation, NavigationService));
            }
        }

        private void SearchAvailableDates_Click()
        {
            if (!AccommodationUtils.IsSearchValid(Reservation.CheckIn, Reservation.CheckOut, Reservation.StayLength, Accommodation.MinDays)) return;
            SearchedDates = new ObservableCollection<Tuple<DateTime, DateTime>>(_reservationService.GetAvailableDates(Accommodation.Id, Reservation.CheckIn, Reservation.CheckOut, Reservation.StayLength));
            if (SearchedDates.Count == 0)
            {
                MessageBox.Show(TranslationSource.Instance["NoAvailableDates"]);
            }
        }
        private void PreviousImage() { CurrentImage = ImageControl.GetPreviousImage(SortedImages, CurrentImage); }
        private void NextImage() { CurrentImage = ImageControl.GetNextImage(SortedImages, CurrentImage); }
    }
}
