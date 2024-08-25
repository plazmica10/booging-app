using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestsViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        public ICommand ShowStatisticsCommand { get; }
        public ICommand CreateRequestNormalCommand { get; }
        public ICommand CreateRequestComplexCommand { get; }

        private readonly TourRequestService _tourRequestService = new();
        private readonly TourRequestComplexService _tourRequestComplexService = new();

        public ObservableCollection<ItemRequestViewModel> Requests { get; set; } = new();
        public ObservableCollection<ItemRequestComplexViewModel> RequestsComplex { get; set; } = new();

        public int SelectedTabIndex { get; set; } = 0; 

        public RequestsViewModel(User currentUser, NavigationService navigationService, int selectedTabIndex)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            ShowStatisticsCommand = new FunctionCommand(ShowStatisticsAction);
            CreateRequestNormalCommand = new FunctionCommand(CreateRequestNormalAction);
            CreateRequestComplexCommand = new FunctionCommand(CreateRequestComplexAction);

            SelectedTabIndex = selectedTabIndex;

            foreach (TourRequest request in _tourRequestService.GetAllByTouristId(CurrentUser.Id)) { Requests.Add(new ItemRequestViewModel(request)); }
            foreach (TourRequestComplex request in _tourRequestComplexService.GetAllByTouristId(CurrentUser.Id)) { RequestsComplex.Add(new ItemRequestComplexViewModel(request)); }
        }

        public void ShowStatisticsAction()  { MainNavigationService.Navigate(new RequestsStatisticsView(CurrentUser, MainNavigationService)); }
        public void CreateRequestNormalAction() { MainNavigationService.Navigate(new RequestsCreateNormalView(CurrentUser, MainNavigationService)); }
        public void CreateRequestComplexAction() { MainNavigationService.Navigate(new RequestsCreateComplexView(CurrentUser, MainNavigationService)); }
    }
}
