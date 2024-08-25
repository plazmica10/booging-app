using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for MyToursView.xaml
    /// </summary>
    public partial class MyToursView : Page
    {
        public MyToursView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new MyToursViewModel(currentUser, navigationService);
        }
    }
}
