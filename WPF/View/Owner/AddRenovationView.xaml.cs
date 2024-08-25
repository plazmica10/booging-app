using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AddRenovationView.xaml
    /// </summary>
    public partial class AddRenovationView : Page
    {
        public AddRenovationView(User user, NavigationService nav, AccommodationDTO accommodation)
        {
            InitializeComponent();
            DataContext = new AddRenovationViewModel(user, nav, accommodation);
        }
    }
}
