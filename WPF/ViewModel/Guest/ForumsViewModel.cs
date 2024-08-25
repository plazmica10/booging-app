using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guest;
using MenuNavigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ForumsViewModel  : ViewModelBase
    {
        private ForumService _forumService = new ForumService();

        public ForumsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
            MyForums = new ObservableCollection<Forum>(_forumService.GetAllByGuestId(user.Id));
            OtherForums = new ObservableCollection<Forum>(_forumService.GetAllOther(user.Id));
        }

        #region bindings

        public User CurrentUser;
        public NavigationService NavigationService;
        public ICommand AddForumCommand => new FunctionCommand(AddForum);
        public ICommand CloseForumCommand => new FunctionCommand(CloseMyForum);
        public ICommand ViewCommand => new FunctionCommand(ViewMyForum);
        public ICommand OthersViewCommand => new FunctionCommand(ViewForum);
        private ObservableCollection<Forum> _myForums;
        public ObservableCollection<Forum> MyForums { get => _myForums; set { _myForums = value; OnPropertyChanged(nameof(MyForums)); } }
        private ObservableCollection<Forum> _otherForums;
        public ObservableCollection<Forum> OtherForums { get => _otherForums; set { _otherForums = value; OnPropertyChanged(nameof(OtherForums)); } }
        private Forum selectedMyForum;
        public Forum SelectedMyForum { get => selectedMyForum; set { selectedMyForum = value; OnPropertyChanged(nameof(SelectedMyForum)); } }
        private Forum selectedOtherForum;
        public Forum SelectedOtherForum { get => selectedOtherForum; set { selectedOtherForum = value; OnPropertyChanged(nameof(SelectedOtherForum)); } }
        
        #endregion
        public void AddForum()
        {
            AddForumView  addForumWindow = new AddForumView(CurrentUser, NavigationService);
            addForumWindow.ShowDialog();
        }
        public void ViewMyForum() { if (selectedMyForum == null) return; NavigationService.Navigate(new ForumView(CurrentUser, NavigationService, SelectedMyForum)); }
        public void ViewForum() { if (selectedOtherForum == null) return; NavigationService.Navigate(new ForumView(CurrentUser, NavigationService, SelectedOtherForum)); }

        public void CloseMyForum()
        {
            if (selectedMyForum == null) return;
            if (selectedMyForum.IsClosed == true)
            {
                MessageBox.Show(TranslationSource.Instance["ForumClosedAlr"]);
                return;
            }
            var result = MessageBox.Show(TranslationSource.Instance["ForumQ"], TranslationSource.Instance["CloseForum"], MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                selectedMyForum.IsClosed = true;
                _forumService.Update(selectedMyForum);
                MessageBox.Show(TranslationSource.Instance["ForumClosed"]);
            }

        }
    }
}
