using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using MenuNavigation;
using WPF;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReviewViewModel : ValidationBase
    {
        private readonly OwnerReviewService _ownerReviewService = new();
        private readonly AccommodationReservationService _reservationService = new();
        private readonly PictureService _pictureService = new();
        public NavigationService  NavigationService;

        public User CurrentUser;
        public Frame MainFrame;
        public AccommodationReservation Reservation;
        public event Action ReviewSubmitted;
        private List<string> _photos = new();

        public ReviewViewModel(User user, NavigationService navService, AccommodationReservation reservation)
        {
            CurrentUser = user;
            NavigationService = navService;
            Reservation = reservation;
            SubmitCommand = new FunctionCommand(SubmitReview);
            Review = new ReviewSubmitDto();
            Review.OwnerRating = 0;
            Review.Cleanliness = 0;
            OwnerRatingCommand = new FunctionArgCommand(param => Review.OwnerRating = int.Parse((string)param));
            PropertyRatingCommand = new FunctionArgCommand(param => Review.Cleanliness = int.Parse((string)param));
            AddPhotoCommand = new FunctionCommand(AddPhoto);
        }
        #region bindings and commands
        public ICommand SubmitCommand { get; set; }
        public ICommand OwnerRatingCommand { get; set; }
        public ICommand PropertyRatingCommand { get; set; }
        public ICommand AddPhotoCommand { get; set; }
        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ReviewSubmitDto Review { get; set; }

        private string _renovationRecommendation;
        public string RenovationRecommendation { get => _renovationRecommendation; set { _renovationRecommendation = value; OnPropertyChanged(nameof(RenovationRecommendation)); } }
        public ObservableCollection<int> UrgencyLevels { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 5 };
        public int SelectedUrgencyLevel { get; set; }
        public ObservableCollection<string> PhotoLocations { get; set; } = new();

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } }

        #endregion
        public void SubmitReview()
        {
            Validate();
            if(!ReservationUtils.ValidateReview(Review.OwnerRating,Review.Cleanliness,Review.Comment,Reservation.CheckOut)) return;
            try
            {
                OwnerReview review = new OwnerReview
                {
                    AccommodationId = Reservation.AccommodationId, GuestId = CurrentUser.Id,
                    OwnerId = Reservation.OwnerId, ReservationId = Reservation.Id, OwnerRating = Review.OwnerRating,
                    Cleanliness = Review.Cleanliness, Comment = Review.Comment, Photos = _photos.ToList()
                };
                var reservation = _reservationService.GetById(Reservation.Id);
                reservation.HasGuestReviewed = true;
                _reservationService.Update(reservation);
                _ownerReviewService.Save(review);
                SubmitRecommendation();
                ReviewSubmitted?.Invoke();
                MessageBox.Show(TranslationSource.Instance["ReviewSucc"]);
                NavigationService.Navigate(new ReservationsView(CurrentUser, NavigationService));
            }
            catch {MessageBox.Show(TranslationSource.Instance["ReviewErr"]);}
        }

        public void SubmitRecommendation()
        {
            if (string.IsNullOrEmpty(RenovationRecommendation) || SelectedUrgencyLevel == 0) return;
            var reviewId = _ownerReviewService.GetByReservationId(Reservation.Id).Id;
            RenovationRecommendation renovation = new RenovationRecommendation { ReservationId = Reservation.Id, Comment = RenovationRecommendation, Urgency = SelectedUrgencyLevel, ReviewId = reviewId };
            _ownerReviewService.SaveRenovationRecommendation(renovation);
        }

        public void AddPhoto()
        {
            List<string>? photos = _pictureService.CreatePictures();
            if (photos == null) { return; }

            foreach (string photo in photos)
            {
                _photos.Add(photo);
                PhotoLocations.Add(_pictureService.GetPictureLocation(photo));
                CurrentPhoto = PhotoLocations.Last();
            }
        }
        private void NextPhoto() { CurrentPhoto = ImageControl.GetNextImage(PhotoLocations, CurrentPhoto); }
        private void PrevPhoto() { CurrentPhoto = ImageControl.GetPreviousImage(PhotoLocations, CurrentPhoto); }
        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(Review.Comment)) { this.ValidationErrors["Comment"] = TranslationSource.Instance["EmptyComm"]; }
            if(Review.OwnerRating == 0) { this.ValidationErrors["OwnerRating"] = TranslationSource.Instance["EmptyOwnerRating"]; }
            if(Review.Cleanliness == 0) { this.ValidationErrors["Cleanliness"] = TranslationSource.Instance["EmptyCleanliness"]; }
        }
    }
}
