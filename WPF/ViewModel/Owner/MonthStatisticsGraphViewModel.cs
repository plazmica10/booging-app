using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class MonthStatisticsGraphViewModel : ViewModelBase
    {
        #region Bindings

        public ObservableCollection<MonthStatisticsDto> MonthStatistics { get; set; } = new();

        private string _accommodationName = "";
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { if (value != _currentPhoto) { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } } }

        private string _bestMonth = "";
        public string BestMonth { get => _bestMonth; set { _bestMonth = value; OnPropertyChanged(nameof(BestMonth)); } }

        private SeriesCollection _pieChartSeries = new();
        public SeriesCollection PieChartSeries { get => _pieChartSeries; set { _pieChartSeries = value; OnPropertyChanged(nameof(PieChartSeries)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;
        public int AccommodationId;
        public int Year;
        private List<string> _photos = new();

        private readonly AccommodationStatisticsService _statisticsService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();

        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);
        public ICommand BackCommand => new FunctionCommand(() => NavService.GoBack());
        public ICommand MonthTableCommand => new FunctionCommand(() => NavService.Navigate(new MonthStatisticsView(CurrentUser, NavService, AccommodationId, Year)));
        public ICommand MonthGraphCommand => new FunctionCommand(() => NavService.Navigate(new MonthStatisticsGraphView(CurrentUser, NavService, AccommodationId, Year)));

        public MonthStatisticsGraphViewModel(User currentUser, NavigationService navService, int accommodationId, int year)
        {
            CurrentUser = currentUser;
            NavService = navService;
            AccommodationId = accommodationId;
            Year = year;

            LoadPageInfo(accommodationId);
            LoadMonthStatistics(accommodationId, year);
            LoadBestMonth(accommodationId, year);
            DrawPieChart();
        }

        private void DrawPieChart()
        {
            PieChartSeries.Clear();
            foreach (var month in MonthStatistics)
            {
                PieChartSeries.Add(new PieSeries { Title = month.MonthName, Values = new ChartValues<int> { month.DaysOccupied } } );
            }
        }

        private void LoadMonthStatistics(int accommodationId, int year)
        {
            MonthStatistics = new ObservableCollection<MonthStatisticsDto>(_statisticsService.GetMonthStatisticsForOwner(accommodationId, year));
        }

        private void LoadBestMonth(int accommodationId, int year)
        {
            BestMonth = _statisticsService.GetBestMonth(accommodationId, year);
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
