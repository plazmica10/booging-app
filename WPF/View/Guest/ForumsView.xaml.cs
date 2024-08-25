using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ForumsView.xaml
    /// </summary>
    public partial class ForumsView : Page
    {
        public ForumsView(User user, NavigationService navService)
        {
            InitializeComponent();
            DataContext = new ViewModel.Guest.ForumsViewModel(user, navService);
        }
    }
}
