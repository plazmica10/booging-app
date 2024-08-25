using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationView.xaml
    /// </summary>
    public partial class AccommodationView : Page
    {
        public AccommodationView(User user, NavigationService nav, int accommodationId)
        {
            InitializeComponent();
            DataContext = new AccommodationViewModel(user, nav, accommodationId);
        }
    }
}
