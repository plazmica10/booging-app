using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class StartedTourViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        private readonly TourManagementService _tourManagementService = new();
        private readonly TourInfoService _tourInfoService = new();
        private readonly TourReservationService _tourReservationService = new();
        private readonly TourSearchService _tourSearchService = new();
        private readonly PictureService _pictureService = new();

        private int TourId { get; set; } = -1;

        public ItemTourReservationViewModel? TourReservationsSelected { get; set; }

        private ItemPeakPointViewModel _peakPointsSelected;
        public ItemPeakPointViewModel PeakPointsSelected { get => _peakPointsSelected; set { _peakPointsSelected = value; UpdateCurrentPeakPoint(); OnPropertyChanged(nameof(PeakPointsSelected)); } }
        public ObservableCollection<ItemTourReservationViewModel> TourReservations { get; set; } = new();
        public ObservableCollection<ItemPeakPointViewModel> PeakPoints { get; set; } = new();
        public List<int> PeakPointsArrived { get; set; } = new List<int>();
        public List<string> PictureLocations { get; set; } = new();

        public bool IsCurrent { get; set; }
        private int _peopleArrived;
        public int PeopleArrived { get => _peopleArrived; set { _peopleArrived = value; OnPropertyChanged(nameof(PeopleArrived)); } }

        private string _tourName = "";
        public string TourName { get => _tourName; set { _tourName = value; OnPropertyChanged(nameof(TourName)); } }
        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
        private int _numTourists;
        public int MaxTourists { get => _numTourists; set { _numTourists = value; OnPropertyChanged(nameof(MaxTourists)); } }
        private string _duration;
        public string Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }
        private string _location = "";
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }
        private string _language = "";
        public string Language { get => _language; set { _language = value; OnPropertyChanged(nameof(Language)); } }
        public string StartsAt { get => _startsAt; set { _startsAt = value; OnPropertyChanged(nameof(StartsAt)); } }
        private string _startsAt;

        private string _pictureLocation = "";
        public string PictureLocation { get => _pictureLocation; set { _pictureLocation = value; OnPropertyChanged(nameof(PictureLocation)); } }

        private int _currentPictureIndex = 0;

        public ICommand NextPictureCommand => new FunctionCommand(NextPictureAction);
        public ICommand MarkPresentCommand { get; set; }
        public ICommand FinishTourCommand { get; set; }

        public StartedTourViewModel(User currentUser, NavigationService navigationService, int tourId)
        {
            CurrentUser = currentUser;
            NavigationService = navigationService;
            TourId = tourId;

            MarkPresentCommand = new FunctionCommand(MarkPresentAction);
            FinishTourCommand = new FunctionCommand(FinishTourAction);

            Tour? tour = _tourSearchService.GetById(tourId);
            if (tour == null) { return; }

            TourName = tour.Name;
            Description = tour.Description;
            MaxTourists = tour.MaxTourists;
            Duration = tour.Duration + "h";
            Location = tour.Location.ToSmallString();
            Language = tour.Language.Name;
            StartsAt = $"{tour.StartsAt:yyyy-MM-dd HH:mm}";
            PictureLocations = _pictureService.GetPictureLocations(tour.PictureIds);
            PictureLocation = PictureLocations.Count >= 1 ? PictureLocations[0] : "";

            foreach (string peakPoint in _tourInfoService.GetTourPeakPoints(TourId))
            {
                PeakPoints.Add(new ItemPeakPointViewModel(peakPoint, 0));
            }

            foreach (TourReservation tourReservation in _tourReservationService.GetTourReservationsByTourId(TourId))
            {
                TourReservations.Add(new ItemTourReservationViewModel(tourReservation));

            }

            if (tour.CurrentPeakPointIndex != -1)
            {
                _peakPointsSelected = PeakPoints[tour.CurrentPeakPointIndex];
            }

            UpdateCounters();
        }

        private void NextPictureAction()
        {
            if (PictureLocations != null && PictureLocations.Count > 0){ _currentPictureIndex = (_currentPictureIndex + 1) % PictureLocations.Count; PictureLocation = PictureLocations[_currentPictureIndex]; }
           
        }

        private void UpdateCurrentPeakPoint()
        {
            if (_peakPointsSelected == null) { return; }
            _tourManagementService.UpdateTourPeakPoint(TourId, _peakPointsSelected.Name);
        }

        private void UpdateCounters()
        {
            foreach (ItemPeakPointViewModel peakPoint in PeakPoints){ peakPoint.PeopleArrived = 0; }
            
            foreach (ItemTourReservationViewModel reservations in TourReservations)
            {
                foreach (ItemPeakPointViewModel peakPoint in PeakPoints) { if (peakPoint.Name == reservations.ArrivedPeakPoint) { peakPoint.PeopleArrived++; } }
              
            }
        }

        private void MarkPresentAction()
        {
            if (TourReservationsSelected == null || PeakPointsSelected == null) return;
            if (_tourManagementService.HasPersonArrived(TourReservationsSelected.Id, PeakPointsSelected.Name) == TourServiceReturn.Success)
            {
                TourReservationsSelected.Arrived = true;
                TourReservationsSelected.ArrivedPeakPoint = PeakPointsSelected.Name;
                UpdateCounters();
            }
        }

        private void FinishTourAction()
        {
            switch(_tourManagementService.FinishTour(CurrentUser.Id, TourId))
            {
                case TourServiceReturn.NotFoundTour: MessageBox.Show("tour not found / can't finish"); break;
                case TourServiceReturn.Success: NavigationService.Navigate(new ToursView(CurrentUser, NavigationService)); break;
            }
        }
    }
}
