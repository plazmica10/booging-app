using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using BookingApp.WPF.View.Tourist;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestsCreateComplexViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourRequestComplexService _tourRequestComplexService = new();
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();

        public ICommand CreateCommand { get; }
        public ICommand AddPartCommand { get; }
        public ICommand RemovePartCommand { get; }
        public TourRequestComplexDto Data { get; set; }

        private string _statusMessage = "";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

        public RequestsCreateComplexViewModel(User currentUser, NavigationService navigationService, TourRequestComplexDto? data = null)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            Data = data ?? new TourRequestComplexDto(CurrentUser.Id, "new complex request");

            AddPartCommand = new FunctionCommand(AddPartAction);
            RemovePartCommand = new RelayCommand<TourRequestDto>(RemovePartAction);
            CreateCommand = new FunctionCommand(CreateAction);
        }

        public void AddPartAction()
        {
            MainNavigationService.Navigate(new RequestsCreateComplexPartView(CurrentUser, MainNavigationService, Data)); // todo: focus on id
        }

        public void RemovePartAction(TourRequestDto item)
        {
            Data.Parts.Remove(item);
        }

        public void CreateAction()
        {
            if (Data.Parts.Count <= 0) { StatusMessage = "Can't create tour with 0 parts."; return; }

            _tourRequestComplexService.MakeTourRequestComplex(Data);
            MainNavigationService.Navigate(new RequestsView(CurrentUser, MainNavigationService, 1)); // todo: focus on id
        }
    }
}
