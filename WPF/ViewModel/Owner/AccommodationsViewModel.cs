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
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AccommodationsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<AccommodationsDTO>? _accommodations;
        public ObservableCollection<AccommodationsDTO>? Accommodations { get => _accommodations; set { _accommodations = value; OnPropertyChanged(nameof(Accommodations)); } }

        private bool _isSuccessCloseOpen;
        public bool IsSuccessCloseOpen { get => _isSuccessCloseOpen; set { _isSuccessCloseOpen = value; OnPropertyChanged(nameof(IsSuccessCloseOpen)); } }

        #endregion

        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();

        public User CurrentUser { get; set; }
        public NavigationService NavService;
        public int ClosedId;

        public ICommand AddAccommodationsCommand => new FunctionCommand(AddAccommodations);
        public ICommand OpenAccommodationCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenAccommodation(o); });
        public ICommand ConfirmSuccessCommand => new FunctionCommand(() => IsSuccessCloseOpen = false);
        public ICommand CancelCloseCommand => new FunctionCommand(CancelClose);

        public AccommodationsViewModel(User user, NavigationService navService, int closedId = -1)
        {
            CurrentUser = user;
            NavService = navService;

            FillDto();

            if (closedId != -1)
            {
                IsSuccessCloseOpen = true;
                ClosedId = closedId;
            }
        }

        private void CancelClose()
        {
            IsSuccessCloseOpen = false;
            var accommodation = _accommodationService.GetById(ClosedId);
            accommodation.IsDeleted = false;
            _accommodationService.Update(accommodation);
            NavService.Navigate(new AccommodationsView(CurrentUser, NavService));
        }

        private void AddAccommodations()
        {
            NavService.Navigate(new AddAccommodationView(CurrentUser, NavService));
        }

        private void OpenAccommodation(int accommodationId)
        {
            NavService.Navigate(new AccommodationView(CurrentUser, NavService, accommodationId));
        }

        private void FillDto()
        {
            Accommodations = new ObservableCollection<AccommodationsDTO>();
            var accommodations = _accommodationService.GetByOwnerId(CurrentUser.Id);
            foreach (var accommodation in accommodations)
            {
                if (accommodation.IsDeleted) continue;
                var dto = new AccommodationsDTO { Id = accommodation.Id, Name = accommodation.Name, Address = $"{accommodation.Location.City}, {accommodation.Location.Country}", Photo = _pictureService.GetPictureLocation(accommodation.Photos.First()) };
                Accommodations.Add(dto);
            }
        }
    }
}
