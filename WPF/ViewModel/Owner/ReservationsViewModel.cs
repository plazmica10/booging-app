using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Owner;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ReservationsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<AccommodationReservation>? _reservations { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations
        {
            get { return _reservations; }
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        #endregion

        public User CurrentUser { get; set; }
        public NavigationService NavService;

        private readonly AccommodationReservationService _reservationService = new();

        public ICommand ReviewCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ReservationsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;

            ReviewCommand = new FunctionArgCommand(obj => { if (obj is int o) ReviewAction(o); });
            CancelCommand = new FunctionCommand(CancelAction);

            _reservationService.UpdateReservations();
            Reservations = _reservationService.GetPendingReviewOwnerReservations(CurrentUser.Id);
        }

        private void ReviewAction(int reservationId)
        {
            NavService.Navigate(new ReviewGuestView(CurrentUser, NavService, reservationId));
        }

        private void CancelAction()
        {
            if (NavService.CanGoBack)
                NavService.GoBack();
            else
                NavService.Navigate(new HomeView(CurrentUser, NavService));
        }
    }
}
