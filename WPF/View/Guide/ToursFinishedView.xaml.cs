using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for ToursFinishedView.xaml
    /// </summary>
    public partial class ToursFinishedView : Page
    {
        public ToursFinishedView(User currentUser, NavigationService navigation,NavigationService tourNavigationService)
        {
            InitializeComponent();
            this.DataContext = new ToursFinishedViewModel(currentUser, navigation, tourNavigationService);
        }
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.Items.Count > 0)
            {
                dataGrid.Focus();
                dataGrid.SelectedItem = dataGrid.Items[0];

                if (!IsItemFullyVisible(dataGrid, dataGrid.SelectedItem))
                {
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                }
            }
        }
        private bool IsItemFullyVisible(DataGrid dataGrid, object item)
        {
            if (dataGrid.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement container)
            {
                var transform = container.TransformToAncestor((Visual)dataGrid);
                var rect = transform.TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));
                return rect.Top >= 0 && rect.Bottom <= dataGrid.ActualHeight;
            }
            return false;
        }
        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = null;
            if (sender is DataGrid dg)
            {
                dataGrid = dg; // Assign dg to dataGrid
                int currentIndex = dataGrid.SelectedIndex;
                if (e.Key == Key.Up && currentIndex > 0)
                {
                    dataGrid.SelectedIndex = currentIndex - 1;
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    e.Handled = true;
                }
                else if (e.Key == Key.Down && currentIndex < dataGrid.Items.Count - 1)
                {
                    dataGrid.SelectedIndex = currentIndex + 1;
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    e.Handled = true;
                }
                else if (e.Key == Key.Right && dataGrid.SelectedItem != null)
                {
                    // Navigate to the ListView
                    ReviewsListView.Focus();
                    if (ReviewsListView.Items.Count > 0)
                    {
                        ReviewsListView.SelectedItem = ReviewsListView.Items[0];
                    }
                    e.Handled = true;
                }
                else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.B)
                {
                    if (ReviewsListView.SelectedItems.Count > 0)
                    {
                        foreach (var selectedItem in ReviewsListView.SelectedItems)
                        {
                            if (selectedItem is TourReview review)
                            {

                                if (!review.IsValid)
                                {
                                    ((ToursFinishedViewModel)DataContext).MarkInvalidReviewCommand.Execute(review);
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("This item is already marked as invalid.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                           
                        }
                    }
                    e.Handled = true;
                }
            }
            else if (sender is ListView listView && e.Key == Key.Left)
            {
                // Navigate back to the DataGrid
                dataGrid.Focus();
                if (dataGrid.Items.Count > 0)
                {
                    dataGrid.SelectedItem = dataGrid.Items[0];
                }
                e.Handled = true;
            }

        }

        /*
                private void DataGrid_KeyDown(object sender, KeyEventArgs e)
                {
                    if (sender is DataGrid dataGrid)
                    {
                        int currentIndex = dataGrid.SelectedIndex;
                        if (e.Key == Key.Up && currentIndex > 0)
                        {
                            dataGrid.SelectedIndex = currentIndex - 1;
                            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                            e.Handled = true;
                        }
                        else if (e.Key == Key.Down && currentIndex < dataGrid.Items.Count - 1)
                        {
                            dataGrid.SelectedIndex = currentIndex + 1;
                            dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                            e.Handled = true;
                        }
                    }
                }*/

    }
}
