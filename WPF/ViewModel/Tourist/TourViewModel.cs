using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly PictureService _pictureService = new();
        private readonly TourSearchService _tourSearchService = new();

        // tour info
        private readonly Tour _tour;
        public string Name => _tour.Name;
        public string Description => _tour.Description;
        public string Location => _tour.Location.ToSmallString();
        public string Language => _tour.Language.ToReadableString();
        public string MaxTourists => _tour.MaxTourists.ToString();
        public string SpaceLeft => _tour.SpaceLeft.ToString();
        public string StartsAt => _tour.StartsAt.ToString("yyyy-MM-dd HH:mm:ss");
        public string Duration => _tour.Duration.ToString() + "h";
        public string StatusText { get; set; }
        public List<string> PeakPoints { get; set; } = new();


        // pictures
        private string _pictureLocation = "";
        public string PictureLocation { get => _pictureLocation; set { _pictureLocation = value; OnPropertyChanged(nameof(PictureLocation)); } }
        private int _currentPictureLocationIndex;
        public int CurrentPictureLocationIndex
        {
            get => _currentPictureLocationIndex;
            set
            {
                // if same value do nothing
                if (_currentPictureLocationIndex == value) { return; }

                // cap min
                if (value < 0) { _currentPictureLocationIndex = 0; }

                // cap max
                int max = PictureLocations.Count - 1;
                if (value >= max) { _currentPictureLocationIndex = max; }

                // set picture index
                _currentPictureLocationIndex = value;
                OnPropertyChanged(nameof(CurrentPictureLocationIndex));

                // set picture
                PictureLocation = PictureLocations[_currentPictureLocationIndex];
                PictureScrollLeftCommand.RaiseCanExecuteChanged();
                PictureScrollRightCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility PicturesSliderVisibility { get; set; } = Visibility.Collapsed;

        private List<string> _pictureLocations = new();
        public List<string> PictureLocations
        {
            get => _pictureLocations;
            set
            {
                _pictureLocations = value;
                PictureSliderMin = 0;
                PictureSliderMax = _pictureLocations.Count - 1;
                if (PictureSliderMin != PictureSliderMax) { PicturesSliderVisibility = Visibility.Visible; }
            }
        }
        public RelayCommand<object> PictureScrollLeftCommand { get; }
        public RelayCommand<object> PictureScrollRightCommand { get; }
        public void ScrollLeft(object parameter) { CurrentPictureLocationIndex--; }
        public void ScrollRight(object parameter) { CurrentPictureLocationIndex++; }
        public bool ScrollLeftCanExecute(object parameter) { return CurrentPictureLocationIndex != 0; }
        public bool ScrollRightCanExecute(object parameter) { return CurrentPictureLocationIndex != PictureLocations.Count - 1; }
        public int PictureSliderMin { get; set; }
        public int PictureSliderMax { get; set; }

        public TourViewModel(User currentUser, NavigationService navigationService, int tourId)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            // setup tour
            _tour = _tourSearchService.GetById(tourId) ?? new Tour();


            // setup picture
            PictureLocations = _pictureService.GetPictureLocations(_tour.PictureIds);
            PictureLocation = PictureLocations.Count > 0 ? PictureLocations[0] : "";

            // status text
            switch (_tour.Status)
            {
                case TourStatus.NotStarted: StatusText = "\ud83d\udea5 Not Started "; break;
                case TourStatus.Started:    StatusText = "\u25b6\ufe0f Started";      break;
                case TourStatus.Canceled:   StatusText = "\u274c Canceled";           break;
                case TourStatus.Finished:   StatusText = "\ud83c\udfc1 Finished";     break;
                default:                    StatusText = "...";                       break;
            }

            PictureScrollLeftCommand = new RelayCommand<object>(ScrollLeft, ScrollLeftCanExecute);
            PictureScrollRightCommand = new RelayCommand<object>(ScrollRight, ScrollRightCanExecute);

            // custom peak points
            for (int i = 0; i < _tour.PeakPoints.Count; i++)
            {
                PeakPoints.Add((i+1) + ". " + _tour.PeakPoints[i] + (i == _tour.CurrentPeakPointIndex ? " \ud83d\udc48" : ""));
            }

            ReserveCommand = new FunctionCommand(ReserveAction);
        }

        public ICommand ReserveCommand { get; }
        private void ReserveAction() { MainNavigationService.Navigate(new ReserveView(CurrentUser, MainNavigationService, _tour.Id)); }
    }
}
