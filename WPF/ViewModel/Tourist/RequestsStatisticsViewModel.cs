using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class RequestsStatisticsViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourStatisticsService _tourStatisticsService = new();

        private string _selectedYear = "All Time";
        public string SelectedYear { get => _selectedYear; set { _selectedYear = value; UpdateAction(); OnPropertyChanged(nameof(SelectedYear)); } }
        public RequestsStatisticsViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;
            UpdateAction();
        }

        private string _requestsCount = "";
        public string RequestsCount { get => _requestsCount; set { _requestsCount = value; OnPropertyChanged(nameof(RequestsCount)); } }

        private string _waitingCount = "";
        public string WaitingCount { get => _waitingCount; set { _waitingCount = value; OnPropertyChanged(nameof(WaitingCount)); } }

        private string _waitingPercentage = "";
        public string WaitingPercentage { get => _waitingPercentage; set { _waitingPercentage = value; OnPropertyChanged(nameof(WaitingPercentage)); } }

        private string _rejectedCount = "";
        public string RejectedCount { get => _rejectedCount; set { _rejectedCount = value; OnPropertyChanged(nameof(RejectedCount)); } }

        private string _rejectedPercentage = "";
        public string RejectedPercentage { get => _rejectedPercentage; set { _rejectedPercentage = value; OnPropertyChanged(nameof(RejectedPercentage)); } }

        private string _approvedCount = "";
        public string ApprovedCount { get => _approvedCount; set { _approvedCount = value; OnPropertyChanged(nameof(ApprovedCount)); } }

        private string _approvedPercentage = "";
        public string ApprovedPercentage { get => _approvedPercentage; set { _approvedPercentage = value; OnPropertyChanged(nameof(ApprovedPercentage)); } }

        private SeriesCollection _pieChartSeries = new();
        public SeriesCollection PieChartSeries { get => _pieChartSeries; set { _pieChartSeries = value; OnPropertyChanged(nameof(PieChartSeries)); } }

        private SeriesCollection _barChartLocationSeries = new();
        public SeriesCollection BarChartLocationSeries { get => _barChartLocationSeries; set { _barChartLocationSeries = value; OnPropertyChanged(nameof(BarChartLocationSeries)); } }

        private List<string> _barChartLocationLabels = new();
        public List<string> BarChartLocationLabels { get => _barChartLocationLabels; set { _barChartLocationLabels = value; OnPropertyChanged(nameof(BarChartLocationLabels)); } }

        private int _barChartLocationSeriesMin = 0;
        public int BarChartLocationSeriesMin { get => _barChartLocationSeriesMin; set { _barChartLocationSeriesMin = value; OnPropertyChanged(nameof(BarChartLocationSeriesMin)); } }

        private int _barChartLocationSeriesMax = 1;

        public int BarChartLocationSeriesMax
        {
            get => _barChartLocationSeriesMax;
            set
            {
                if (value <= 0) return;
                _barChartLocationSeriesMax = value;
                OnPropertyChanged(nameof(BarChartLocationSeriesMax));
            }
        }

        private SeriesCollection _barChartLanguageSeries = new();
        public SeriesCollection BarChartLanguageSeries { get => _barChartLanguageSeries; set { _barChartLanguageSeries = value; OnPropertyChanged(nameof(BarChartLanguageSeries)); } }

        private List<string> _barChartLanguageLabels = new();
        public List<string> BarChartLanguageLabels { get => _barChartLanguageLabels; set { _barChartLanguageLabels = value; OnPropertyChanged(nameof(BarChartLanguageLabels)); } }

        private int _barChartLanguageSeriesMin = 0;
        public int BarChartLanguageSeriesMin { get => _barChartLanguageSeriesMin; set { _barChartLanguageSeriesMin = value; OnPropertyChanged(nameof(BarChartLanguageSeriesMin)); } }

        private int _barChartLanguageSeriesMax = 1;

        public int BarChartLanguageSeriesMax
        {
            get => _barChartLanguageSeriesMax;
            set
            {
                if (value <= 0) return;
                _barChartLanguageSeriesMax = value;
                OnPropertyChanged(nameof(BarChartLanguageSeriesMax));
            }
        }

        public Func<double, string> XFormatter { get; } = value => value.ToString("0.##");

        public Func<double, string> YFormatter { get; } = value => value.ToString("0.##");

        private string _approvedPeopleCountMean = "";
        public string ApprovedPeopleCountMean { get => _approvedPeopleCountMean; set { _approvedPeopleCountMean = value; OnPropertyChanged(nameof(ApprovedPeopleCountMean)); } }

        public ObservableCollection<ItemRequestStatisticsLocationViewModel> LocationStatistics { get; set; } = new();
        public ObservableCollection<ItemRequestStatisticsLanguageViewModel> LanguageStatistics { get; set; } = new();

        private void UpdateAction()
        {
            int year = 0;
            int.TryParse(_selectedYear, out year);
            TouristRequestsStatisticsDto statistics = _tourStatisticsService.GetTouristStatistics(CurrentUser.Id, year);
            DrawLabels(statistics);
            DrawPieChart(statistics);
            DrawBarChart(statistics);
        }
        public void DrawLabels(TouristRequestsStatisticsDto statistics)
        {
            RequestsCount = statistics.RequestsCount.ToString();
            WaitingCount = statistics.WaitingCount.ToString();
            WaitingPercentage = statistics.WaitingCount <= 0 ? "" : (statistics.WaitingPercentage * 100).ToString("##.#") + "%";
            RejectedCount = statistics.RejectedCount.ToString();
            RejectedPercentage = statistics.RejectedCount <= 0 ? "" : (statistics.RejectedPercentage * 100).ToString("##.#") + "%";
            ApprovedCount = statistics.ApprovedCount.ToString();
            ApprovedPercentage = statistics.ApprovedCount <= 0 ? "" : (statistics.ApprovedPercentage * 100).ToString("##.#") + "%";
            ApprovedPeopleCountMean = statistics.ApprovedPeopleCountMean.ToString("##.#");
        }

        private void DrawPieChart(TouristRequestsStatisticsDto statistics)
        {

            PieChartSeries = new SeriesCollection
            {
                new PieSeries { Title = "Waiting",  Values = new ChartValues<double> { statistics.WaitingCount }, Fill = new SolidColorBrush(Colors.Yellow)},
                new PieSeries { Title = "Approved", Values = new ChartValues<double> { statistics.ApprovedCount }, Fill = new SolidColorBrush(Colors.Green)},
                new PieSeries { Title = "Rejected", Values = new ChartValues<double> { statistics.RejectedCount }, Fill = new SolidColorBrush(Colors.Red)}
            };
        }

        private void DrawBarChart(TouristRequestsStatisticsDto statistics)
        {
            LocationStatistics.Clear();
            BarChartLocationSeries.Clear();
            BarChartLocationLabels.Clear();
            BarChartLocationSeriesMax = statistics.LocationMax;
            ChartValues<double> locationValues = new ChartValues<double>();
            foreach (KeyValuePair<Location, int> locationCounter in statistics.LocationCounters.ToList())
            {
                locationValues.Add(locationCounter.Value);
                BarChartLocationLabels.Add(locationCounter.Key.ToReadableString());
                LocationStatistics.Add(new ItemRequestStatisticsLocationViewModel(locationCounter.Key, locationCounter.Value));
            }
            BarChartLocationSeries.Add(new RowSeries { Title = "Location count:", Values = locationValues });
            LanguageStatistics.Clear();
            BarChartLanguageLabels.Clear();
            BarChartLanguageSeries.Clear();
            BarChartLanguageSeriesMax = statistics.LanguageMax;
            ChartValues<double> languageValues = new ChartValues<double>();
            foreach (KeyValuePair<Language, int> languageCounter in statistics.LanguageCounters.ToList())
            {
                languageValues.Add(languageCounter.Value);
                BarChartLanguageLabels.Add(languageCounter.Key.ToReadableString());
                LanguageStatistics.Add(new ItemRequestStatisticsLanguageViewModel(languageCounter.Key, languageCounter.Value));
            }
            BarChartLanguageSeries.Add(new RowSeries { Title = "Language count:", Values = languageValues });
        }
    }
}
