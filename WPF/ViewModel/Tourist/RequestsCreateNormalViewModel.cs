using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestsCreateNormalViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourRequestService _tourRequestService = new();
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();

        public RequestControlViewModel RequestControlViewModel { get; set; }

        public ICommand CreateCommand { get; }

        public RequestsCreateNormalViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            RequestControlViewModel = new RequestControlViewModel(CurrentUser, navigationService);

            // other commands
            CreateCommand = new FunctionCommand(CreateAction);
        }

        public void CreateAction()
        {
            if (!RequestControlViewModel.CreateAction()) { return; }

            if (RequestControlViewModel.TourRequest != null)
            {
                _tourRequestService.MakeTourRequest(RequestControlViewModel.TourRequest, -1);
            }

            MainNavigationService.Navigate(new RequestsView(CurrentUser, MainNavigationService, 0)); // todo: focus on id
        }
    }
}
