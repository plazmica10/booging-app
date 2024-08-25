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
    public class YearStatisticsViewModel : ViewModelBase
    {
        #region Bindings

        public ObservableCollection<YearStatisticsDto> YearStatistics { get; set; } = new();

        private string _accommodationName = "";
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { if (value != _currentPhoto) { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } } }

        private int _bestYear;
        public int BestYear { get => _bestYear; set { _bestYear = value; OnPropertyChanged(nameof(BestYear)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;
        private List<string> _photos = new();
        private readonly int _accommodationId;

        private readonly AccommodationStatisticsService _statisticsService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();

        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ICommand OpenYearCommand => new FunctionArgCommand(obj => { if (obj is int o) NavService.Navigate(new MonthStatisticsView(CurrentUser, NavService, _accommodationId, o)); });
        public ICommand BackCommand => new FunctionCommand(() => NavService.GoBack());
        public ICommand YearTableCommand => new FunctionCommand(() => NavService.Navigate(new YearStatisticsView(CurrentUser, NavService, _accommodationId)));
        public ICommand YearGraphCommand => new FunctionCommand(() => NavService.Navigate(new YearStatisticsGraphView(CurrentUser, NavService, _accommodationId)));

        public YearStatisticsViewModel(User currentUser, NavigationService navService, int accommodationId)
        {
            CurrentUser = currentUser;
            NavService = navService;
            _accommodationId = accommodationId;

            LoadPageInfo(accommodationId);
            LoadYearStatistics(accommodationId);
            LoadBestYear(accommodationId);
        }

        private void LoadYearStatistics(int accommodationId)
        {
            YearStatistics = new ObservableCollection<YearStatisticsDto>(_statisticsService.GetYearStatisticsForOwner(CurrentUser.Id, accommodationId));
        }

        private void LoadBestYear(int accommodationId)
        {
            BestYear = _statisticsService.GetBestYear(CurrentUser.Id, accommodationId);
        }

        private void LoadPageInfo(int accommodationId)
        {
            Accommodation accommodation = _accommodationService.GetById(accommodationId);
            AccommodationName = accommodation.Name;
            _photos = _pictureService.GetPictureLocations(accommodation.Photos);
            if (_photos.Count >= 0)
                CurrentPhoto = _photos[0];
        }

        private void NextPhoto()
        {
            if (_photos.Count == 0) return;
            int index = _photos.IndexOf(CurrentPhoto);
            if (index == _photos.Count - 1)
                CurrentPhoto = _photos[0];
            else
                CurrentPhoto = _photos[index + 1];
        }

        private void PrevPhoto()
        {
            if (_photos.Count == 0) return;
            int index = _photos.IndexOf(CurrentPhoto);
            if (index == 0)
                CurrentPhoto = _photos.Last();
            else
                CurrentPhoto = _photos[index - 1];
        }
    }
}
