using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for RequestsCreateNormalView.xaml
    /// </summary>
    public partial class RequestsCreateNormalView : Page
    {
        public RequestsCreateNormalView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new RequestsCreateNormalViewModel(currentUser, navigationService);
        }
    }
}
