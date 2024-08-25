using BookingApp.Domain.Model;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Command;
using BookingApp.WPF.View.Tourist.HelpPages;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class HelpViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService HelpNavigationService;

        public ICommand NavigateCommand { get; }

        public HelpViewModel(User currentUser, NavigationService helpNavigationService)
        {
            CurrentUser = currentUser;
            HelpNavigationService = helpNavigationService;

            NavigateCommand = new RelayCommand<string>(Navigate);

            HelpNavigationService.Navigate(new HelpGeneral());
        }

        private void Navigate(string target)
        {
            switch (target)
            {
                case "General":         HelpNavigationService.Navigate(new HelpGeneral());         break;
                case "Tours":           HelpNavigationService.Navigate(new HelpTours());           break;
                case "MyTours":         HelpNavigationService.Navigate(new HelpMyTours());         break;
                case "Inbox":           HelpNavigationService.Navigate(new HelpInbox());           break;
                case "Vouchers":        HelpNavigationService.Navigate(new HelpVouchers());        break;
                case "NormalRequests":  HelpNavigationService.Navigate(new HelpRequestsNormal());  break;
                case "ComplexRequests": HelpNavigationService.Navigate(new HelpRequestsComplex()); break;
            }
        }
    }
}
