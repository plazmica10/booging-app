using System.Text.RegularExpressions;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guide
{
    public partial class NormalRequestView : Page
    {
        public NormalRequestView(User currentUser, NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new NormalRequestViewModel(currentUser, navigationService);
        }
        private void NumericOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Regex to match non-numeric text
            return !regex.IsMatch(text);
        }
    }
}
