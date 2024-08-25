using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for InboxPage.xaml
    /// </summary>
    public partial class InboxView : Page
    {
        public InboxView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new InboxViewModel(currentUser, navigationService);
        }
    }
}
