using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for NewForumsView.xaml
    /// </summary>
    public partial class NewForumsView : Page
    {
        public NewForumsView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new NewForumsViewModel(user, nav);
        }
    }
}
