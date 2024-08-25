using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using BookingApp.WPF.View.Tourist;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestsCreateComplexPartViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourRequestService _tourRequestService = new();
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();

        public RequestControlViewModel RequestControlViewModel { get; set; }
        public TourRequestComplexDto Data { get; set; }

        public ICommand CreateCommand { get; }

        public RequestsCreateComplexPartViewModel(User currentUser, NavigationService navigationService, TourRequestComplexDto data)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;
            Data = data;
            RequestControlViewModel = new RequestControlViewModel(CurrentUser, navigationService)
            {
                Name = $"Part {Data.Parts.Count + 1}"
            };
            CreateCommand = new FunctionCommand(CreateAction);
        }


        public void CreateAction()
        {
            if (!RequestControlViewModel.CreateAction()) { return; }
            if (RequestControlViewModel.TourRequest != null) { Data.Parts.Add(RequestControlViewModel.TourRequest); }
            MainNavigationService.Navigate(new RequestsCreateComplexView(CurrentUser, MainNavigationService, Data));
        }
    }
}
