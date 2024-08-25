using BookingApp.Domain.Model;
using System.Windows.Controls;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for MonthStatistics.xaml
    /// </summary>
    public partial class MonthStatisticsView : Page
    {
        public MonthStatisticsView(User user, NavigationService nav, int accommodationId, int year)
        {
            InitializeComponent();
            DataContext = new MonthStatisticsViewModel(user, nav, accommodationId, year);
        }
    }
}
