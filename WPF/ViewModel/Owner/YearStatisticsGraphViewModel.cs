using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class YearStatisticsGraphViewModel : ViewModelBase
    {
        #region Bindings

        public ObservableCollection<YearStatisticsDto> YearStatistics { get; set; } = new();

        private string _accommodationName = "";
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { if (value != _currentPhoto) { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } } }

        private int _bestYear;
        public int BestYear { get => _bestYear; set { _bestYear = value; OnPropertyChanged(nameof(BestYear)); } }

        private SeriesCollection _barChartSeries = new();
        public SeriesCollection BarChartSeries { get => _barChartSeries; set { _barChartSeries = value; OnPropertyChanged(nameof(BarChartSeries)); } }

        private List<string> _yearLabels = new();
        public List<string> YearLabels { get => _yearLabels; set { _yearLabels = value; OnPropertyChanged(nameof(YearLabels)); } }

        private bool _reservations = true;
        public bool Reservations { get => _reservations; set { _reservations = value; OnPropertyChanged(nameof(Reservations)); } }

        private bool _cancellations;
        public bool Cancellations { get => _cancellations; set { _cancellations = value; OnPropertyChanged(nameof(Cancellations)); } }

        private bool _reschedulings;
        public bool Reschedulings { get => _reschedulings; set { _reschedulings = value; OnPropertyChanged(nameof(Reschedulings)); } }

        private bool _renovations;
        public bool Renovations { get => _renovations; set { _renovations = value; OnPropertyChanged(nameof(Renovations)); } }

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
        public ICommand ReservationsCommand => new FunctionCommand(() => { DisableButtons(); Reservations = true; DrawBarChart(); });
        public ICommand CancellationsCommand => new FunctionCommand(() => { DisableButtons(); Cancellations = true; DrawBarChart(); });
        public ICommand ReschedulingsCommand => new FunctionCommand(() => { DisableButtons(); Reschedulings = true; DrawBarChart(); });
        public ICommand RenovationsCommand => new FunctionCommand(() => { DisableButtons(); Renovations = true; DrawBarChart(); });

        public YearStatisticsGraphViewModel(User currentUser, NavigationService navService, int accommodationId)
        {
            CurrentUser = currentUser;
            NavService = navService;
            _accommodationId = accommodationId;

            LoadPageInfo(accommodationId);
            LoadYearStatistics(accommodationId);
            LoadBestYear(accommodationId);
            DrawBarChart();
        }

        private void DrawBarChart()
        {
            BarChartSeries.Clear();
            YearLabels.Clear();
            string title = GetTitle();
            var chartValues = new ChartValues<double>();
            YearStatistics.Zip(YearStatistics, (year, label) =>
            {
                chartValues.Add(title switch { nameof(Reservations) => year.Reservations, nameof(Cancellations) => year.Cancellations, nameof(Reschedulings) => year.ReschedulingCount, nameof(Renovations) => year.RenovationCount, _ => 0 });
                YearLabels.Add(label.Year.ToString());
                return (year, label);
            }).ToList();
            BarChartSeries.Add(new ColumnSeries { Title = GetTitle(), Values = chartValues });
        }

        private string GetTitle()
        {
            if (Reservations) return nameof(Reservations);
            if (Cancellations) return nameof(Cancellations);
            if (Reschedulings) return nameof(Reschedulings);
            return nameof(Renovations);
        }

        public Func<double, string> YFormatter { get; } = value => value.ToString("0");

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

        private void DisableButtons() { Reservations = false; Cancellations = false; Reschedulings = false; Renovations = false; }
    }
}
