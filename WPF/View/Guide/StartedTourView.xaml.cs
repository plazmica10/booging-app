using System.Linq;
using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for StartedTourView.xaml
    /// </summary>
    public partial class StartedTourView : Page
    {
        public StartedTourView(User currentUser, NavigationService navigationService, int tourId)
        {
            InitializeComponent();
            DataContext = new StartedTourViewModel(currentUser, navigationService, tourId);
            PeakPointsDataGrid.PreviewKeyDown += PeakPointsDataGrid_PreviewKeyDown;
            TouristsDataGrid.PreviewKeyDown += TouristsDataGrid_PreviewKeyDown;
            Loaded += StartedTourView_Loaded;
        }
        private void TouristsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Focus on the TouristsDataGrid
                TouristsDataGrid.Focus();

                // Check if there are any items in the data grid
                if (TouristsDataGrid.Items.Count > 0)
                {
                    // Select the first item
                    TouristsDataGrid.SelectedIndex = 0;
                }

                e.Handled = true;
            }
        }
        private void StartedTourView_Loaded(object sender, RoutedEventArgs e)
        {
            PeakPointsDataGrid.Focus();
        }

        private void PeakPointsDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Switch focus to TouristsDataGrid
                TouristsDataGrid.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Up || e.Key == Key.Down)
            {
                var dataGrid = sender as DataGrid;
                int currentIndex = dataGrid.SelectedIndex;
                int newIndex = currentIndex;

                // Adjust the index based on arrow key pressed
                if (e.Key == Key.Up && currentIndex > 0)
                {
                    newIndex = currentIndex - 1;
                }
                else if (e.Key == Key.Down && currentIndex < dataGrid.Items.Count - 1)
                {
                    newIndex = currentIndex + 1;
                }

                // Update the selected index and scroll into view
                if (newIndex != currentIndex)
                {
                    dataGrid.SelectedIndex = newIndex;
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    e.Handled = true;
                }
            }
        }

    }
}
