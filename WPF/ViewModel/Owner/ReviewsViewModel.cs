using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Domain.IRepository;
using BookingApp.DTO.Owner;
using BookingApp.Utilities;
using BookingApp.Command;
using System.Windows.Input;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ReviewsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<ReviewDto> _reviews = new();
        public ObservableCollection<ReviewDto> Reviews { get => _reviews; set { _reviews = value; OnPropertyChanged(nameof(Reviews)); } }

        private string _searchText = "";
        public string SearchText { get => _searchText; set { _searchText = value; OnPropertyChanged(nameof(SearchText)); FilterReviews(); } }

        private ReviewDto _selectedReview = new();
        public ReviewDto SelectedReview { get => _selectedReview; set { _selectedReview = value; OnPropertyChanged(nameof(SelectedReview)); } }

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly OwnerReviewService _ownerReviewService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _reservationService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ICommand OpenReviewCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenReview(o); });
        public ICommand DetailsCommand => new FunctionCommand(() => NavService.Navigate(new AccommodationView(CurrentUser, NavService, SelectedReview.AccommodationId)));

        public ReviewsViewModel(User user, NavigationService navService, string search = "")
        {
            CurrentUser = user;
            NavService = navService;

            FillDto();
            SearchText = search;
        }

        private void FillDto()
        {
            Reviews.Clear();
            var reviews = _ownerReviewService.GetAllViewableForOwner(CurrentUser.Id);
            foreach (var review in reviews)
            {
                var accommodation = _accommodationService.GetById(review.AccommodationId);
                var reservation = _reservationService.GetById(review.ReservationId);
                var guest = _userService.GetById(review.GuestId);
                Reviews.Add(new ReviewDto
                {
                    Id = review.Id,
                    GuestName = $"{guest.FirstName} {guest.LastName}",
                    GuestPhoto = _pictureService.GetPictureLocation(guest.Photo),
                    AccommodationName = accommodation.Name,
                    AccommodationId = accommodation.Id,
                    CheckIn = reservation.CheckIn,
                    CheckOut = reservation.CheckOut,
                    Cleanliness = review.Cleanliness,
                    OwnerRating = review.OwnerRating,
                    Comment = review.Comment,
                    Photos = _pictureService.GetPictureLocations(review.Photos),
                    AccommodationLocation = $"{accommodation.Location.City}, {accommodation.Location.Country}",
                    AccommodationPhoto = _pictureService.GetPictureLocation(accommodation.Photos[0]),
                    GuestCount = reservation.GuestCount
                });
            }
            if (Reviews.Count > 0)
                OpenReview(Reviews[0].Id);
        }

        private void FilterReviews()
        {
            FillDto();
            Reviews = new ObservableCollection<ReviewDto>(Reviews.Where(r => r.AccommodationName.ToLower().Contains(SearchText.ToLower())));
            if (Reviews.Count > 0)
                OpenReview(Reviews[0].Id);
        }

        public void OpenReview(int reviewId)
        {
            foreach (var r in Reviews)
            {
                if (r.Id != reviewId)
                    r.Selected = false;
                else
                    r.Selected = true;
            }

            SelectedReview = Reviews.First(r => r.Selected);
            CurrentPhoto = SelectedReview.Photos.FirstOrDefault();
        }

        private void NextPhoto()
        {
            if (SelectedReview.Photos.Count == 0) return;
            int index = SelectedReview.Photos.IndexOf(CurrentPhoto);
            if (index == SelectedReview.Photos.Count - 1)
                CurrentPhoto = SelectedReview.Photos[0];
            else
                CurrentPhoto = SelectedReview.Photos[index + 1];
        }

        private void PrevPhoto()
        {
            if (SelectedReview.Photos.Count == 0) return;
            int index = SelectedReview.Photos.IndexOf(CurrentPhoto);
            if (index == 0)
                CurrentPhoto = SelectedReview.Photos.Last();
            else
                CurrentPhoto = SelectedReview.Photos[index - 1];
        }
    }
}
