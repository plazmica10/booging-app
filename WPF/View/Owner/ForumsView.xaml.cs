using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ForumsView.xaml
    /// </summary>
    public partial class ForumsView : Page
    {
        public ForumsView(User user, NavigationService nav, int forumId = -1)
        {
            InitializeComponent();
            DataContext = new ForumsViewModel(user, nav, forumId);
        }
    }
}
