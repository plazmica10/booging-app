using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.WPF.View.Guide;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Colors = QuestPDF.Helpers.Colors;
using Location = BookingApp.Domain.Model.Location;



namespace BookingApp.WPF.ViewModel.Guide
{
    public class StatisticsRequestsViewModel: ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        private readonly LocationService _locationService = new();
        private readonly LanguageService _languageService = new();
        private readonly TourStatisticsService _tourStatisticsService = new();

        public ObservableCollection<ItemTourRequestViewModel> Requests { get; set; } = new();
      
        private string _name = "";
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
      
        private string _peopleCount = "";
        public string PeopleCount { get => _peopleCount; set { _peopleCount = value; OnPropertyChanged(nameof(PeopleCount)); } }
        private string _location = "";
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }
        private string _language = "";
        public string Language { get => _language; set { _language = value; OnPropertyChanged(nameof(Language)); } }

        public int SearchType { get; set; } = 0;

        private string _filterLocationOrLanguage = "";
        public string FilterLocationOrLanguage { get => _filterLocationOrLanguage; set { _filterLocationOrLanguage = value; OnPropertyChanged(nameof(FilterLocationOrLanguage)); } }
        private string? _filterPeopleCount;
        public string? FilterPeopleCount { get => _filterPeopleCount; set { _filterPeopleCount = value; OnPropertyChanged(nameof(FilterPeopleCount)); } }
        private string _selectedYear = "";
        public string SelectedYear { get => _selectedYear; set { _selectedYear = value; SearchAction(); OnPropertyChanged(nameof(SelectedYear)); } }
        
        public ICommand AboutToursCommand { get; }
        public ICommand SearchCommand { get; }

        public StatisticsRequestsViewModel(User currentUser, NavigationService navigation )
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            AboutToursCommand = new FunctionCommand(AboutToursAction);
            SearchCommand = new FunctionCommand(SearchAction);
            ExportStatisticsCommand = new RelayCommand(ExportStatistics, ExportStatisticsCommandCanExecute);
            SearchAction();
        }

        private SeriesCollection _barChartSeries = new();
        public SeriesCollection BarChartSeries { get => _barChartSeries; set { _barChartSeries = value; OnPropertyChanged(nameof(BarChartSeries)); } }

        private List<string> _barChartLabels = new();
        public List<string> BarChartLabels { get => _barChartLabels; set { _barChartLabels = value; OnPropertyChanged(nameof(BarChartLabels)); } }

        private int _barChartSeriesMin = 0;
        public int BarChartSeriesMin { get => _barChartSeriesMin; set { _barChartSeriesMin = value; OnPropertyChanged(nameof(BarChartSeriesMin)); } }

        private int _barChartSeriesMax = 1;

        public int BarChartSeriesMax { get => _barChartSeriesMax; set{ if (value <= 0) return; _barChartSeriesMax = value; OnPropertyChanged(nameof(BarChartSeriesMax)); } }
       
        private void AboutToursAction() { NavigationService.Navigate(new StatisticsToursView(CurrentUser, NavigationService)); }

        public static string GetMonthName(int month)
        {
            if (month < 1 || month > 12) { return "UNKNOWN"; }
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        private void SearchAction()
        {
            Location? filterLocation = null;
            Language? filterLanguage = null;

            if (!string.IsNullOrEmpty(_filterLocationOrLanguage))
            {
                switch (SearchType)
                {
                    case 0: filterLocation = _locationService.SearchByCityCountry(_filterLocationOrLanguage); break;
                    case 1: filterLanguage = _languageService.SearchByName(_filterLocationOrLanguage);        break;
                }
            }

            int selectedYear = 0;
            if (int.TryParse(SelectedYear, out int year)) { selectedYear = year; }

            Requests.Clear();
            GuideRequestsStatisticsDto guideRequestsStatisticsDto = _tourStatisticsService.GetGuideStatistics(selectedYear, filterLocation, filterLanguage);
            foreach (TourRequest request in guideRequestsStatisticsDto.FilteredTourRequests) { Requests.Add(new ItemTourRequestViewModel(request)); }

            BarChartSeries.Clear();
            BarChartLabels.Clear();
            ChartValues<double> values = new ChartValues<double>();

            try
            {
                if (year == 0)
                {
                    BarChartSeriesMax = guideRequestsStatisticsDto.YearCounter.Values.Max();
                    foreach (KeyValuePair<int, int> yearCounter in guideRequestsStatisticsDto.YearCounter){ values.Add(yearCounter.Value); BarChartLabels.Add(yearCounter.Key.ToString());}
                    
                    BarChartSeries.Add(new ColumnSeries() { Title = "Year count:", Values = values });
                }
                else
                {
                    BarChartSeriesMax = guideRequestsStatisticsDto.MonthCounter.Values.Max();
                    foreach (KeyValuePair<int, int> monthCounter in guideRequestsStatisticsDto.MonthCounter){ values.Add(monthCounter.Value); BarChartLabels.Add(GetMonthName(monthCounter.Key).ToString()); }
                    
                    BarChartSeries.Add(new ColumnSeries() { Title = "Month count:", Values = values });
                }
            }catch (Exception e) { }
        }
        public ICommand ExportStatisticsCommand { get; }

        public bool ExportStatisticsCommandCanExecute(object? argument)
        {
            return BarChartSeries.Count > 0 && BarChartLabels.Count > 0;
        }

      

        public void ExportStatistics(object? argument)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            if (Requests.Count > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = "Statistics.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);

                            page.Content().Padding(20).Column(column =>
                            {
                                column.Spacing(10);

                                column.Item().Text("Tour Requests Statistics").FontSize(25).Bold().AlignCenter();

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(2); 
                                        columns.RelativeColumn(1); 
                                        columns.RelativeColumn(2);
                                        columns.RelativeColumn(2); 
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("Name").FontSize(12).Bold();
                                        header.Cell().Element(CellStyle).Text("People Count").FontSize(12).Bold();
                                        header.Cell().Element(CellStyle).Text("Location").FontSize(12).Bold();
                                        header.Cell().Element(CellStyle).Text("Language").FontSize(12).Bold();

                                        static IContainer CellStyle(IContainer container) => container
                                            .PaddingVertical(5)
                                            .PaddingHorizontal(5)
                                            .Background(Colors.Grey.Lighten3)
                                            .Border(1)
                                            .BorderColor(Colors.Grey.Lighten1);
                                    });

                                    foreach (var request in Requests)
                                    {
                                        table.Cell().Element(CellStyle).Text(request.Name).FontSize(10);
                                        table.Cell().Element(CellStyle).Text(request.PeopleCount).FontSize(10);
                                        table.Cell().Element(CellStyle).Text(request.Location).FontSize(10);
                                        table.Cell().Element(CellStyle).Text(request.Language).FontSize(10);

                                        static IContainer CellStyle(IContainer container) => container
                                            .PaddingVertical(5)
                                            .PaddingHorizontal(5)
                                            .BorderBottom(1)
                                            .BorderColor(Colors.BlueGrey.Darken1);
                                    }

                                    
                                });
                            });
                        });
                    });

                    document.GeneratePdf(saveFileDialog.FileName);
                    
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = saveFileDialog.FileName,
                        UseShellExecute = true
                    });
                }
            }
        }





    }

}

