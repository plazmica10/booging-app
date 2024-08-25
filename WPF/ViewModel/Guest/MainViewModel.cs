using System.Windows;
using System;
using System.Linq;
using System.Windows.Navigation;
using System.Windows.Input;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guest;
using System.Windows.Controls;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BookingApp.WPF.ViewModel.Guest
{

    public class MainViewModel : ViewModelBase
    {
        private readonly AccommodationReservationService _reservationService = new();
        private readonly UserService _userService = new();
        public NavigationService NavigationService;
        public User CurrentUser;

        public MainViewModel(User user,NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
            this.CurrentLanguage = "en-US";
            IsDarkMode = false;
            _userService.RefreshSuperGuest();
            _reservationService.UpdateReservations();
            navService.Navigate(new AccommodationsView(CurrentUser, NavigationService));
            ShowNotifications();

        }
        #region commands and actions
        public ICommand HomeCommand => new FunctionCommand(HomeAction);
        public ICommand ReservationsCommand => new FunctionCommand(ReservationsAction);
        public ICommand ProfileCommand => new FunctionCommand(ProfileAction);
        public ICommand ForumsCommand => new FunctionCommand(ForumsAction);
        public ICommand SwitchLanguage => new FunctionCommand(Execute_SwitchLanguageCommand);
        public ICommand SwichModeCommand => new FunctionCommand(SwitchMode);
        public void HomeAction() { NavigationService.Navigate(new AccommodationsView(CurrentUser, NavigationService)); }
        public void ReservationsAction() { NavigationService.Navigate(new ReservationsView(CurrentUser, NavigationService)); }
        public void ProfileAction() { NavigationService.Navigate(new ProfileView(CurrentUser, NavigationService)); }
        public void ForumsAction() { NavigationService.Navigate(new ForumsView(CurrentUser, NavigationService)); }

        private bool _isDarkMode;
        public bool IsDarkMode { get { return _isDarkMode; } set {_isDarkMode = value; OnPropertyChanged(nameof(IsDarkMode)); } }
        #endregion
        public void ShowNotifications()
        {
            var requests = _reservationService.GetRequestsByUserId(CurrentUser.Id);
            var responded = requests.Where(r => r.Status != RequestStatus.Pending).ToList();
            if (responded.Count > 0)
            {
                MessageBox.Show("One or more rescheduling requests have been reviewed");
            }
        }

        private string currentLanguage;
        public string CurrentLanguage { get { return currentLanguage; } set { currentLanguage = value; } }
        private void Execute_SwitchLanguageCommand()
        {
            var app = (App)System.Windows.Application.Current;
            if (CurrentLanguage.Equals("en-US"))
            {
                CurrentLanguage = "sr-LATN";
            }
            else
            {
                CurrentLanguage = "en-US";
            }
            app.ChangeLanguage(CurrentLanguage);
        }
        public void SwitchMode()
        {
            var dict = System.Windows.Application.Current.Resources.MergedDictionaries.FirstOrDefault();
            if (dict != null)
            {
                var themeDict = dict.MergedDictionaries.FirstOrDefault();
                if (themeDict != null)
                {
                    if (themeDict.Source.OriginalString == "LightTheme.xaml")
                    {
                        themeDict.Source = new Uri("DarkTheme.xaml", UriKind.Relative);
                        IsDarkMode = true;
                    }
                    else
                    {
                        themeDict.Source = new Uri("LightTheme.xaml", UriKind.Relative);
                        IsDarkMode = false;
                    }
                }
            }
        }
    }
}
