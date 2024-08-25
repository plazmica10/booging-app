using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Repository;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly GuestReviewService _reviewService = new();
        private readonly UserService _userService = new();
        private readonly GuestReviewService _guestReviewService = new();
        private  readonly RescheduleRequestService _rescheduleRequestService = new();
        private readonly OwnerReviewService _ownerReviewService = new();
        public NavigationService NavigationService;
        private User _currentUser;

        public ProfileViewModel(User user, NavigationService navService)
        {
            NavigationService = navService;
            CurrentUser = _userService.GetById(user.Id);
            _userService.RefreshSuperGuest();
            Reviews = new ObservableCollection<ReviewsListDto>(_reviewService.GetReviewsDto(CurrentUser.Id));
            AverageRating = _guestReviewService.GetAverageRating(CurrentUser.Id);
            RescheduleRequests = new ObservableCollection<RescheduleRequestsListDto>(_rescheduleRequestService.GetRequestsShortDtos(CurrentUser.Id));
        }
        #region bindings
        public User CurrentUser { get => _currentUser; set{ _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); }}
        private ObservableCollection<ReviewsListDto> _reviews;
        public ObservableCollection<ReviewsListDto> Reviews { get => _reviews; set { _reviews = value; OnPropertyChanged(nameof(Reviews)); } }
        private double _averageRating;
        public double AverageRating { get => _averageRating; set { _averageRating = value; OnPropertyChanged(nameof(AverageRating)); } }
        private ObservableCollection<RescheduleRequestsListDto> _rescheduleRequests;
        public ObservableCollection<RescheduleRequestsListDto> RescheduleRequests { get => _rescheduleRequests; set { _rescheduleRequests = value; OnPropertyChanged(nameof(RescheduleRequests)); } }

        private RescheduleRequestsListDto _selectedRequest;
        public RescheduleRequestsListDto SelectedRequest { get => _selectedRequest; set { _selectedRequest = value; OnPropertyChanged(nameof(SelectedRequest)); } }
        private ReviewsListDto _selectedReview;
        public ReviewsListDto SelectedReview { get => _selectedReview; set { _selectedReview = value; OnPropertyChanged(nameof(SelectedReview)); } }
        public ICommand DetailsCommand => new FunctionCommand(Details);
        public ICommand ReviewDetailsCommand => new FunctionCommand(ReviewDetails);

        #endregion

        public void Details()
        {
            if (SelectedRequest != null)
            {
                RequestDto req = _ownerReviewService.GetRequest(SelectedRequest);
                NavigationService.Navigate(new RequestDetailsView(CurrentUser, NavigationService, req));
            }
        }

        public void ReviewDetails()
        {
            if (SelectedReview != null)
            {
                GuestReview review = _guestReviewService.GetById(SelectedReview.ReviewId);
                NavigationService.Navigate(new ReviewDetailsView(CurrentUser, NavigationService, review));
            }
        }
    }
}
