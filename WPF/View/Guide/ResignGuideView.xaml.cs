using BookingApp.Domain.Model;
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
using BookingApp.WPF.ViewModel.Guide;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for ResignGuideView.xaml
    /// </summary>
    public partial class ResignGuideView : Page
    {
        public ResignGuideView(User currentUser, NavigationService navigation)
        {
            InitializeComponent();
            this.DataContext = new ResignGuideViewModel(currentUser, navigation);
            Loaded += ResignGuideView_Loaded;
        }
        private void ResignGuideView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Set focus to the Yes button
            YesButton.Focus();
        }
    }
}
