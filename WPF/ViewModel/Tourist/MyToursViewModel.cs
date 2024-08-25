using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class MyToursViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourSearchService _tourSearchService = new();

        public ObservableCollection<ItemMyTourViewModel> Tours { get; set; } = new();

        public ICommand OpenTourCommand { get; }
        public ICommand ReviewCommand { get; }


        public MyToursViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            OpenTourCommand = new RelayCommand<ItemMyTourViewModel>(OpenTourAction);
            ReviewCommand = new RelayCommand<ItemMyTourViewModel>(ReviewAction);

            Populate();
        }

        private void Populate()
        {
            foreach (Tour tour in _tourSearchService.GetMyToursForTourist(CurrentUser.Id))
            {
                Tours.Add(new ItemMyTourViewModel(tour));
            }
        }

        public void OpenTourAction(ItemMyTourViewModel item)
        {
            MainNavigationService.Navigate(new TourView(CurrentUser, MainNavigationService, item.Id));
        }

        public void ReviewAction(ItemMyTourViewModel item)
        {
            MainNavigationService.Navigate(new ReviewView(CurrentUser, MainNavigationService, item.Id));
        }
    }
}
