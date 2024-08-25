using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{

    public partial class ToursView : Page
    {
        public ToursView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new ToursViewModel(currentUser, navigationService, ToursFrame.NavigationService);

        }

       
    }
}
