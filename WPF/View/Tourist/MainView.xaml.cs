using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(User currentUser)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(currentUser, WindowFrame.NavigationService);
        }
    }
}
