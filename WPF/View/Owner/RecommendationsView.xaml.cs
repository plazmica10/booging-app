using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for RecommendationsView.xaml
    /// </summary>
    public partial class RecommendationsView : Page
    {
        public RecommendationsView(User user, NavigationService nav)
        {
            InitializeComponent();
            DataContext = new RecommendationViewModel(user, nav);
        }
    }
}
