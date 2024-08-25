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
    /// Interaction logic for ToursAllView.xaml
    /// </summary>
    public partial class ToursAllView : Page
    {
        public ToursAllView(User currentUser, NavigationService navigationService, NavigationService tourNavigationService)
        {
            InitializeComponent();
            this.DataContext = new ToursAllViewModel(currentUser, navigationService, tourNavigationService);
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
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null)
                {
                    var btnStartTour = GetStartTourButton(dataGrid, dataGrid.SelectedItem);
                    if (btnStartTour != null)
                    {
                        btnStartTour.Focus();
                    }
                }
            }
        }

        private Button GetStartTourButton(DataGrid dataGrid, object selectedItem)
        {
            if (dataGrid.ItemContainerGenerator.ContainerFromItem(selectedItem) is FrameworkElement container)
            {
                return FindChild<Button>(container, "btnStartTour");
            }
            return null;
        }


        private T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T childElement && (childElement as FrameworkElement)?.Name == childName)
                {
                    return childElement;
                }
                else
                {
                    var result = FindChild<T>(child, childName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

       
    }
}
