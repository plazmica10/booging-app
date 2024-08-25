using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for RescheduleView.xaml
    /// </summary>
    public partial class RescheduleView : Page
    {
        public RescheduleView(User user, NavigationService navService, AccommodationReservation reservation)
        {
            InitializeComponent();
            this.DataContext = new RescheduleViewModel(user, navService, reservation);
        }
    }
}
