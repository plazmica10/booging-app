using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class MainViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        private bool _showDropdown;
        public static event Action? GiveUpResignEvent;
        public bool ShowDropdown { get => _showDropdown; set { _showDropdown = value; OnPropertyChanged(nameof(ShowDropdown)); } }
        private string _appTitle = "...";
        public string AppTitle { get => _appTitle; set { _appTitle = value; OnPropertyChanged(nameof(AppTitle)); } }

        private bool _isToursEnable = false;
        private bool _isCreateEnable = true;
        private bool _isRequestsEnable = true;
        private bool _isComplexRequestsEnable = true;
        private bool _isStatisticsEnable = true;
        private bool _isResignedChecked;

        public bool IsToursEnable { get => _isToursEnable; set { _isToursEnable = value; OnPropertyChanged(nameof(IsToursEnable)); } }
        public bool IsCreateEnable { get => _isCreateEnable; set { _isCreateEnable = value; OnPropertyChanged(nameof(IsCreateEnable)); } }
        public bool IsRequestsEnable { get => _isRequestsEnable; set { _isRequestsEnable = value; OnPropertyChanged(nameof(IsRequestsEnable)); } }
        public bool IsComplexRequestsEnable { get => _isComplexRequestsEnable; set { _isComplexRequestsEnable = value; OnPropertyChanged(nameof(IsComplexRequestsEnable)); } }
        public bool IsStatisticsEnable { get => _isStatisticsEnable; set { _isStatisticsEnable = value; OnPropertyChanged(nameof(IsStatisticsEnable)); } }
        public bool IsResignChecked { get => _isResignedChecked; set { _isResignedChecked = value; OnPropertyChanged(nameof(IsResignChecked)); } }
        private bool _isPageEnabled = true;
        public bool IsPageEnabled { get => _isPageEnabled; set { if (_isPageEnabled != value) { _isPageEnabled = value; OnPropertyChanged(nameof(IsPageEnabled)); } } }


        public ICommand ShowToursCommand { get; set; }
        public ICommand ShowCreateCommand { get; set; }
        public ICommand ShowRequestsCommand { get; set; }
        public ICommand ShowComplexRequestsCommand { get; set; }
        public ICommand ShowStatisticsCommand { get; set; }
        public ICommand ShowDemoCommand { get; set; }
        public ICommand ShowProfileCommand { get; set; }
        public ICommand ResignCommand { get; set; }
        public ICommand NavigatedCommand { get; private set; }

        public ICommand SignOutCommand { get; set; }

        public MainViewModel(User user, NavigationService navigationService)
        {
            CurrentUser = user;
            NavigationService = navigationService;

            ShowToursCommand = new FunctionCommand(ShowToursAction);
            ShowCreateCommand = new FunctionCommand(ShowCreateAction);
            ShowRequestsCommand = new FunctionCommand(ShowRequestsAction);
            ShowComplexRequestsCommand = new FunctionCommand(ShowComplexRequestsAction);
            ShowStatisticsCommand = new FunctionCommand(ShowStatisticsAction);
            ShowDemoCommand = new FunctionCommand(ShowDemoAction);
            ShowProfileCommand = new FunctionCommand(ShowProfileAction);
            ResignCommand = new FunctionCommand(ResignAction);
            SignOutCommand = new FunctionCommand(SignOutAction);

            NavigatedCommand = new FunctionCommand(NavigatedAction);
            GiveUpResignEvent += OnGiveUpResign;

            NavigationService.Navigate(new ToursView(CurrentUser, NavigationService));
        }

        private void SignOutAction()
        {
            ShowSignOutPopup("Do you want to sign out?", "Sign Out");
        }

        public void ShowSignOutPopup(string text, string title = "Sign Out?")
        {
            IsPageEnabled = false;

            var popupWindow = new SignOutWindow(title, text);
            popupWindow.Closed += (sender, args) => { IsPageEnabled = true; };
            popupWindow.Show();
        }
        public static void RaiseGiveUpResignEvent()
        {
            GiveUpResignEvent?.Invoke();
        }
        private void OnGiveUpResign()
        {
            IsToursEnable = false;
        }
        private void NavigatedAction()
        {
            Page? content = NavigationService.Content as Page;
            if (content == null) { return; }
            AppTitle = "...";
            content.Loaded += Content_Loaded;
        }

        private void Content_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Page? content = sender as Page;
            if (content == null) { return; }
            AppTitle = content.Title;
        }


        public void ShowToursAction()
        {
            ResetMenuChecks();
            IsToursEnable = false;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;

            NavigationService.Navigate(new ToursView(CurrentUser, NavigationService));
        }

        public void ShowCreateAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = false;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;

            NavigationService.Navigate(new CreateView(CurrentUser, NavigationService));
        }

        public void ShowRequestsAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = false;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;

            NavigationService.Navigate(new NormalRequestView(CurrentUser, NavigationService));
        }

        public void ShowComplexRequestsAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = false;
            IsStatisticsEnable = true;


        }

        public void ShowStatisticsAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = false;


            NavigationService.Navigate(new StatisticsToursView(CurrentUser, NavigationService));
        }

        public void ShowDemoAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;

        }

        public void ShowProfileAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;

        }
        private void ResetMenuChecks()
        {
            IsResignChecked = false;
        }
        public void ResignAction()
        {
            ResetMenuChecks();
            IsToursEnable = true;
            IsCreateEnable = true;
            IsRequestsEnable = true;
            IsComplexRequestsEnable = true;
            IsStatisticsEnable = true;
            IsResignChecked = true;
            NavigationService.Navigate(new ResignGuideView(CurrentUser, NavigationService));
        }


    }
}
