using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ForumsViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<ForumDto> _forums = new();
        public ObservableCollection<ForumDto> Forums { get => _forums; set { _forums = value; OnPropertyChanged(nameof(Forums)); } }

        private ForumDto _selectedForum;
        public ForumDto SelectedForum { get => _selectedForum; set { _selectedForum = value; OnPropertyChanged(nameof(SelectedForum)); } }

        private string _newComment = "";
        public string NewComment { get => _newComment; set { _newComment = value; OnPropertyChanged(nameof(NewComment)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly ForumService _forumService = new();
        private readonly ForumCommentService _forumCommentService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public ICommand OpenForumCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenForum(o); });
        public ICommand SendCommentCommand => new FunctionCommand(SendComment);
        public ICommand ReportCommand => new FunctionArgCommand(obj => { if (obj is int o) ReportComment(o); });

        public ForumsViewModel(User user, NavigationService navService, int forumId = -1)
        {
            CurrentUser = user;
            NavService = navService;

            LoadForums();
            if (forumId != -1)
                OpenForum(forumId);
        }

        private void ReportComment(int commentId)
        {
            var comment = SelectedForum.Comments.First(c => c.Id == commentId);
            var comm = _forumCommentService.GetById(commentId);
            
            if (comment.ReportedBy[0] == -1)
            {
                comment.ReportedBy.Clear();
                comment.ReportedBy.Add(CurrentUser.Id);
                comment.ReportCount++;
            }
            else if (!comment.ReportedBy.Contains(CurrentUser.Id))
            {
                comment.ReportedBy.Add(CurrentUser.Id);
                comment.ReportCount++;
            }

            var temp = comment.UserPhoto;
            comment.UserPhoto = comm.UserPhoto;
            _forumCommentService.Update(comment);
            comment.UserPhoto = temp;
        }

        private void SendComment()
        {
            if (string.IsNullOrWhiteSpace(NewComment)) return;

            var comment = new ForumComment { UserId = CurrentUser.Id, UserName = $"{CurrentUser.FirstName} {CurrentUser.LastName}", UserPhoto = CurrentUser.Photo, ForumId = SelectedForum.Id, Comment = NewComment, IsOwner = true };

            _forumService.Save(comment);
            _forumService.IsUseful(_forumService.GetById(SelectedForum.Id));
            comment.UserPhoto = _pictureService.GetPictureLocation(comment.UserPhoto);
            SelectedForum.Comments.Add(comment);
            NewComment = "";
        }

        private void LoadForums()
        {
            var accommodations = _accommodationService.GetByOwnerId(CurrentUser.Id);
            var locations = accommodations.Select(a => (a.Location.City, a.Location.Country)).ToList();
            var forums = _forumService.GetByLocation(locations);
            Forums.Clear();
            foreach (var forum in forums)
            {
                _forumService.IsUseful(forum);
                var user = _userService.GetById(forum.UserId);
                Forums.Add(new ForumDto { Id = forum.Id, Title = forum.Title, Question = forum.Question, City = forum.City, Country = forum.Country, Loc = $"{forum.City}, {forum.Country}", Location = forum.Location, UserId = forum.UserId, UserName = $"{user.FirstName} {user.LastName}", UserPhoto = _pictureService.GetPictureLocation(user.Photo), Comments = new ObservableCollection<ForumComment>(forum.Comments), IsUseful = forum.IsUseful, IsClosed = forum.IsClosed });
            }

            if (Forums.Count > 0)
                OpenForum(Forums[0].Id);
        }

        public void OpenForum(int forumId)
        {
            foreach (var f in Forums)
            {
                f.Selected = f.Id == forumId;
            }

            SelectedForum = Forums.First(f => f.Selected);
        }
    }
}
