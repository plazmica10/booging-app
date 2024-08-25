using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class RenovationsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<RenovationsDto>? _renovations;
        public ObservableCollection<RenovationsDto>? Renovations { get => _renovations; set { _renovations = value; OnPropertyChanged(nameof(Renovations)); } }

        private RenovationSelectedDto? _selectedRenovation;
        public RenovationSelectedDto? SelectedRenovation { get => _selectedRenovation; set { _selectedRenovation = value; OnPropertyChanged(nameof(SelectedRenovation)); } }

        private bool _isAreYouSureOpen;
        public bool IsAreYouSureOpen { get => _isAreYouSureOpen; set { _isAreYouSureOpen = value; OnPropertyChanged(nameof(IsAreYouSureOpen)); } }

        private bool _isSuccessOpen;
        public bool IsSuccessOpen { get => _isSuccessOpen; set { _isSuccessOpen = value; OnPropertyChanged(nameof(IsSuccessOpen)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly AccommodationRenovationService _renovationService = new();
        private readonly AccommodationReservationService _reservationService = new();
        private readonly PictureService _pictureService = new();

        public ICommand OpenRenovationCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenRenovation(o); });
        public ICommand AccommodationDetailsCommand => new FunctionCommand(() => NavService.Navigate(new AccommodationView(CurrentUser, NavService, SelectedRenovation.AccommodationId)));
        public ICommand CancelRenovationCommand => new FunctionCommand(() => IsAreYouSureOpen = true);
        public ICommand ClosePopupCommand => new FunctionCommand(() => IsAreYouSureOpen = false);
        public ICommand ConfirmCancelCommand => new FunctionCommand(CancelRenovation);
        public ICommand ConfirmSuccessCommand => new FunctionCommand(ConfirmSuccess);

        public RenovationsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;

            FillRenovations();
            OpenRenovation(Renovations.First().RenovationId);
        }

        private void ConfirmSuccess()
        {
            IsSuccessOpen = false;
            NavService.Navigate(new RenovationsView(CurrentUser, NavService));
        }

        private void CancelRenovation()
        {
            var renovation = _renovationService.GetById(SelectedRenovation.RenovationId);
            renovation.Status = RenovationStatus.Canceled;
            _renovationService.UpdateRenovation(renovation);
            var reservation = _reservationService.GetById(renovation.Reservation.Id);
            reservation.Status = ReservationStatus.RenovationOver;
            _reservationService.Update(reservation);

            IsAreYouSureOpen = false;
            IsSuccessOpen = true;
        }

        private void OpenRenovation(int renovationId)
        {
            foreach (var renovation in Renovations)
            {
                renovation.Selected = renovation.RenovationId == renovationId;
            }
            FillSelectedRenovation();
        }

        private void FillSelectedRenovation()
        {
            var selectedRenovation = Renovations.FirstOrDefault(x => x.Selected);
            if (selectedRenovation != null)
            {
                SelectedRenovation = new RenovationSelectedDto
                {
                    RenovationId = selectedRenovation.RenovationId, AccommodationId = selectedRenovation.AccommodationId,
                    AccommodationName = selectedRenovation.AccommodationName, Location = selectedRenovation.Location,
                    Type = selectedRenovation.Type, AccommodationPhoto = selectedRenovation.AccommodationPhoto,
                    StartDate = selectedRenovation.StartDate, EndDate = selectedRenovation.EndDate,
                    Description = selectedRenovation.Description, Status = selectedRenovation.Status
                };
            }
        }

        private void FillRenovations()
        {
            Renovations = new ObservableCollection<RenovationsDto>();
            var renovations = _renovationService.GetByUserId(CurrentUser.Id);
            foreach (var renovation in renovations)
            {
                Renovations.Add(new RenovationsDto
                {
                    RenovationId = renovation.Id, AccommodationId = renovation.AccommodationForRenovation.Id,
                    AccommodationName = renovation.AccommodationForRenovation.Name, Location = $"{renovation.AccommodationForRenovation.Location.City}, {renovation.AccommodationForRenovation.Location.Country}",
                    Type = renovation.AccommodationForRenovation.Type, AccommodationPhoto = _pictureService.GetPictureLocation(renovation.AccommodationForRenovation.Photos.First()),
                    StartDate = renovation.StartDate, EndDate = renovation.EndDate, Description = renovation.Description, Status = renovation.Status
                });
            }
        }
    }
}
