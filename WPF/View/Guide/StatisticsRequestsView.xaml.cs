using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{
    public partial class StatisticsRequestsView : Page
    {
        public StatisticsRequestsView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new StatisticsRequestsViewModel(currentUser, navigationService);
        }
    }
}

