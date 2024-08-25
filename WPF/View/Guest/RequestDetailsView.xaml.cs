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
    public partial class RequestDetailsView : Page
    {
        public RequestDetailsView(User user,NavigationService navService,RequestDto req)
        {
            InitializeComponent();
            this.DataContext = new RequestDetailsViewModel(user,navService,req);
        }
    }
}
