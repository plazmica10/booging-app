using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ReviewGuestView.xaml
    /// </summary>
    public partial class ReviewGuestView : Page
    {
        public ReviewGuestView(User user, NavigationService nav, int reservationId)
        {
            InitializeComponent();
            DataContext = new ReviewGuestViewModel(user, nav, reservationId);
        }
    }
}
