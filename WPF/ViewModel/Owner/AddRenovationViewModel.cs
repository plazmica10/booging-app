using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AddRenovationViewModel : ViewModelBase
    {
        #region Bindings

        private string _currentPhoto = "";
        public string? CurrentPhoto { get => _currentPhoto; set { if (value != _currentPhoto) { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } } }

        private int _expectedDuration = 1;
        public int ExpectedDuration { get => _expectedDuration; set { if (value != _expectedDuration) { _expectedDuration = value; OnPropertyChanged(nameof(ExpectedDuration)); } } }

        private DateTime _startDate = DateTime.Today.AddDays(1);
        public DateTime StartDate { get => _startDate; set { if (_startDate != value) { _startDate = value; OnPropertyChanged(nameof(StartDate)); } } }
        
        private DateTime _endDate = DateTime.Today.AddDays(2);
        public DateTime EndDate { get => _endDate; set { if (_endDate != value) { _endDate = value; OnPropertyChanged(nameof(EndDate)); } } }

        private string _description = "";
        public string Description { get => _description; set { if (value != _description) { _description = value; OnPropertyChanged(nameof(Description)); } } }

        public Tuple<DateTime, DateTime>? SelectedDate { get; set; }

        private ObservableCollection<Tuple<DateTime, DateTime>> _searchedDates = new();
        public ObservableCollection<Tuple<DateTime, DateTime>> SearchedDates { get => _searchedDates; set { _searchedDates = value; OnPropertyChanged(nameof(SearchedDates)); } }


        #endregion


        public User CurrentUser;
        public NavigationService NavService;
        public AccommodationDTO Accommodation { get; set; }

        private readonly AccommodationReservationService _reservationService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationRenovationService _renovationService = new();


        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ICommand BackCommand => new FunctionCommand(() => NavService.GoBack());
        public ICommand SearchDatesCommand => new FunctionCommand(SearchDates);
        public ICommand SubmitCommand => new FunctionCommand(Submit);

        public AddRenovationViewModel(User user, NavigationService navService, AccommodationDTO accommodation)
        {
            CurrentUser = user;
            NavService = navService;
            Accommodation = accommodation;
            CurrentPhoto = accommodation.Photos.FirstOrDefault();
        }

        private void Submit()
        {
            if (SelectedDate == null)
            {
                MessageBox.Show("Please select a date");
                return;
            }
            Accommodation? accommodation = _accommodationService.GetById(Accommodation.Id);
            if (accommodation == null) return;

            AccommodationReservation reservation = new()
            {
                AccommodationId = accommodation.Id, CheckIn = SelectedDate.Item1, CheckOut = SelectedDate.Item2,
                GuestCount = 1, GuestId = CurrentUser.Id, OwnerId = CurrentUser.Id, Status = ReservationStatus.Renovation
            };
            _reservationService.Save(reservation);

            AccommodationRenovation renovation = new()
            {
                StartDate = SelectedDate.Item1, EndDate = SelectedDate.Item2, Description = Description,
                Status = RenovationStatus.Pending, AccommodationForRenovation = accommodation, Reservation = reservation
            };
            _renovationService.SaveRenovation(renovation);

            MessageBox.Show("Renovation added successfully.");
            NavService.GoBack();
        }

        private void SearchDates()
        {
            _searchedDates.Clear();
            if (StartDate >= EndDate) return;
            if (StartDate <= DateTime.Today || EndDate <= DateTime.Today) return;
            SearchedDates = new ObservableCollection<Tuple<DateTime, DateTime>>(_reservationService.GetAvailableDates(Accommodation.Id, StartDate, EndDate, ExpectedDuration));
        }

        private void NextPhoto()
        {
            if (CurrentPhoto == null) return;
            if (Accommodation.Photos.Count == 0) return;
            int index = Accommodation.Photos.IndexOf(CurrentPhoto);
            if (index == Accommodation.Photos.Count - 1)
                CurrentPhoto = Accommodation.Photos[0];
            else
                CurrentPhoto = Accommodation.Photos[index + 1];
        }

        private void PrevPhoto()
        {
            if (CurrentPhoto == null) return;
            if (Accommodation.Photos.Count == 0) return;
            int index = Accommodation.Photos.IndexOf(CurrentPhoto);
            if (index == 0)
                CurrentPhoto = Accommodation.Photos.Last();
            else
                CurrentPhoto = Accommodation.Photos[index - 1];
        }
    }
}
