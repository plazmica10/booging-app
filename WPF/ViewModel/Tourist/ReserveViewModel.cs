using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using BookingApp.WPF.View.Tourist;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ReserveViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourReservationService _tourReservationService = new();
        private readonly TourInfoService _tourInfoService = new();
        private readonly TourSearchService _tourSearchService = new();

        private readonly int _tourId;
        public string Title { get; set; }

        public AddPeopleControlViewModel AddPeopleControl { get; set; }

        public ObservableCollection<ItemSelectVoucherViewModel> Vouchers { get; set; } = new();
        public ItemSelectVoucherViewModel? VouchersSelected { get; set; }

        private string _statusMessage = "";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

        public ICommand AlternativeCommand { get; }
        public ICommand SubmitCommand { get; }

        public void SubmitAction()
        {
            if (AddPeopleControl.Reservations.Count <= 0) { StatusMessage = "Can't reserve nothing..."; return; }
            ReservationRequestDto requestDto = new(_tourId, CurrentUser.Id, VouchersSelected == null ? -1 : VouchersSelected.Id, AddPeopleControl.Reservations.ToList());
            switch(_tourReservationService.ReserveTour(requestDto))
            {
                case TourServiceReturn.Success: StatusMessage = "Success!"; break;
                case TourServiceReturn.NotFoundTour: StatusMessage = "Tour can't be found."; break;
                case TourServiceReturn.TooManyPeople: StatusMessage = "Too many people in reservation."; break;
                case TourServiceReturn.NotEnoughSpace: StatusMessage = "Tour has no more space left, Show alternatives on same location?"; break;
            }
        }

        private void AlternativeAction() { MainNavigationService.Navigate(new ToursView(CurrentUser, MainNavigationService, _tourSearchService.SearchAvailableToursAlternative(_tourId))); }

        public ReserveViewModel(User currentUser, NavigationService navigationService, int tourId)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            // display title
            _tourId = tourId;
            Tour? tour = _tourSearchService.GetById(_tourId);
            Title = "\ud83d\udcdd Reserve: " + (tour == null ? "" : tour.Name);

            // add people control
            AddPeopleControl = new AddPeopleControlViewModel(CurrentUser);

            // other buttons
            AlternativeCommand = new FunctionCommand(AlternativeAction);
            SubmitCommand = new FunctionCommand(SubmitAction);

            // populate vouchers
            foreach (TourVoucher voucher in _tourInfoService.GetTouristVouchers(CurrentUser.Id)) { Vouchers.Add(new ItemSelectVoucherViewModel(voucher)); }
        }
    }
}
