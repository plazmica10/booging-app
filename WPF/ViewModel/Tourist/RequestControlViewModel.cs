using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestControlViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourRequestService _tourRequestService = new();
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<string>();
        }

        public void Validate(string propertyName, object propertyValue)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

            if (results.Any())
            {
                _errors[propertyName] = results.Select(r => r.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            //AddCommand.RaiseCanExecuteChanged();
        }

        private string _name = "new tour request";
        [Required(ErrorMessage = "Request name is required.")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                Validate(nameof(Name), value);
            }
        }

        private Location? _selectedLocation;
        [Required(ErrorMessage = "Location is required.")]
        public Location? SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                Validate(nameof(SelectedLocation), value);
            }
        }

        private Language? _selectedLanguage;
        [Required(ErrorMessage = "Language is required.")]
        public Language? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                Validate(nameof(SelectedLanguage), value);
            }
        }

        private string _description = "new tour request description";
        [Required(ErrorMessage = "Description is required.")]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
                Validate(nameof(Description), value);
            }
        }


        public AddPeopleControlViewModel AddPeopleControl { get; set; }

        // dates

        private DateTime _startDate = DateTime.Now.AddDays(4);
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                if (_endDate < _startDate) { _startDate = _endDate; }
                OnPropertyChanged(nameof(StartDate));
            }
        }


        private DateTime _endDate = DateTime.Now.AddDays(10);
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                if (_endDate < _startDate) { _endDate = _startDate; }
                OnPropertyChanged(nameof(EndDate));
            }
        }

        // min max dates to display
        public DateTime SpanStartDisplayDateStart { get; set; } = DateTime.Now.AddDays(4);
        public DateTime SpanStartDisplayDateEnd { get; set; } = DateTime.Now.AddDays(30);
        public DateTime SpanEndDisplayDateStart { get; set; } = DateTime.Now.AddDays(5);
        public DateTime SpanEndDisplayDateEnd { get; set; } = DateTime.Now.AddDays(100);


        private string _statusMessage = "";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

        public RequestControlViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            AddPeopleControl = new AddPeopleControlViewModel(CurrentUser);
        }

        public TourRequestDto? TourRequest { get; set; }

        public bool CreateAction()
        {
            if (_selectedLocation == null) { StatusMessage = "Invalid location selected."; return false; }
            if (_selectedLanguage == null) { StatusMessage = "Invalid language selected."; return false; }

            if (AddPeopleControl.Reservations.Count <= 0) { StatusMessage = "No people specified."; return false; }

            StatusMessage = "Created.";
            TourRequest = new TourRequestDto(CurrentUser.Id, Name, _selectedLocation, Description, _selectedLanguage, StartDate.Date, EndDate.Date, AddPeopleControl.Reservations.ToList());
            return true;
        }
    }
}
