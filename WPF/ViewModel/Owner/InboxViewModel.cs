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
    public class InboxViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<InboxRequestDTO> _requests = new();
        public ObservableCollection<InboxRequestDTO> Requests { get => _requests; set { _requests = value; OnPropertyChanged(nameof(Requests)); } }

        private RequestBigDTO? _selectedRequest;
        public RequestBigDTO? SelectedRequest { get => _selectedRequest; set { _selectedRequest = value; OnPropertyChanged(nameof(SelectedRequest)); } }

        private bool _isPopupOpen;
        public bool IsPopupOpen { get => _isPopupOpen; set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); } }

        private bool _isSuccessApproveOpen;
        public bool IsSuccessApproveOpen { get => _isSuccessApproveOpen; set { _isSuccessApproveOpen = value; OnPropertyChanged(nameof(IsSuccessApproveOpen)); } }

        private bool _isSuccessDenyOpen;
        public bool IsSuccessDenyOpen { get => _isSuccessDenyOpen; set { _isSuccessDenyOpen = value; OnPropertyChanged(nameof(IsSuccessDenyOpen)); } }

        private string _denyComment = "";
        public string DenyComment { get => _denyComment; set { _denyComment = value; OnPropertyChanged(nameof(DenyComment)); } }
        
        #endregion

        private readonly RescheduleRequestService _rescheduleRequestService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public User CurrentUser;
        public NavigationService NavService;

        public ICommand OpenReq => new FunctionArgCommand(obj => { if (obj is int o) OpenRequest(o); });
        public ICommand ApproveCommand => new FunctionCommand(AcceptRequest);
        public ICommand DenyCommand => new FunctionCommand(() => IsPopupOpen = true);
        public ICommand SubmitDenyCommand => new FunctionCommand(SubmitDeny);
        public ICommand ClosePopupCommand => new FunctionCommand(() => IsPopupOpen = false);
        public ICommand ConfirmSuccessCommand => new FunctionCommand(ConfirmSuccess);
        public ICommand DetailsCommand => new FunctionCommand(() => NavService.Navigate(new AccommodationView(CurrentUser, NavService, SelectedRequest.AccommodationId)));

        public InboxViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;

            Requests = new ObservableCollection<InboxRequestDTO>();

            FillRequests();
        }

        private void ConfirmSuccess()
        {
            IsSuccessApproveOpen = false;
            IsSuccessDenyOpen = false;

            NavService.Navigate(new InboxView(CurrentUser, NavService));
        }

        private void SubmitDeny()
        {
            var request = _rescheduleRequestService.GetById(SelectedRequest.RequestId);
            request.Status = RequestStatus.Denied;
            request.OwnersComment = DenyComment;
            _rescheduleRequestService.Update(request);

            IsPopupOpen = false;
            IsSuccessDenyOpen = true;
        }

        private void AcceptRequest()
        {
            var request = _rescheduleRequestService.GetById(SelectedRequest.RequestId);
            var reservation = _accommodationReservationService.GetById(request.ReservationId);

            request.Status = RequestStatus.Approved;
            _rescheduleRequestService.Update(request);

            reservation.CheckIn = request.NewStartDate;
            reservation.CheckOut = request.NewEndDate;
            _accommodationReservationService.Update(reservation);

            IsSuccessApproveOpen = true;
        }

        public void OpenRequest(int requestId)
        {
            foreach (var r in Requests)
            {
                if (r.RequestId != requestId)
                    r.Selected = false;
                else
                    r.Selected = true;
            }

            SelectedRequest = OwnerInboxUtils.FillSelectedRequest(Requests.First(r => r.Selected));
        }

        private void FillRequests()
        {
            var requests = new ObservableCollection<RescheduleRequest>(_rescheduleRequestService.GetByOwnerId(CurrentUser.Id));
            foreach (var request in requests)
            {
                var reservation = _accommodationReservationService.GetById(request.ReservationId);
                var user = _userService.GetById(reservation.GuestId);
                Requests.Add(new InboxRequestDTO
                {
                    RequestId = request.Id, UserName = user.FirstName + " " + user.LastName, UserPhoto = _pictureService.GetPictureLocation(user.Photo), Status = request.Status, Selected = false
                });
            }
            if (Requests.Count > 0) Requests.First(r => r.Status == RequestStatus.Pending).Selected = true;
            SelectedRequest = OwnerInboxUtils.FillSelectedRequest(Requests.First(r => r.Selected));
        }
    }
}
