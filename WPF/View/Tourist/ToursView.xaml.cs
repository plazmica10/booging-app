using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ToursPage.xaml
    /// </summary>
    public partial class ToursView : Page
    {
        public ToursView(User currentUser, NavigationService navigationService, List<Tour>? tours = null)
        {
            InitializeComponent();
            DataContext = new ToursViewModel(currentUser, navigationService, tours);
        }
    }
}
