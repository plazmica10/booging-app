using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class MainViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService WindowNavigationService;

        private string _appTitle = "...";
        public string AppTitle { get => _appTitle; set { _appTitle = value; OnPropertyChanged(nameof(AppTitle)); } }

        public MainViewModel(User currentUser, NavigationService windowNavigationService)
        {
            CurrentUser = currentUser;
            WindowNavigationService = windowNavigationService;

            // WindowNavigationService.Navigate(new MainContent(CurrentUser));

            windowNavigationService.LoadCompleted += WindowNavigationService_LoadCompleted;

            WindowNavigationService.Navigate(CurrentUser.HasFirstLoginHappened ?
                    new MainContent(currentUser) : new StartupWizard(CurrentUser, WindowNavigationService)
            );
        }

        private void WindowNavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Page? content = e.Content as Page;
            if (content == null) { return; }

            if (string.IsNullOrEmpty(content.Title))
            {
                if (content.FindName("MainFrame") is Frame mainFrame)
                {
                    mainFrame.NavigationService.LoadCompleted += MainFrameNavigationService_LoadCompleted;
                }
            }
            else
            {
                AppTitle = content.Title;
            }
        }

        private void MainFrameNavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Page? content = e.Content as Page;
            if (content == null) { return; }
            AppTitle = content.Title;
        }
    }
}
