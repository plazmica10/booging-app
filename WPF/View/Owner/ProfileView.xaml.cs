using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        public ProfileView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new ProfileViewModel(user, nav);
        }
    }
}
