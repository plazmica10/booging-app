using System;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ToursAllViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        public NavigationService ToursNavigationService;

        private readonly PictureService _pictureService = new();
        private readonly TourManagementService _tourManagementService = new();
        private readonly TourSearchService _tourSearchService = new();
        public ObservableCollection<ItemTourViewModel> Tours { get; set; } = new();
        private bool _isPageEnabled = true;

        public bool IsPageEnabled{ get => _isPageEnabled; set{ if (_isPageEnabled != value){ _isPageEnabled = value; OnPropertyChanged(nameof(IsPageEnabled)); } } }
       
        public List<string> PictureLocations { get; set; } = new();

        private string _tourName = "";
        public string TourName { get => _tourName;  set { _tourName = value; OnPropertyChanged(nameof(TourName)); } }
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

        private ItemTourViewModel? _tourSelected;
        public ItemTourViewModel? ToursSelected
        {
            get => _tourSelected;
            set
            {
                _tourSelected = value;

                if (_tourSelected != null)
                {
                    Tour? tour = _tourSearchService.GetById(_tourSelected.Id);
                    if (tour != null)
                    {
                        TourName = tour.Name;
                        Description = tour.Description;
                        MaxTourists = tour.MaxTourists;
                        Duration = tour.Duration + "h";
                        Location = tour.Location.ToSmallString();
                        Language = tour.Language.Name;
                        StartsAt = $"{tour.StartsAt:yyyy-MM-dd HH:mm}";
                        PictureLocations = _pictureService.GetPictureLocations(tour.PictureIds);
                        PictureLocation = PictureLocations.Count >= 1 ? PictureLocations[0] : "";
                    }
                }

                OnPropertyChanged(nameof(ToursSelected));
            }
        }

        public ICommand NextPictureCommand { get; }
        public ICommand CancelTourCommand { get; }

        public ToursAllViewModel(User currentUser, NavigationService navigation, NavigationService toursNavigationService)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            ToursNavigationService = toursNavigationService;
            CancelTourCommand = new FunctionCommand(CancelTourAction);

            NextPictureCommand = new FunctionCommand(NextPictureAction);

            UpdateTours();
        }
        private void NextPictureAction()
        {
            if (PictureLocations != null && PictureLocations.Count > 0) { _currentPictureIndex = (_currentPictureIndex + 1) % PictureLocations.Count; PictureLocation = PictureLocations[_currentPictureIndex]; }
            
        }
        private void UpdateTours()
        {
            Tours.Clear();
            foreach (Tour tour in _tourSearchService.GetAllByGuideId(CurrentUser.Id)){ Tours.Add(new ItemTourViewModel(tour)); }
            
        }

        public void ShowErrorPopup(string text, string title = "Invalid Input")
        {
            IsPageEnabled = false;

            var popupWindow = new PopupErrorWindow(title, text);
            popupWindow.Closed += (sender, args) =>
            {
                IsPageEnabled = true;


                MessageBox.Show("Cancel the right tour.");


            };

            popupWindow.ShowDialog();
        }



        public void CancelTourAction()
        {
            if (ToursSelected == null) { MessageBox.Show("can't cancel nothing"); return; }

            switch (_tourManagementService.CancelTour(CurrentUser.Id, ToursSelected.Id, DateTime.Now.AddYears(1)))
            {
                case TourServiceReturn.Success: ShowErrorPopup("tour canceled"); UpdateTours(); break;
                case TourServiceReturn.FailedToCancelTour: ShowErrorPopup("can't cancel tour because its finished"); break;
                case TourServiceReturn.FailedToCancelTour48H: ShowErrorPopup("can't cancel tour < 48h before begin"); break;
            }
        }
    }
}
