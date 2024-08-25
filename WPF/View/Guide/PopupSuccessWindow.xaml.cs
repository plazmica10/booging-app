using BookingApp.WPF.ViewModel.Guide;
using System.Windows;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for PopupSuccessWindow.xaml
    /// </summary>
    public partial class PopupSuccessWindow : Window
    {
        public PopupSuccessWindow(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new PopupSuccessViewModel(this, title, message);
        }

    }
}
