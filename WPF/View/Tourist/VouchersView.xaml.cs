using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for VouchersView.xaml
    /// </summary>
    public partial class VouchersView : Page
    {
        public VouchersView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new VouchersViewModel(currentUser, navigationService);
        }
    }
}
