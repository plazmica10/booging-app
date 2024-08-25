using System.Diagnostics;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class StartupWizardViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService WindowNavigationService;
        public NavigationService MainNavigationService;

        public ICommand ContinueCommand { get; }

        public StartupWizardViewModel(User currentUser, NavigationService windowNavigationService, NavigationService mainNavigationService)
        {
            CurrentUser = currentUser;
            WindowNavigationService = windowNavigationService;
            MainNavigationService = mainNavigationService;

            ContinueCommand = new FunctionCommand(ContinueAction);

            windowNavigationService.LoadCompleted += WindowNavigationServiceOnLoadCompleted;
            mainNavigationService.Navigate(new HelpView(currentUser));
        }

        public void ContinueAction()
        {
            WindowNavigationService.Navigate(new MainContent(CurrentUser));
        }

        private void WindowNavigationServiceOnLoadCompleted(object sender, NavigationEventArgs e)
        {
            Page? content = e.Content as Page;
            if (content == null) { return; }

            if (content.FindName("MainFrame") is Frame mainFrame)
            {
                while (mainFrame.NavigationService.RemoveBackEntry() != null) { } // Remove back entries until none are left
            }
        }
    }
}
