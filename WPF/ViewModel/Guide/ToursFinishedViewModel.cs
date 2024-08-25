using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ToursFinishedViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        public NavigationService ToursNavigationService;

        private readonly TourSearchService _tourSearchService = new();
        private readonly TourReviewService _tourReviewService = new();
        private readonly TourReservationService _tourReservationService = new();
        public ObservableCollection<ItemTourViewModel> Tours { get; set; } = new();
        public ObservableCollection<ItemReviewViewModel> Reviews { get; set; } = new();
        public ObservableCollection<ItemTourReservationViewModel> TourReservations { get; set; } = new();

        private ItemTourViewModel? _toursSelected;
        public ItemTourViewModel? ToursSelected { get { return _toursSelected; } set { _toursSelected = value; UpdateReviews(); UpdateReservations(); OnPropertyChanged(nameof(ToursSelected)); }   }
        public ItemReviewViewModel? ReviewsSelected
        {
            get { return _reviewsSelected; }
            set
            {
                _reviewsSelected = value;
                OnPropertyChanged(nameof(ReviewsSelected));
            }
        }
        private ItemReviewViewModel? _reviewsSelected;
     
        public ICommand MarkInvalidReviewCommand { get; }


        public ToursFinishedViewModel(User currentUser, NavigationService navigation, NavigationService toursNavigationService)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            ToursNavigationService = toursNavigationService;
            MarkInvalidReviewCommand = new FunctionCommand(MarkInvalidReviewAction);

            UpdateTours();
        }

        private void UpdateTours()
        {
            Tours.Clear();
            foreach (Tour tour in _tourSearchService.GetAllByGuideIdFinished(CurrentUser.Id)){ Tours.Add(new ItemTourViewModel(tour)); }
            
        }

        private void UpdateReviews()
        {
            if (ToursSelected == null) { return; }

            Reviews.Clear();
            foreach (TourReview tourReview in _tourReviewService.GetTourReviews(ToursSelected.Id)) { Reviews.Add(new ItemReviewViewModel(tourReview)); }
            
        }

        private void UpdateReservations()
        {
            if (ToursSelected == null) { return; }

            TourReservations.Clear();
            foreach (TourReservation tourReservation in _tourReservationService.GetTourReservationsByTourId(ToursSelected.Id)){ TourReservations.Add(new ItemTourReservationViewModel(tourReservation)); }
            
        }

        private void MarkInvalidReviewAction()
        {
            if (ReviewsSelected == null) { return; }
            _tourReviewService.MarkInvalidReview(ReviewsSelected.Id);
            UpdateReviews();
        }

        }
    }

