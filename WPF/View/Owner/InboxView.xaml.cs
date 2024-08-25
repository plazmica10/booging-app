using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for InboxView.xaml
    /// </summary>
    public partial class InboxView : Page
    {
        public InboxView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new InboxViewModel(user, nav);
        }
    }
}
