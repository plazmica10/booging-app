using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for RenovationsView.xaml
    /// </summary>
    public partial class RenovationsView : Page
    {
        public RenovationsView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new RenovationsViewModel(user, nav);
        }
    }
}
