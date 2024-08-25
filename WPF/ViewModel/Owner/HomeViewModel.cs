using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    internal class HomeViewModel : ViewModelBase
    {
        #region Bindings

        public ObservableCollection<HomeDTO> CurrentReservations { get; set; } = new();

        private bool _checkingOut = true;
        public bool CheckingOut { get => _checkingOut; set { _checkingOut = value; OnPropertyChanged(nameof(CheckingOut)); } }

        private bool _currentlyHosting;
        public bool CurrentlyHosting { get => _currentlyHosting; set { _currentlyHosting = value; OnPropertyChanged(nameof(CurrentlyHosting)); } }

        private bool _arrivingSoon;
        public bool ArrivingSoon { get => _arrivingSoon; set { _arrivingSoon = value; OnPropertyChanged(nameof(ArrivingSoon)); } }

        private bool _pendingReview;
        public bool PendingReview { get => _pendingReview; set { _pendingReview = value; OnPropertyChanged(nameof(PendingReview)); } }

        private string _ownerName = "";
        public string OwnerName { get => _ownerName; set { _ownerName = value; OnPropertyChanged(nameof(OwnerName)); } }

        #endregion

        User CurrentUser { get; set; }
        public NavigationService NavService;

        private List<HomeDTO> Reservations { get; set; } = new();

        private readonly AccommodationReservationService _reservationService = new();
        private readonly UserService _userService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();

        public ICommand CheckingOutCommand => new FunctionCommand(CheckingOutAction);
        public ICommand CurrentlyHostingCommand => new FunctionCommand(CurrentlyHostingAction);
        public ICommand ArrivingSoonCommand => new FunctionCommand(ArrivingSoonAction);
        public ICommand PendingReviewCommand => new FunctionCommand(PendingReviewAction);
        public ICommand OpenReviewCommand => new FunctionArgCommand(obj => { if (obj is int o) NavService.Navigate(new ReviewGuestView(CurrentUser, NavService, o)); });
        

        public HomeViewModel(User currentUser, NavigationService navService)
        {
            CurrentUser = currentUser;
            NavService = navService;
            OwnerName = CurrentUser.FirstName;

            FillCollections();
            CheckingOutAction();
        }

        private void CheckingOutAction()
        {
            CheckingOut = true;
            CurrentlyHosting = false;
            ArrivingSoon = false;
            PendingReview = false;

            CurrentReservations.Clear();
            foreach (var reservation in Reservations.Where(r => r.ReservationKind == HomeDtoKind.CheckingOut))
                CurrentReservations.Add(reservation);
        }

        private void CurrentlyHostingAction()
        {
            CheckingOut = false;
            CurrentlyHosting = true;
            ArrivingSoon = false;
            PendingReview = false;

            CurrentReservations.Clear();
            foreach (var reservation in Reservations.Where(r => r.ReservationKind == HomeDtoKind.CurrentlyHosting))
                CurrentReservations.Add(reservation);
        }

        private void ArrivingSoonAction()
        {
            CheckingOut = false;
            CurrentlyHosting = false;
            ArrivingSoon = true;
            PendingReview = false;

            CurrentReservations.Clear();
            foreach (var reservation in Reservations.Where(r => r.ReservationKind == HomeDtoKind.ArrivingSoon))
                CurrentReservations.Add(reservation);
        }

        private void PendingReviewAction()
        {
            CheckingOut = false;
            CurrentlyHosting = false;
            ArrivingSoon = false;
            PendingReview = true;

            CurrentReservations.Clear();
            foreach (var reservation in Reservations.Where(r => r.ReservationKind == HomeDtoKind.PendingReview))
                CurrentReservations.Add(reservation);
        }

        private void FillCollections()
        {
            var reservations = _reservationService.GetAllReservationsForOwner(CurrentUser.Id);
            Reservations = AccommodationReservationUtils.FillHomeDto(reservations, _userService, _accommodationService, _pictureService);
        }
    }
}
