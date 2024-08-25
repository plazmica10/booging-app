using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AccommodationViewModel : ViewModelBase
    {
        #region Bindings

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } }

        private bool _isAreYouSureOpen;
        public bool IsAreYouSureOpen { get => _isAreYouSureOpen; set { _isAreYouSureOpen = value; OnPropertyChanged(nameof(IsAreYouSureOpen)); } }

        #endregion

        public AccommodationDTO Accommodation { get; set; } = new();
        public User CurrentUser;
        public NavigationService NavService;

        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();
        private readonly AccommodationStatisticsService _statisticsService = new();

        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ICommand RenovateCommand  => new FunctionCommand(Renovate);
        public ICommand StatisticsCommand => new FunctionCommand(() => NavService.Navigate(new YearStatisticsView(CurrentUser, NavService, Accommodation.Id)));
        public ICommand PdfCommand => new FunctionCommand(ExportToPDF);
        public ICommand ReviewsCommand => new FunctionCommand(() => NavService.Navigate(new ReviewsView(CurrentUser, NavService, Accommodation.Name)));
        public ICommand RemoveCommand => new FunctionCommand(() => IsAreYouSureOpen = true);
        public ICommand SureCommand => new FunctionCommand(CloseAccommodation);
        public ICommand NotSureCommand => new FunctionCommand(() => IsAreYouSureOpen = false);

        public AccommodationViewModel(User user, NavigationService navService, int accommodationId)
        {
            CurrentUser = user;
            NavService = navService;

            FillAccommodation(accommodationId);
        }

        private void CloseAccommodation()
        {
            IsAreYouSureOpen = false;
            _accommodationService.CloseAccommodation(Accommodation.Id);
            NavService.Navigate(new AccommodationsView(CurrentUser, NavService, Accommodation.Id));
        }

        private void Renovate()
        {
            NavService.Navigate(new AddRenovationView(CurrentUser, NavService, Accommodation));
        }

        private void NextPhoto()
        {
            if (Accommodation.Photos.Count == 0) return;
            int index = Accommodation.Photos.IndexOf(CurrentPhoto);
            if (index == Accommodation.Photos.Count - 1)
                CurrentPhoto = Accommodation.Photos[0];
            else
                CurrentPhoto = Accommodation.Photos[index + 1];
        }

        private void PrevPhoto()
        {
            if (Accommodation.Photos.Count == 0) return;
            int index = Accommodation.Photos.IndexOf(CurrentPhoto);
            if (index == 0)
                CurrentPhoto = Accommodation.Photos.Last();
            else
                CurrentPhoto = Accommodation.Photos[index - 1];
        }

        private void FillAccommodation(int accommodationId)
        {
            var accommodation = _accommodationService.GetById(accommodationId);

            Accommodation = new AccommodationDTO { Id = accommodation.Id, Name = accommodation.Name, Location = $"{accommodation.Location.City}, {accommodation.Location.Country}", Type = accommodation.Type, MaxGuests = accommodation.MaxGuests, MinDays = accommodation.MinDays, RefundPeriod = accommodation.RefundPeriod, Photos = new ObservableCollection<string>(_pictureService.GetPictureLocations(accommodation.Photos)), Description = accommodation.Description };

            CurrentPhoto = Accommodation.Photos.FirstOrDefault();
        }

        private void ExportToPDF()
        {
            OwnerPdfUtils.ExportToPDF(CurrentUser, _statisticsService.GetYearStatisticsForOwner(CurrentUser.Id, Accommodation.Id), Accommodation);
        }
    }
}
