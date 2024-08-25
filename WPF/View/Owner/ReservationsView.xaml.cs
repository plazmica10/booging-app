using BookingApp.Domain.Model;
using System.Windows.Controls;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ReservationsView.xaml
    /// </summary>
    public partial class ReservationsView : Page
    {
        public ReservationsView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new ReservationsViewModel(user, nav);
        }
    }
}
