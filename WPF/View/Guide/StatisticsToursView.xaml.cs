using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.WPF.ViewModel.Guide;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;


namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for StatisticsToursView.xaml
    /// </summary>
    public partial class StatisticsToursView : Page
    {
        public StatisticsToursView(User currentUser, NavigationService navigation)
        {
            InitializeComponent();
            this.DataContext = new StatisticsToursViewModel(currentUser, navigation);

            ToursDataGrid.SelectionChanged += ToursDataGrid_SelectionChanged;

        }

        private void ToursDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected tour from the data grid
            ItemTourStatViewModel selectedTour = (ItemTourStatViewModel)ToursDataGrid.SelectedItem;
            if (selectedTour != null)
            {
                // Update the chart with statistics for the selected tour
                ShowStatisticsChart(selectedTour.Statistics);
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.SelectedIndex = 0; // Select the first item
            }
        }
        private void ShowStatisticsChart(TouristsAgeStatisticsDto statistics)
        {
            // Define custom colors for the pie chart
            var customColors = new List<Color>
            {
                Color.FromRgb(225,247,213),   
                Color.FromRgb(249,235,166),   
                Color.FromRgb(207,235,255)
            };

            // Configure the chart
            StatisticsChart.Series.Clear();
            StatisticsChart.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Bellow 18",
                    Values = new ChartValues<double> { statistics.Under18 },
                    Fill = new SolidColorBrush(customColors[0])
                },
                new PieSeries
                {
                    Title = "Between 18 And 50",
                    Values = new ChartValues<double> { statistics.Between18And50 },
                    Fill = new SolidColorBrush(customColors[1])
                },
                new PieSeries
                {
                    Title = "Above 50",
                    Values = new ChartValues<double> { statistics.Above50 },
                    Fill = new SolidColorBrush(customColors[2])
                }
            };

            // Set legend
            StatisticsChart.LegendLocation = LegendLocation.Right;
        }

    }
}

