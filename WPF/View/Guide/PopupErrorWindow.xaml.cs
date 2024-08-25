using BookingApp.WPF.ViewModel.Guide;
using System.Windows;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for PopupErrorWindow.xaml
    /// </summary>
    public partial class PopupErrorWindow : Window
    {
        public PopupErrorWindow(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new PopupErrorViewModel(this, title, message);
        }

    }
}
