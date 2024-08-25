using System;
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
using MenuNavigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class RescheduleViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService = new();
        private readonly RescheduleRequestService _rescheduleRequestService = new();
        public NavigationService NavigationService;
        public User CurrentUser;
        public AccommodationReservation Reservation;
        public RescheduleViewModel(User user, NavigationService navService,AccommodationReservation reservation)
        {
            CurrentUser = user;
            NavigationService = navService;
            Reservation = reservation;
            Details = _rescheduleRequestService.GetRequestDto(Reservation.AccommodationId,Reservation.CheckIn,Reservation.CheckOut);
            SortedImages = new ObservableCollection<string>(Details.Images.OrderBy(x => x));
            CurrentImage = SortedImages.First();
            _newCheckIn = Reservation.CheckIn;
            _newCheckOut = Reservation.CheckOut;
            PreviousCommand = new FunctionCommand(PreviousImage);
            NextCommand = new FunctionCommand(NextImage);
            RescheduleCommand = new FunctionCommand(RescheduleAction);

        }
        public ICommand RescheduleCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand NextCommand { get; set; }
        private RescheduleRequestsPageDto _details;
        public RescheduleRequestsPageDto Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }
        private string _currentImage;
        public string CurrentImage { get => _currentImage; set { _currentImage = value; OnPropertyChanged(nameof(CurrentImage)); } }
        public ObservableCollection<string> SortedImages { get; set; }
        public int ImageCount { get { return SortedImages?.Count ?? 0; } }
        private DateTime _newCheckIn;
        public DateTime NewCheckIn { get => _newCheckIn; set { if (value != _newCheckIn) { _newCheckIn = value; OnPropertyChanged(nameof(NewCheckIn)); } } }
        private DateTime _newCheckOut;
        public DateTime NewCheckOut { get => _newCheckOut; set { if (value != _newCheckOut) { _newCheckOut = value; OnPropertyChanged(nameof(NewCheckOut)); } } }
        public void RescheduleAction()
        {
            if (!ReservationUtils.ValidateReschedulingDates(NewCheckIn, NewCheckOut,Reservation)) return;
            try
            {
                RescheduleRequest request = new RescheduleRequest() { UserId = CurrentUser.Id, OwnerId = Reservation.OwnerId, ReservationId = Reservation.Id, AccommodationId = Reservation.AccommodationId, NewStartDate = NewCheckIn, NewEndDate = NewCheckOut, OwnersComment = "", Status = RequestStatus.Pending };
                _reservationService.SaveRequest(request);
                MessageBox.Show(TranslationSource.Instance["ReqSent"]);
            }
            catch { MessageBox.Show(TranslationSource.Instance["CommentError"]); }
        }
        private void PreviousImage() { CurrentImage = ImageControl.GetPreviousImage(SortedImages, CurrentImage); }
        private void NextImage() { CurrentImage = ImageControl.GetNextImage(SortedImages, CurrentImage); }
    }
}
