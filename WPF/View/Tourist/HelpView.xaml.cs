using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : Page
    {
        public HelpView(User currentUser)
        {
            InitializeComponent();
            this.DataContext = new HelpViewModel(currentUser, HelpFrame.NavigationService);
        }
    }
}
