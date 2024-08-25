using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {
        public HomeView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new HomeViewModel(user, nav);
        }
    }
}
