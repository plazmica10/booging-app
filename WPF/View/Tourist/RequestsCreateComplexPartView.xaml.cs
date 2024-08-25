using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for RequestsCreateComplexPartView.xaml
    /// </summary>
    public partial class RequestsCreateComplexPartView : Page
    {
        public RequestsCreateComplexPartView(User currentUser, NavigationService navigationService, TourRequestComplexDto data)
        {
            InitializeComponent();
            this.DataContext = new RequestsCreateComplexPartViewModel(currentUser, navigationService, data);
        }
    }
}
