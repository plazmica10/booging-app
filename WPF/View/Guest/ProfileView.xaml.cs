using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        public ProfileView(User user,NavigationService navService)
        {
            InitializeComponent();
            DataContext = new ViewModel.Guest.ProfileViewModel(user, navService);
        }
    }
}
