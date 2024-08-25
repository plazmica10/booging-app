using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Owner;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class MainViewModel : ViewModelBase
    {
        #region Bindings

        private bool _showDropdown;
        public bool ShowDropdown { get => _showDropdown; set { _showDropdown = value; OnPropertyChanged(nameof(ShowDropdown)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly AccommodationReservationService _reservationService = new();
        private readonly UserService _userService = new();
        private readonly AccommodationRenovationService _renovationService = new();

        public ICommand HomeCommand => new FunctionCommand(HomeAction);
        public ICommand InboxCommand => new FunctionCommand(InboxAction);
        public ICommand ForumsCommand => new FunctionCommand(() => NavService.Navigate(new ForumsView(CurrentUser, NavService)));
        public ICommand AddAccommCommand => new FunctionCommand(AddAccommodationAction);
        public ICommand AccommodationsCommand => new FunctionCommand(AccommodationsAction);
        public ICommand ReservationsCommand => new FunctionCommand(ReservationsAction);
        public ICommand ReviewsCommand => new FunctionCommand(ReviewsAction);
        public ICommand RenovationsCommand => new FunctionCommand(RenovationsAction);
        public ICommand RecommendationsCommand => new FunctionCommand(() => { ShowDropdown = false; NavService.Navigate(new RecommendationsView(CurrentUser, NavService)); });
        public ICommand ProfileCommand => new FunctionCommand(() => NavService.Navigate(new ProfileView(CurrentUser, NavService)));
        public ICommand DropdownCommand => new FunctionCommand(() => ShowDropdown = !ShowDropdown);
        public ICommand NewForumsCommand => new FunctionCommand(() => { ShowDropdown = false; NavService.Navigate(new NewForumsView(CurrentUser, NavService)); });

        public MainViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;

            _reservationService.UpdateReservations();
            _renovationService.UpdateRenovationsStatus();
            ShowNotification();
            _userService.RefreshSuper();
            HomeAction();
        }

        private void HomeAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new HomeView(CurrentUser, NavService));
        }

        private void InboxAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new InboxView(CurrentUser, NavService));
        }

        private void AddAccommodationAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new AddAccommodationView(CurrentUser, NavService));
        }

        private void AccommodationsAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new AccommodationsView(CurrentUser, NavService));
        }

        private void ReservationsAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new ReservationsView(CurrentUser, NavService));
        }

        private void ReviewsAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new ReviewsView(CurrentUser, NavService));
        }

        private void RenovationsAction()
        {
            ShowDropdown = false;
            NavService.Navigate(new RenovationsView(CurrentUser, NavService));
        }

        private void ShowNotification()
        {
            List<AccommodationReservation> ownerReservations = _reservationService.GetAllReservationsForOwner(CurrentUser.Id);
            bool showNotification = ownerReservations.Any(res => res.Status == ReservationStatus.PendingReview);
            if (showNotification)
            {
                MessageBox.Show("You have pending reviews");
            }
        }
    }
}
