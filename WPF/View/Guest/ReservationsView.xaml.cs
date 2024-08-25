using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReservationsView.xaml
    /// </summary>
    public partial class ReservationsView : Page
    {
        public ReservationsView(User currentUser, NavigationService navService)
        {
            InitializeComponent();
            DataContext = new ReservationsViewModel(currentUser, navService);
        }
    }
}
