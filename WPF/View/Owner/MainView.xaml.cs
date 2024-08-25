using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(User user)
        {
            InitializeComponent();
            DataContext = new MainViewModel(user, MainFrame.NavigationService);
        }
    }
}
