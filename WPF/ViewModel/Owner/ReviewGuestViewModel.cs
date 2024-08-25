using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Owner;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApp.Utilities;
using System.Windows.Navigation;
using BookingApp.DTO.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ReviewGuestViewModel : ViewModelBase
    {
        #region Bindings

        private int _cleanliness;
        public int Cleanliness { get => _cleanliness; set { if (value != _cleanliness) { _cleanliness = value; OnPropertyChanged(nameof(Cleanliness)); } } }

        private int _ruleFollowing;
        public int RuleFollowing { get => _ruleFollowing; set { if (value != _ruleFollowing) { _ruleFollowing = value; OnPropertyChanged(nameof(RuleFollowing)); } } }

        private string _comment = "";
        public string Comment { get => _comment; set { if (value != _comment) { _comment = value; OnPropertyChanged(nameof(Comment)); } } }

        private ReviewGuestDto _reviewDto;
        public ReviewGuestDto ReviewDto { get => _reviewDto; set { if (value != _reviewDto) { _reviewDto = value; OnPropertyChanged(nameof(ReviewDto)); } } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;
        public int ReservationId;

        private readonly AccommodationReservationService _reservationService = new();
        private readonly GuestReviewService _reviewService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public ICommand SubmitCommand => new FunctionCommand(SubmitReview);
        public ICommand CancelCommand => new FunctionCommand(CancelReview);
        public ICommand CleanlinessCommand => new FunctionArgCommand(param => Cleanliness = int.Parse((string)param));
        public ICommand RuleFollowingCommand => new FunctionArgCommand(param => RuleFollowing = int.Parse((string)param));

        public ReviewGuestViewModel(User user, NavigationService navService, int reservationId)
        {
            CurrentUser = user;
            NavService = navService;
            ReservationId = reservationId;

            FillDto();
        }

        private void FillDto()
        {
            var reservation = _reservationService.GetById(ReservationId);
            var accommodation = _accommodationService.GetById(reservation.AccommodationId);
            var guest = _userService.GetById(reservation.GuestId);
            ReviewDto = new ReviewGuestDto { GuestName = $"{guest.FirstName} {guest.LastName}", AccommodationPhoto = _pictureService.GetPictureLocation(accommodation.Photos[0]), AccommodationName = accommodation.Name, AccommodationLocation = $"{accommodation.Location.City}, {accommodation.Location.Country}", CheckIn = reservation.CheckIn, CheckOut = reservation.CheckOut };
        }

        private void SubmitReview()
        {
            if (!GuestReviewUtils.ValidateReview(Cleanliness, RuleFollowing, Comment))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            AccommodationReservation reservation = _reservationService.GetById(ReservationId);
            reservation.Status = ReservationStatus.Reviewed;
            _reservationService.Update(reservation);

            GuestReview review = new GuestReview(0, reservation.AccommodationId, reservation.GuestId, reservation.Id, Comment, Cleanliness, RuleFollowing);
            _reviewService.AddGuestReview(review);

            MessageBox.Show("Added review.");
            NavService.Navigate(new HomeView(CurrentUser, NavService));
        }

        private void CancelReview()
        {
            if (NavService.CanGoBack)
                NavService.GoBack();
            else
                NavService.Navigate(new HomeView(CurrentUser, NavService));
        }
    }
}
