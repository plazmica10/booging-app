using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.WPF.View.Guide;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class StatisticsToursViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        private readonly TourStatisticsService _tourStatisticsService = new();

        public ObservableCollection<ItemTourStatViewModel> Tours { get; set; } = new();
        public ItemTourStatViewModel? ToursSelected { get; set; }


        private bool _showBest;
        public bool ShowBest { get => _showBest; set { _showBest = value; OnPropertyChanged(nameof(ShowBest)); } }

        private string _selectedYear = "All Time";
        public string SelectedYear { get => _selectedYear; set { _selectedYear = value; UpdateAction(); OnPropertyChanged(nameof(SelectedYear)); } }

        private string _tourName = "";
        public string TourName { get => _tourName; set { _tourName = value; OnPropertyChanged(nameof(TourName)); } }

        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }
      
        private int _numTourists;
        public int NumTourists { get => _numTourists; set { _numTourists = value; OnPropertyChanged(nameof(NumTourists)); } }
      
        private int _duration;
        public int Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }
       
        private string _location = "";
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }

        private string _language = "";
        public string Language { get => _language; set { _language = value; OnPropertyChanged(nameof(Language)); } }

        public ICommand RequestsForToursCommand { get; }

        public StatisticsToursViewModel(User currentUser, NavigationService navigation)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            RequestsForToursCommand = new FunctionCommand(RequestsForToursAction);

            UpdateAction();

        }

        private void RequestsForToursAction(){ NavigationService.Navigate(new StatisticsRequestsView(CurrentUser, NavigationService)); }

        private void UpdateAction() 
        {
            int year = 0;
            int.TryParse(_selectedYear, out year);

            BestTourStatsDto? statsDto = _tourStatisticsService.BestTourForYear(CurrentUser.Id, year);
            if (statsDto == null){ShowBest=false;}
            else
            {
                TourName = statsDto.Name;
                Language = statsDto.Language.ToReadableString();
                Location = statsDto.Location.ToSmallString();
                Description = statsDto.Description;
                NumTourists = statsDto.PeopleArrived;
                ShowBest = true;
            }

            Tours.Clear();
            foreach (TouristsAgeStatisticsDto touristsAgeStats in _tourStatisticsService.GetStatisticsForYear(CurrentUser.Id, year)){ Tours.Add(new ItemTourStatViewModel(touristsAgeStats)); }
            
        }
    }
}
