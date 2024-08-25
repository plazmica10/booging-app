using System.Windows;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ToursViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService NavigationService;
        public NavigationService ToursNavigationService;
        private TourSearchService tourSearchService = new();

        public ICommand ShowTodayCommand { get; }
        public ICommand ShowAllCommand { get; }
        public ICommand ShowFinishedCommand { get; }
        public ICommand ShowStartedTourCommand { get; }
        private bool _isPageEnabled = true;
        public bool IsPageEnabled{ get => _isPageEnabled; set{ if (_isPageEnabled != value){ _isPageEnabled = value; OnPropertyChanged(nameof(IsPageEnabled)); } } }

        private bool _isShowTodayEnabled = false;
        private bool _isShowAllEnabled = true;
        private bool _isShowFinishedEnabled = true;
        private bool _isShowStartedTourEnabled = true;

        public bool IsShowTodayEnabled{ get => _isShowTodayEnabled; set { _isShowTodayEnabled = value; OnPropertyChanged(nameof(IsShowTodayEnabled)); }}

        public bool IsShowAllEnabled {get => _isShowAllEnabled;set { _isShowAllEnabled = value; OnPropertyChanged(nameof(IsShowAllEnabled)); } }

        public bool IsShowFinishedEnabled { get => _isShowFinishedEnabled;set { _isShowFinishedEnabled = value; OnPropertyChanged(nameof(IsShowFinishedEnabled)); }}

        public bool IsShowStartedTourEnabled{ get => _isShowStartedTourEnabled; set { _isShowStartedTourEnabled = value; OnPropertyChanged(nameof(IsShowStartedTourEnabled)); } }


        public ToursViewModel(User currentUser,NavigationService navigation, NavigationService toursFrame)
        {
            CurrentUser = currentUser;
            NavigationService = navigation;
            ToursNavigationService = toursFrame;

            ShowTodayCommand = new FunctionCommand(ShowTodayAction);
            ShowAllCommand = new FunctionCommand(ShowAllAction);
            ShowFinishedCommand = new FunctionCommand(ShowFinishedAction);
            ShowStartedTourCommand = new FunctionCommand(ShowStartedTourAction);

            ToursNavigationService.Navigate(new ToursTodayView(CurrentUser, NavigationService, ToursNavigationService));
        }
        public void ShowErrorPopup(string text, string title = "invalid input")
        {
            IsPageEnabled = false;

            var popupWindow = new PopupErrorWindow(title, text);
            popupWindow.Closed += (sender, args) => { IsPageEnabled = true; };
           
            popupWindow.Show();
        }

        public void ShowTodayAction() {
            IsShowTodayEnabled = false;
            IsShowAllEnabled = true;
            IsShowFinishedEnabled = true;
            IsShowStartedTourEnabled = true; ToursNavigationService.Navigate(new ToursTodayView(CurrentUser, NavigationService, ToursNavigationService)); }
        public void ShowAllAction()
        {
            IsShowTodayEnabled = true;
            IsShowAllEnabled = false;
            IsShowFinishedEnabled = true;
            IsShowStartedTourEnabled = true; ToursNavigationService.Navigate(new ToursAllView(CurrentUser, NavigationService, ToursNavigationService)); }
        public void ShowFinishedAction()
        {
            IsShowTodayEnabled = true;
            IsShowAllEnabled = true;
            IsShowFinishedEnabled = false;
            IsShowStartedTourEnabled = true; ToursNavigationService.Navigate(new ToursFinishedView(CurrentUser, NavigationService, ToursNavigationService)); }
        public void ShowStartedTourAction()
        {
            IsShowTodayEnabled = true;
            IsShowAllEnabled = true;
            IsShowFinishedEnabled = true;
            IsShowStartedTourEnabled = false;
            Tour? tour = tourSearchService.GetStartedByGuideId(CurrentUser.Id);
            if (tour == null) { MessageBox.Show("no my tour started"); return; }
            ToursNavigationService.Navigate(new StartedTourView(CurrentUser, NavigationService, tour.Id));
        }

    }
}
