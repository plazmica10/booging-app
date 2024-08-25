using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ToursTodayViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        public NavigationService ToursNavigationService;
        private Window MainWindow;
        private readonly PictureService _pictureService = new();

        private readonly TourManagementService _tourManagementService = new();
        private readonly TourSearchService _tourSearchService = new();

        public ObservableCollection<ItemTourViewModel> Tours { get; set; } = new();
        public ObservableCollection<Tour> SelectedTourList { get; set; }
        private bool _isPageEnabled = true;

        public bool IsPageEnabled{get=>_isPageEnabled;set{ if (_isPageEnabled != value){ _isPageEnabled = value; OnPropertyChanged(nameof(IsPageEnabled)); } } }
       
        public List<string> PictureLocations { get; set; } = new();

        private string _tourName = "";
        public string TourName { get { return _tourName; } set { _tourName = value; OnPropertyChanged(nameof(TourName)); } }
        private string _description = "";
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged(nameof(Description)); } }
        private int _numTourists;
        public int MaxTourists { get { return _numTourists; } set { _numTourists = value; OnPropertyChanged(nameof(MaxTourists)); } }
        private string _duration;
        public string Duration { get { return _duration; } set { _duration = value; OnPropertyChanged(nameof(Duration)); } }
        private string _location = "";
        public string Location { get { return _location; } set { _location = value; OnPropertyChanged(nameof(Location)); } }
        private string _language = "";
        public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); } }
        private string _startsAt;
        public string StartsAt { get { return _startsAt; } set { _startsAt = value; OnPropertyChanged(nameof(StartsAt)); } }

        private string _pictureLocation = "";
        public string PictureLocation { get { return _pictureLocation; } set { _pictureLocation = value; OnPropertyChanged(nameof(PictureLocation)); } }

        private int _currentPictureIndex = 0;

        public ICommand NextPictureCommand => new FunctionCommand(NextPictureAction);
        public ICommand StartTourCommand => new FunctionCommand(StartTourAction);

        public RelayCommand StackPanelCommand => new RelayCommand(StackPanelAction);

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
    
        public ToursTodayViewModel(User currentUser, NavigationService navigation, NavigationService toursNavigationService, Window mainWindow)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            ToursNavigationService = toursNavigationService;
            MainWindow = mainWindow;
            UpdateTours();

        }

        private void NextPictureAction()
        {
            if (PictureLocations != null && PictureLocations.Count > 0){ _currentPictureIndex = (_currentPictureIndex + 1) % PictureLocations.Count; PictureLocation = PictureLocations[_currentPictureIndex]; }
           
        }

        private void StackPanelAction(object parameter)
        {
            if (parameter is StackPanel stackPanel && stackPanel.DataContext is Tour tour)
            {
                if (SelectedTourList.Contains(tour)){ SelectedTourList.Remove(tour); }
               
                else{ SelectedTourList.Clear(); SelectedTourList.Add(tour); }
               
            }
        }

        private void UpdateTours()
        {
            Tours.Clear();
            foreach (Tour tour in _tourSearchService.GetAllByGuideIdToday(CurrentUser.Id)) { Tours.Add(new ItemTourViewModel(tour)); }
            
        }
        public void ShowErrorPopup(string text, string title = "Invalid Input")
        {
            IsPageEnabled = false;

            var popupWindow = new PopupErrorWindow(title, text);
            popupWindow.Closed += (sender, args) =>
            {
                IsPageEnabled = true;


                MessageBox.Show("Finish the started tour first.");


            };

            popupWindow.ShowDialog();
        }


       
        public void StartTourAction()
        {
            if (ToursSelected == null) { MessageBox.Show("can't start nothing"); return; }

            switch(_tourManagementService.StartTour(CurrentUser.Id, ToursSelected.Id))
            {
                case TourServiceReturn.Success: NavigationService.Navigate(new StartedTourView(CurrentUser, NavigationService, ToursSelected.Id)); break;
                case TourServiceReturn.MyTourAlreadyStarted: ShowErrorPopup("my tour already started"); break;
                case TourServiceReturn.NotFoundTour: ShowErrorPopup("tour not found"); break;
                case TourServiceReturn.NotMyTour: ShowErrorPopup("not my tour"); break;
                case TourServiceReturn.FailedToStartTour: ShowErrorPopup("can't start tour"); break;
            }
        }
       
    }
}
