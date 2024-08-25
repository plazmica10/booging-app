using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for ToursTodayView.xaml
    /// </summary>
    public partial class ToursTodayView : Page
    {
        public ToursTodayView(User currentUser, NavigationService navigationService,NavigationService tourNavigationService)
        {
            InitializeComponent();
            this.DataContext = new ToursTodayViewModel(currentUser, navigationService, tourNavigationService, System.Windows.Application.Current.MainWindow);

        }

       
    }
}
