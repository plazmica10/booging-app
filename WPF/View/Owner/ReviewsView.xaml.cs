using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for ReviewsView.xaml
    /// </summary>
    public partial class ReviewsView : Page
    {
        public ReviewsView(User user, NavigationService nav, string search = "")
        {
            InitializeComponent();
            DataContext = new ReviewsViewModel(user, nav, search);
        }
    }
}
