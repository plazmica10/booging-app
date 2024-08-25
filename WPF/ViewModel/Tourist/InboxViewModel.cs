using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class InboxViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourInfoService _tourInfoService = new();

        public ICommand ShowVouchersCommand { get; }

        public ObservableCollection<ItemMessageViewModel> Messages { get; set; } = new();

        public ItemMessageViewModel? MessagesSelected { get; set; }

        public InboxViewModel(User currentUser, NavigationService navigationService)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            ShowVouchersCommand = new FunctionCommand(ShowVouchersAction);

            Populate();
        }

        private void Populate()
        {
            foreach (TourMessage message in _tourInfoService.GetTouristMessagesDescending(CurrentUser.Id))
            {
                Messages.Add(new ItemMessageViewModel(message));
            }
        }

        public void ShowVouchersAction() => MainNavigationService.Navigate(new VouchersView(CurrentUser, MainNavigationService));
    }
}
