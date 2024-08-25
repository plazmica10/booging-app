using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReservationSummaryView.xaml
    /// </summary>
    public partial class ReservationSummaryView : Page
    {
        public ReservationSummaryView(User user,ReservationsDto reservation, NavigationService navService)
        {
            InitializeComponent();
            DataContext = new ReservationSummaryViewModel(user,reservation, navService);
        }
    }
}
