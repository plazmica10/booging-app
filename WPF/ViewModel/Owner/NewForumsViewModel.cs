using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class NewForumsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<NewForumDto> _forums = new();
        public ObservableCollection<NewForumDto> Forums { get { return _forums; } set { _forums = value; OnPropertyChanged("Forums"); } }

        private NewForumDto _selectedForum = new();
        public NewForumDto SelectedForum { get { return _selectedForum; } set { _selectedForum = value; OnPropertyChanged("SelectedForum"); } }

        private bool _showRight = true;
        public bool ShowRight { get { return _showRight; } set { _showRight = value; OnPropertyChanged("ShowRight"); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly ForumService _forumService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public ICommand OpenForumCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenForum(o); });

        public ICommand VisitForumCommand => new FunctionCommand(VisitForum);

        public NewForumsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;
            
            LoadForums();
        }

        private void VisitForum()
        {
            var forum = _forumService.GetById(SelectedForum.ForumId);
            forum.IsNotified = true;
            _forumService.Update(forum);
            NavService.Navigate(new ForumsView(CurrentUser, NavService, SelectedForum.ForumId));
        }

        private void LoadForums()
        {
            Forums.Clear();
            var accommodations = _accommodationService.GetByOwnerId(CurrentUser.Id);
            var locations = accommodations.Select(a => (a.Location.City, a.Location.Country)).ToList();
            var forums = _forumService.GetByLocation(locations);
            forums.RemoveAll(f => f.IsNotified);
            foreach (var forum in forums)
            {
                var user = _userService.GetById(forum.UserId);
                Forums.Add(new NewForumDto
                {
                    ForumId = forum.Id,
                    GuestName = $"{user.FirstName} {user.LastName}",
                    GuestPhoto = _pictureService.GetPictureLocation(user.Photo)
                });
            }

            if (Forums.Count > 0)
                OpenForum(Forums[0].ForumId);
            if (Forums.Count == 0)
                ShowRight = false;
        }

        private void OpenForum(int forumId)
        {
            foreach (var forum in Forums)
            {
                forum.Selected = forum.ForumId == forumId;
            }

            SelectedForum = Forums.First(f => f.Selected);
        }
    }
}
