using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using PopupErrorWindow = BookingApp.WPF.View.Guide.PopupErrorWindow;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class CreateViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;

        private readonly PictureService _pictureService = new();
        private readonly TourManagementService _tourManagementService = new();
        private readonly TourStatisticsService _tourStatisticsService = new();

        public ObservableCollection<DateTime> DatesAndTimes { get; set; } = new();

        public ObservableCollection<string> PeakPoints { get; set; } = new();

        public ObservableCollection<string> PictureLocations { get; set; } = new();

        public List<string> PictureIds { get; set; } = new();
        
        private string _tourName = "";
        public string TourName { get => _tourName; set { _tourName = value; OnPropertyChanged(nameof(TourName)); } }
        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
        private int _numTourists;
        public int NumTourists { get => _numTourists; set { _numTourists = value; OnPropertyChanged(nameof(NumTourists)); } }
        private int _duration ;
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }
        public DateTime? SelectedDate { get; set; } = DateTime.Now.Date;
        private string _selectedTime = "13:00:00";
        public string SelectedTime { get => _selectedTime; set { _selectedTime = value; OnPropertyChanged(nameof(SelectedTime)); } }
        private string _peakPoint = "";
        public string PeakPoint { get => _peakPoint; set { _peakPoint = value; OnPropertyChanged(nameof(PeakPoint)); } }

        private bool _isPageEnabled = true;
        public bool IsPageEnabled { get=> _isPageEnabled; set{ if (_isPageEnabled != value) { _isPageEnabled=value; OnPropertyChanged(nameof(IsPageEnabled));}}}
      
        private bool _isLocationOrLanguageSelected = false;
        public bool IsLocationOrLanguageSelected
        {
            get => _isLocationOrLanguageSelected;
            set
            {
                if (_isLocationOrLanguageSelected != value)
                {
                    _isLocationOrLanguageSelected = value;
                    OnPropertyChanged(nameof(IsLocationOrLanguageSelected));
                    OnPropertyChanged(nameof(IsButtonEnabled));
                }
            }
        }

        private Language? _selectedLanguage;
        public Language? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        private bool? _selectedLanguageEnabled = true;
        public bool? SelectedLanguageEnabled
        {
            get => _selectedLanguageEnabled;
            set
            {
                _selectedLanguageEnabled = value;
                OnPropertyChanged(nameof(SelectedLanguageEnabled));

            }
        }

        private Location? _selectedLocation;
        public Location? SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        private bool? _selectedLocationEnabled = true;
        public bool? SelectedLocationEnabled
        {
            get => _selectedLocationEnabled;
            set
            {
                _selectedLocationEnabled = value;
                OnPropertyChanged(nameof(SelectedLocationEnabled));

            }
        }
        public static event Action RequestFocusNameTextBox;


        public bool IsButtonEnabled => !IsLocationOrLanguageSelected;
        public ICommand CreateCommand => new FunctionCommand(CreateAction);
        public ICommand AddDateCommand => new FunctionCommand(AddDateAction);
        public ICommand AddPeakPointCommand => new FunctionCommand(AddPeakPointAction);
        public ICommand AddPicturesCommand => new FunctionCommand(AddPicturesAction);
        public ICommand IncreaseMaxTouristsCommand => new RelayCommand<string>(IncreaseMaxTourists);
        public ICommand DecreaseMaxTouristsCommand => new RelayCommand<string>(DecreaseMaxTourists);
        public ICommand IncreaseDurationCommand => new RelayCommand<string>(IncreaseDuration);
        public ICommand DecreaseDurationCommand => new RelayCommand<string>(DecreaseDuration);
        public ICommand RemovePictureCommand => new RelayCommand<string>(RemovePicture);

        public ICommand AcceptBestLocationCommand => new RelayCommand<string>(AcceptBestLocationAction);
        public ICommand AcceptBestLanguageCommand => new RelayCommand<string>(AcceptBestLanguageAction);

        private bool _isLanguageListBoxEnabled = true;
        public bool IsLanguageListBoxEnabled
        {
            get => _isLanguageListBoxEnabled;
            set
            {
                if (_isLanguageListBoxEnabled != value)
                {
                    _isLanguageListBoxEnabled = value;
                    OnPropertyChanged(nameof(IsLanguageListBoxEnabled));
                }
            }
        }
        private bool _isLocationListBoxEnabled = true;
        public bool IsLocationListBoxEnabled
        {
            get => _isLocationListBoxEnabled;
            set
            {
                if (_isLocationListBoxEnabled != value)
                {
                    _isLocationListBoxEnabled = value;
                    OnPropertyChanged(nameof(IsLocationListBoxEnabled));
                }
            }
        }
        private void AcceptBestLocationAction(string obj)
        {
            Location? bestLocation = _tourStatisticsService.GetMostRequestedLocation();
            if (bestLocation != null)
            {
                SelectedLocation = bestLocation;
                IsLocationOrLanguageSelected = true;
                SelectedLocationEnabled = false;
                IsLocationListBoxEnabled = false;
            }
            else{ ShowErrorPopup("Not available"); }
            

        }

        private void AcceptBestLanguageAction(string obj)
        {
            Language? bestLanguage =_tourStatisticsService.GetMostRequestedLanguage();
            if (bestLanguage != null)
            {
                SelectedLanguage = bestLanguage;
                IsLocationOrLanguageSelected = true;
                SelectedLanguageEnabled = false;
                IsLanguageListBoxEnabled = false;
            }
            else { ShowErrorPopup("Not available"); }
           
        }
        private readonly Action _focusNameTextBox;
        public CreateViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            NavigationService = navigationService;

        }

        private void RemovePicture(string selectedPicture)
        {
            PictureLocations.Remove(selectedPicture);
        }
        
        private void IncreaseMaxTourists(object obj)
        {
            int increment = Convert.ToInt32(obj);
            NumTourists += increment;
        }

        private void DecreaseMaxTourists(object obj)
        {
            int decrement = Convert.ToInt32(obj);
            if (NumTourists > 0)
                NumTourists -= decrement;
        }

        private void IncreaseDuration(object obj)
        {
            int increment = Convert.ToInt32(obj);
            Duration += increment;
        }

        private void DecreaseDuration(object obj)
        {
            int decrement = Convert.ToInt32(obj);
            if (Duration > 0)
                Duration -= decrement;
        }
        public void AddDateAction()
        {
            if (SelectedDate == null) { return; }
            if (!int.TryParse(HourText, out int hour) || !int.TryParse(MinuteText, out int minute) || !int.TryParse(SecondText, out int second))
            {
                ShowErrorPopup("Invalid time format"); // Display error if the time format is invalid
                return;
            }

            DateTime date = (DateTime)SelectedDate;
            date = date.AddHours(hour).AddMinutes(minute).AddSeconds(second);
            DatesAndTimes.Add(date);
        }

        public void AddPeakPointAction()
        {
            if (string.IsNullOrEmpty(PeakPoint)) { return; }
            PeakPoints.Add(PeakPoint);
        }

        public void AddPicturesAction()
        {
            PictureLocations.Clear(); 
            PictureIds.Clear();

            List<string>? pictureIds = _pictureService.CreatePictures();
            if (pictureIds == null) { return; }
            PictureIds.AddRange(pictureIds);
            List<string> pictureLocations = _pictureService.GetPictureLocations(pictureIds);
            foreach (string pictureLocation in pictureLocations){ PictureLocations.Add(pictureLocation); }
           
        }

        public void ShowErrorPopup(string text, string title = "invalid input")
        {
            IsPageEnabled = false; 

            var popupWindow = new PopupErrorWindow(title, text);
            popupWindow.Closed += (sender, args) => { IsPageEnabled = true; RequestFocusNameTextBox?.Invoke(); };
            popupWindow.Show();
        }

        public void ShowSuccessPopup(string text, string title = "Success")
        {
            IsPageEnabled = false; 

            var popupWindow = new PopupSuccessWindow(title, text);
            popupWindow.Closed += (sender, args) => { IsPageEnabled = true; RequestFocusNameTextBox?.Invoke(); };
            popupWindow.Show();
        }


        public void CreateAction()
        {
            if (_numTourists == 0) { ShowErrorPopup("Max Tourists not set"); return; }
            if (_duration == 0) { ShowErrorPopup("Duration not set"); return; }
            if (DatesAndTimes.Count == 0) { ShowErrorPopup("Dates not set"); return; }
            if (PeakPoints.Count <= 1) { ShowErrorPopup("Must have at least 2 peak points"); return; }

            if (SelectedLocation == null) { ShowErrorPopup("Location not selected"); return; }
            if (SelectedLanguage == null) { ShowErrorPopup("Language not selected"); return; }

            CreateTourRequestDto request = new CreateTourRequestDto(CurrentUser.Id, _tourName, SelectedLocation, _description,
            SelectedLanguage, _numTourists, PeakPoints.ToList(), DatesAndTimes.ToList(), _duration, PictureIds);

            switch (_tourManagementService.CreateTour(request))
            {
                case TourServiceReturn.Success: ShowSuccessPopup("Tour created!");break;
                   
                case TourServiceReturn.InvalidLanguageId:ShowErrorPopup("Invalid language");break;
                    
                case TourServiceReturn.InvalidLocationId:ShowErrorPopup("Invalid location"); break;

            }
        }
        private string _hourText = "";
        public string HourText
        {
            get => _hourText;
            set
            {
                if (value.Length <= 2 && int.TryParse(value, out int number))
                {
                    _hourText = value.PadLeft(2, '0');
                    OnPropertyChanged(nameof(HourText));
                }
            }
        }

        private string _minuteText = "";
        public string MinuteText
        {
            get => _minuteText;
            set
            {
                if (value.Length <= 2 && int.TryParse(value, out int number))
                {
                    _minuteText = value.PadLeft(2, '0');
                    OnPropertyChanged(nameof(MinuteText));
                }
            }
        }

        private string _secondText = "";
        public string SecondText
        {
            get => _secondText;
            set
            {
                if (value.Length <= 2 && int.TryParse(value, out int number))
                {
                    _secondText = value.PadLeft(2, '0');
                    OnPropertyChanged(nameof(SecondText));
                }
            }
        }
    }
}
