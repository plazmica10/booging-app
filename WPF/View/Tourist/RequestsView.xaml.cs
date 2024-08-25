using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for RequestsPage.xaml
    /// </summary>
    public partial class RequestsView : Page
    {
        public RequestsView(User currentUser, NavigationService navigationService, int selectedTabIndex)
        {
            InitializeComponent();
            this.DataContext = new RequestsViewModel(currentUser, navigationService, selectedTabIndex);
        }
    }
}
