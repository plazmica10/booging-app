using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for RequestsStatisticsView.xaml
    /// </summary>
    public partial class RequestsStatisticsView : Page
    {
        public RequestsStatisticsView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new RequestsStatisticsViewModel(currentUser, navigationService);
        }
    }
}
