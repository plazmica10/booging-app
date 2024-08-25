using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    public partial class AccommodationView : Page
    {
        public AccommodationView(User currentUser, Accommodation accommodation, NavigationService navService)
        {
            InitializeComponent();
            DataContext = new AccommodationViewModel(currentUser, accommodation, navService);
        }
    }
}
