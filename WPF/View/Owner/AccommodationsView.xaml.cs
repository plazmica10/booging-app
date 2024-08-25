using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationsView.xaml
    /// </summary>
    public partial class AccommodationsView : Page
    {
        public AccommodationsView(User user, NavigationService nav, int closedId = -1)
        {
            InitializeComponent();
            DataContext = new AccommodationsViewModel(user, nav, closedId);
        }
    }
}
