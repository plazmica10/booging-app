using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReviewView.xaml
    /// </summary>
    public partial class ReviewView : Window
    {
        public ReviewView(User user, NavigationService navService, AccommodationReservation reservation)
        {
            InitializeComponent();
            var viewModel = new ReviewViewModel(user, navService, reservation);
            DataContext = viewModel;
            viewModel.ReviewSubmitted += () =>
            {
                Close();
            };
        }
    }
}
