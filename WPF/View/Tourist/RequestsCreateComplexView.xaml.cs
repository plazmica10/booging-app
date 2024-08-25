using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for RequestsCreateComplexView.xaml
    /// </summary>
    public partial class RequestsCreateComplexView : Page
    {
        public RequestsCreateComplexView(User currentUser, NavigationService navigationService, TourRequestComplexDto? data = null)
        {
            InitializeComponent();
            this.DataContext = new RequestsCreateComplexViewModel(currentUser, navigationService, data);
        }
    }
}
