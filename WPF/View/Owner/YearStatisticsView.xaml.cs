using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for YearStatisticsView.xaml
    /// </summary>
    public partial class YearStatisticsView : Page
    {
        public YearStatisticsView(User user, NavigationService nav, int accommodationId)
        {
            InitializeComponent();
            DataContext = new YearStatisticsViewModel(user, nav, accommodationId);
        }
    }
}
