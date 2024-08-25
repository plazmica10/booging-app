using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ReserveView.xaml
    /// </summary>
    public partial class ReserveView : Page
    {
        public ReserveView(User currentUser, NavigationService navigationService, int tourId)
        {
            InitializeComponent();
            this.DataContext = new ReserveViewModel(currentUser, navigationService, tourId);
        }
    }
}
