using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ReviewView.xaml
    /// </summary>
    public partial class ReviewView : Page
    {
        public ReviewView(User currentUser, NavigationService navigationService, int tourId)
        {
            InitializeComponent();
            this.DataContext = new ReviewViewModel(currentUser, navigationService, tourId);
        }
    }
}
