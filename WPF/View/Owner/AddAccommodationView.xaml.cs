using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodationView.xaml
    /// </summary>
    public partial class AddAccommodationView : Page
    {
        public AddAccommodationView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new AddAccommodationViewModel(user, nav);
        }
    }
}
