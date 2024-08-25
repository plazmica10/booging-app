using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class MainContentViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private string _appTitle = "...";
        public string AppTitle { get => _appTitle; set { _appTitle = value; OnPropertyChanged(nameof(AppTitle)); } }

        private bool _mainNavigationServiceCanGoBack;
        public bool MainNavigationServiceCanGoBack { get => _mainNavigationServiceCanGoBack; set { _mainNavigationServiceCanGoBack = value; OnPropertyChanged(nameof(MainNavigationServiceCanGoBack)); } }

        public ICommand BackCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ShowToursCommand { get; }
        public ICommand ShowMyToursCommand { get; }
        public ICommand ShowInboxCommand { get; }
        public ICommand ShowRequestsCommand { get; }
        public ICommand ShowHelpCommand { get; }

        public MainContentViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            BackCommand = new FunctionCommand(BackAction);
            SearchCommand = new FunctionCommand(SearchAction);
            ShowToursCommand = new FunctionCommand(ShowToursAction);
            ShowMyToursCommand = new FunctionCommand(ShowMyToursAction);
            ShowInboxCommand = new FunctionCommand(ShowInboxAction);
            ShowRequestsCommand = new FunctionCommand(ShowRequestsAction);
            ShowHelpCommand = new FunctionCommand(ShowHelpAction);

            _mainNavigationServiceCanGoBack = MainNavigationService.CanGoBack;
            MainNavigationService.LoadCompleted += NavigationService_LoadCompleted;

            SearchAction();
        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Page? content = e.Content as Page;
            if (content == null) { return; }

            RoutedEventHandler handler = null;
            handler = (o, args) =>
            {
                Page? page = o as Page;
                if (page == null) { return; }
                AppTitle = page.Title;
                MainNavigationServiceCanGoBack = MainNavigationService.CanGoBack;
                page.Loaded -= handler;
            };

            content.Loaded += handler;
        }

        public void BackAction() { if (MainNavigationService.CanGoBack) { MainNavigationService.GoBack(); } }
        public void SearchAction() { MainNavigationService.Navigate(new ToursView(CurrentUser, MainNavigationService)); }
        public void ShowToursAction() { MainNavigationService.Navigate(new ToursView(CurrentUser, MainNavigationService)); }
        public void ShowMyToursAction() { MainNavigationService.Navigate(new MyToursView(CurrentUser, MainNavigationService)); }
        public void ShowInboxAction() { MainNavigationService.Navigate(new InboxView(CurrentUser, MainNavigationService)); }
        public void ShowRequestsAction() { MainNavigationService.Navigate(new RequestsView(CurrentUser, MainNavigationService, 0)); }
        public void ShowHelpAction() { MainNavigationService.Navigate(new HelpView(CurrentUser)); }
    }
}
