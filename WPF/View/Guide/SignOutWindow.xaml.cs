using System.Windows;
using BookingApp.WPF.ViewModel.Guide;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for SignOutWindow.xaml
    /// </summary>
    public partial class SignOutWindow : Window
    {
        public SignOutWindow(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new SignOutPopupViewModel(this,title,message);
        }
    }
}
