using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for YearStatisticsGraphView.xaml
    /// </summary>
    public partial class YearStatisticsGraphView : Page
    {
        public YearStatisticsGraphView(User user, NavigationService nav, int accommodationId)
        {
            InitializeComponent();
            DataContext = new YearStatisticsGraphViewModel(user, nav, accommodationId);
        }
    }
}
