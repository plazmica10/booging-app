using BookingApp.Domain.Model;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for MonthStatisticsGraphView.xaml
    /// </summary>
    public partial class MonthStatisticsGraphView : Page
    {
        public MonthStatisticsGraphView(User user, NavigationService nav, int accommodationId, int year)
        {
            InitializeComponent();
            DataContext = new ViewModel.Owner.MonthStatisticsGraphViewModel(user, nav, accommodationId, year);
        }
    }
}
