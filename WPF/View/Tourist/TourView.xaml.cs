using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourView.xaml
    /// </summary>
    public partial class TourView : Page
    {
        public TourView(User currentUser, NavigationService navigationService, int tourId)
        {
            InitializeComponent();
            this.DataContext = new TourViewModel(currentUser, navigationService, tourId);
        }
    }
}
