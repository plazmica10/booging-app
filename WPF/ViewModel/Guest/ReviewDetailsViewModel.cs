using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.Utilities;
using static QuestPDF.Helpers.Colors;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReviewDetailsViewModel :  ViewModelBase
    {
        #region stuff

            private readonly AccommodationService _accommodationService = new AccommodationService();
            private readonly AccommodationReservationService _accommodationReservationService = new AccommodationReservationService();
            private readonly PictureService _pictureService = new PictureService();
            private User _currentUser;
            private NavigationService _navigationService;
            
            private GuestReview _review;
            public GuestReview Review { get => _review; set { _review = value; OnPropertyChanged(nameof(Review)); } }
            private Accommodation _accommodation;
            public Accommodation Accommodation { get => _accommodation; set { _accommodation = value; OnPropertyChanged(nameof(Accommodation)); } }
            public AccommodationReservation _reservation;
            public AccommodationReservation Reservation { get => _reservation; set { _reservation = value; OnPropertyChanged(nameof(Reservation)); } }
            private string _currentImage;
            public string CurrentImage { get => _currentImage; set { _currentImage = value; OnPropertyChanged(nameof(CurrentImage)); } }
            public ICommand NextCommand => new FunctionCommand(NextImage);
            public ICommand PreviousCommand => new FunctionCommand(PreviousImage);
            public int ImageCount { get { return SortedImages?.Count ?? 0; } }
             public ObservableCollection<string> SortedImages { get; set; }
            public ObservableCollection<int> CleanlinessStars { get { return new ObservableCollection<int>(Enumerable.Range(1, Review.Cleanliness)); } }
            public ObservableCollection<int> RuleF { get { return new ObservableCollection<int>(Enumerable.Range(1, Review.RuleFollowing)); } }
        #endregion

        public ReviewDetailsViewModel(User user, NavigationService navigationService, GuestReview review)
        {
            _currentUser = user;
            _navigationService = navigationService;
            _review = review;
            _accommodation  = _accommodationService.GetById(review.AccommodationId);
            _reservation = _accommodationReservationService.GetById(review.ReservationId);
            _accommodation.Photos = _pictureService.GetPictureLocations(_accommodation.Photos);
            SortedImages = new ObservableCollection<string>(_accommodation.Photos.OrderBy(x => x));
            CurrentImage = SortedImages.First();
        }
        private void NextImage() { CurrentImage = ImageControl.GetNextImage(SortedImages, CurrentImage); }
        private void PreviousImage() { CurrentImage = ImageControl.GetPreviousImage(SortedImages, CurrentImage); }
    }
}
