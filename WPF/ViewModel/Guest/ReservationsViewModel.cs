using System;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReservationsViewModel : ViewModelBase
    {
        private AccommodationReservationService _reservationService = new();
        private OwnerReviewService _ownerReviewService = new();
        private AccommodationService _accommodationService = new();
        private RescheduleRequestService _rescheduleRequestService = new();
        public NavigationService NavigationService;
        public User CurrentUser;

        public ReservationsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
            CurrentReservations = new ObservableCollection<ReservationsDto>(_accommodationService.GetReservationsDto(user.Id,false));
            PastReservations = new ObservableCollection<ReservationsDto>(_accommodationService.GetReservationsDto(user.Id,true));
            CancelCommand = new FunctionCommand(CancellationAction);
            DetailsCommand = new FunctionCommand(ViewCurrentReservationDetails);
            PastDetailsCommand = new FunctionCommand(ViewPastReservationDetails);
            RescheduleCommand = new FunctionCommand(RescheduleReservation);
            ReviewCommand = new FunctionCommand(ReviewReservation);
        }

        #region bindings and commands
        public  ICommand CancelCommand { get; set; }
        public  ICommand DetailsCommand { get; set; }
        public  ICommand PastDetailsCommand { get; set; }
        public ICommand RescheduleCommand { get; set; }
        public ICommand ReviewCommand { get; set; }

        private ObservableCollection<ReservationsDto> _pastReservations;
        public ObservableCollection<ReservationsDto> PastReservations { get => _pastReservations; set { _pastReservations = value; OnPropertyChanged(nameof(PastReservations)); } }
        private ObservableCollection<ReservationsDto> _currentReservations;
        public ObservableCollection<ReservationsDto> CurrentReservations { get => _currentReservations; set { _currentReservations = value; OnPropertyChanged(nameof(CurrentReservations)); } }
        private ReservationsDto _selectedCurrentReservation;
        public ReservationsDto SelectedCurrentReservation { get => _selectedCurrentReservation; set { _selectedCurrentReservation = value; OnPropertyChanged(nameof(SelectedCurrentReservation)); } }
        private ReservationsDto _selectedPastReservation;
        public ReservationsDto SelectedPastReservation { get => _selectedPastReservation; set { _selectedPastReservation = value; OnPropertyChanged(nameof(SelectedPastReservation)); } }

        #endregion
        public void CancellationAction()
        {
            if (_selectedCurrentReservation != null)
            {   
                var accommodation = _accommodationService.GetById(SelectedCurrentReservation.AccommodationId);
                if (ReservationUtils.ValidateCancellation(SelectedCurrentReservation.CheckIn, SelectedCurrentReservation.CheckOut,accommodation.RefundPeriod))
                {
                    var result = MessageBox.Show(TranslationSource.Instance["CancelResQ"], TranslationSource.Instance["CancelRes"], MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        CancelReservation();
                    }
                }
            }
        }

        public void CancelReservation()
        {
            var reservation = _reservationService.GetById(SelectedCurrentReservation.Id);
            reservation.Status = ReservationStatus.Canceled;
            _reservationService.Update(reservation);
            _rescheduleRequestService.Delete(reservation.Id);
            CurrentReservations.Remove(SelectedCurrentReservation);
            MessageBox.Show(TranslationSource.Instance["CancelSucc"]);
        }

        public void ViewCurrentReservationDetails()
        {
            if (_selectedCurrentReservation != null) NavigationService.Navigate(new ReservationSummaryView(CurrentUser,SelectedCurrentReservation , NavigationService));
        }
        public void ViewPastReservationDetails()
        { 
            if (_selectedPastReservation != null) NavigationService.Navigate(new ReservationSummaryView(CurrentUser, SelectedPastReservation, NavigationService));
        }
        public void RescheduleReservation()
        {
            if (_selectedCurrentReservation != null)
            {
                if (_selectedCurrentReservation.CheckIn <= DateTime.Today)
                {
                    MessageBox.Show(TranslationSource.Instance["ReschPast"]);
                    return;
                }
                NavigationService.Navigate(new RescheduleView(CurrentUser, NavigationService, _reservationService.GetById(SelectedCurrentReservation.Id)));
            }
        }
        public void ReviewReservation()
        {
            if (_selectedPastReservation != null)
            {
                var review = _ownerReviewService.GetByReservationId(SelectedPastReservation.Id);
                if (review != null)
                {
                    MessageBox.Show(TranslationSource.Instance["AlrReviewed"]);
                    return;
                }
                if (ReservationUtils.ExpiredReviewDate(_selectedPastReservation.CheckOut))
                {
                    MessageBox.Show(TranslationSource.Instance["5Days"]);
                    return;
                }
                ReviewView reviewWindow;
                reviewWindow = new ReviewView(CurrentUser, NavigationService, _reservationService.GetById(SelectedPastReservation.Id));
                reviewWindow.ShowDialog();
            }
        }
    }
}
