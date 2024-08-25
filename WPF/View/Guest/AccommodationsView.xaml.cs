using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    public partial class AccommodationsView : Page
    {
        public AccommodationsView(User currentUser,NavigationService navService)
        {
            InitializeComponent();
            this.DataContext = new AccommodationsViewModel(currentUser,navService);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyPopup.IsOpen = !MyPopup.IsOpen;
        }
    }
}

