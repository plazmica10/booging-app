using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class NormalRequestViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        private readonly TourRequestService _tourRequestService = new();
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();
        public ICommand IncreaseMaxTouristsCommand => new RelayCommand<string>(IncreaseMaxTourists);
        public ICommand DecreaseMaxTouristsCommand => new RelayCommand<string>(DecreaseMaxTourists);
        public ObservableCollection<ItemTourRequestViewModel> Requests { get; set; } = new();
       
        private void IncreaseMaxTourists(object obj)
        {
            int increment = Convert.ToInt32(obj);
            SelectedDuration += increment;
        }

        private void DecreaseMaxTourists(object obj)
        {
            int decrement = Convert.ToInt32(obj);
            if (SelectedDuration > 0)
                SelectedDuration -= decrement;
        }
        private string _name = "";

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _description = "";

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _peopleCount = "";

        public string PeopleCount
        {
            get => _peopleCount;
            set
            {
                _peopleCount = value;
                OnPropertyChanged(nameof(PeopleCount));
            }
        }

        private string _location = "";

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string _language = "";

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private string _spanStart = "";

        public string SpanStart
        {
            get => _spanStart;
            set
            {
                _spanStart = value;
                OnPropertyChanged(nameof(SpanStart));
            }
        }

        private string _spanEnd = "";

        public string SpanEnd
        {
            get => _spanEnd;
            set
            {
                _spanEnd = value;
                OnPropertyChanged(nameof(SpanEnd));
            }
        }

        private ItemTourRequestViewModel? _requestsSelected;

        public ItemTourRequestViewModel? RequestsSelected
        {
            get => _requestsSelected;
            set
            {
                _requestsSelected = value;

                if (_requestsSelected != null)
                {
                    TourRequest? tourRequest = _tourRequestService.GetById(_requestsSelected.Id);
                    if (tourRequest != null)
                    {
                        Name = tourRequest.Name;
                        Description = tourRequest.Description;
                        PeopleCount = tourRequest.Reservations.Count.ToString();
                        Location = tourRequest.Location.ToSmallString();
                        Language = tourRequest.Language.Name;
                        SpanStart = $"{tourRequest.SpanStart:yyyy-MM-dd HH:mm}";
                        SpanEnd = $"{tourRequest.SpanEnd:yyyy-MM-dd HH:mm}";
                    }
                }

                OnPropertyChanged(nameof(RequestsSelected));
            }
        }

        private int _searchType=0;

        public int SearchType
        {
            get => _searchType;
            set
            {
                _searchType = value;
                OnPropertyChanged(nameof(SearchType));
                UpdateVisibility();
            }
        }

        private Visibility _languageControlVisibility = Visibility.Collapsed;

        public Visibility LanguageControlVisibility
        {
            get => _languageControlVisibility;
            set
            {
                _languageControlVisibility = value;
                OnPropertyChanged(nameof(LanguageControlVisibility));
            }
        }

        private Visibility _locationControlVisibility = Visibility.Collapsed;

        public Visibility LocationControlVisibility
        {
            get => _locationControlVisibility;
            set
            {
                _locationControlVisibility = value;
                OnPropertyChanged(nameof(LocationControlVisibility));
            }
        }

        private void UpdateVisibility()
        {
            SelectedLocation = null;
            SelectedLanguage = null;
            if (SearchType == 0)
            {
                LocationControlVisibility = Visibility.Visible;
                LanguageControlVisibility = Visibility.Collapsed;
            }
            else
            {
                LanguageControlVisibility = Visibility.Visible;
                LocationControlVisibility = Visibility.Collapsed;
            }
        }

        private string _filterLocationOrLanguage = "";

        public string FilterLocationOrLanguage
        {
            get => _filterLocationOrLanguage;
            set
            {
                _filterLocationOrLanguage = value;
                OnPropertyChanged(nameof(FilterLocationOrLanguage));
            }
        }

        private DateTime? _filterSpanStart = DateTime.Now; // filtering

        public DateTime? FilterSpanStart
        {
            get => _filterSpanStart;
            set
            {
                _filterSpanStart = value;
                OnPropertyChanged(nameof(FilterSpanStart));
            }
        }

        private DateTime? _filterSpanEnd = DateTime.Now.AddDays(30);

        public DateTime? FilterSpanEnd
        {
            get => _filterSpanEnd;
            set
            {
                _filterSpanEnd = value;
                OnPropertyChanged(nameof(FilterSpanEnd));
            }
        }

        private string? _filterPeopleCount="0";

        public string? FilterPeopleCount
        {
            get => _filterPeopleCount;
            set
            {
                _filterPeopleCount = value;
                OnPropertyChanged(nameof(FilterPeopleCount));
            }
        }

        private DateTime _selectedStartsAt = DateTime.Now; // picking starts at time & duration for approval

        public DateTime SelectedStartsAt
        {
            get => _selectedStartsAt;
            set
            {
                _selectedStartsAt = value;
                OnPropertyChanged(nameof(SelectedStartsAt));
            }
        }

        private string _selectedTime = "13:00:00";

        public string SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        private int _selectedDuration = 0;

        public int SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                OnPropertyChanged(nameof(SelectedDuration));
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

        public ICommand IncreaseDurationCommand { get; }
        public ICommand DecreaseDurationCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }
        private void RejectAction()
        {
            if (_requestsSelected == null)
            {
                MessageBox.Show("No tour request selected");
                return;
            }

            try
            {
                TourRequest? tourRequest = _tourRequestService.GetById(_requestsSelected.Id);
                if (tourRequest != null)
                {
                    tourRequest.Status = TourRequestStatus.Rejected;  // Set the status to Rejected
                    _tourRequestService.Update(tourRequest);  // Save the changes (assuming Update method exists)
                    MessageBox.Show("Rejected tour request");
                    Requests.Remove(_requestsSelected);
                }
                else
                {
                    MessageBox.Show("Failed to find the tour request");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while rejecting the tour request");
            }
        }

        public NormalRequestViewModel(User currentUser, NavigationService nav)
        {
            CurrentUser = currentUser;
            NavigationService = nav;

            IncreaseDurationCommand = new RelayCommand<string>(IncreaseDuration);
            DecreaseDurationCommand = new RelayCommand<string>(DecreaseDuration);
            SearchCommand = new FunctionCommand(SearchAction);
            ApproveCommand = new FunctionCommand(ApproveAction);
            RejectCommand = new FunctionCommand(RejectAction);

            foreach (TourRequest request in _tourRequestService.GetAllWaiting())
            {
                Requests.Add(new ItemTourRequestViewModel(request));
            }
            UpdateVisibility();
        }

        private void IncreaseDuration(string obj)
        {
            int increment = Convert.ToInt32(obj);
            if (!int.TryParse(FilterPeopleCount, out int num))
            {
                num = 0;
            }

            num += increment;
            FilterPeopleCount = num.ToString();
        }

        private void DecreaseDuration(string obj)
        {
            int decrement = Convert.ToInt32(obj);
            if (!int.TryParse(FilterPeopleCount, out int num))
            {
                num = 0;
            }

            if (num > 0)
                num -= decrement;
            FilterPeopleCount = num.ToString();
        }

        private void SearchAction()
        {
           
            int filterPeopleCount = 0;

            
            // Parse the people count if available
            if (!string.IsNullOrEmpty(FilterPeopleCount) && int.TryParse(FilterPeopleCount, out int num))
            {
                filterPeopleCount = num;
            }

            // Perform the search using the filtered parameters
            var filteredRequests = _tourRequestService.GetAllWaitingFiltered(new RequestsFilterDto(
                SelectedLocation,
                SelectedLanguage,
                filterPeopleCount,
                FilterSpanStart,
                FilterSpanEnd
            ));

            // 3. Error Handling: Check for exceptions
            // 4. Data Binding: Confirm if the properties are correctly bound
            // 5. Breakpoints and Debugging: Debug and check if this code block is being executed

            // Clear the existing requests and add the filtered ones
            Requests.Clear();
            foreach (TourRequest request in filteredRequests)
            {
                Requests.Add(new ItemTourRequestViewModel(request));
            }
        }


        private void ApproveAction()
        {
            if (_requestsSelected == null) { return; }

            try
            {
                DateTime date = (DateTime)SelectedStartsAt;
                if (TimeSpan.TryParse(SelectedTime, out TimeSpan time)) { date = date.Date.Add(time); }

                switch (_tourRequestService.ApproveTourRequest(CurrentUser.Id, _requestsSelected.Id, date, SelectedDuration))
                {
                    case TourServiceReturn.Success: MessageBox.Show("approved tour request"); break;
                    case TourServiceReturn.InvalidTourRequestTime: MessageBox.Show("Invalid Tour Request Time"); break;
                    case TourServiceReturn.FailedToApproveBusyForTourRequest: MessageBox.Show("can't approve -- busy"); break;
                }
            }
            catch (Exception e) { MessageBox.Show("invalid input"); }
        }
    }
    }

