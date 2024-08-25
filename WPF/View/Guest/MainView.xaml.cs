using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(User currentUser)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(currentUser,this.MainFrame.NavigationService);
        }
    }
}
