using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using MenuNavigation;
using WPF;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ForumViewModel : ValidationBase
    {
        private readonly UserService _userService = new UserService();
        private readonly ForumService _forumService = new ForumService();
        private readonly ForumCommentService  _forumCommentService = new ForumCommentService();
        public ForumViewModel(User user, NavigationService navService, Forum forum)
        {
            this.User = user;
            this.NavService = navService;
            this.Forum = forum;
            _forumService.IsUseful(forum);
            var forumCreator  =_userService.GetById(forum.UserId);
            this.ForumCreator = forumCreator.FirstName + " " + forumCreator.LastName;

        }
        public ICommand PostCommentCommand =>  new FunctionCommand(PostComment);
        public User User { get; set; }
        public NavigationService NavService { get; set; }
        private Forum _forum;
        public Forum Forum { get => _forum; set { _forum = value; OnPropertyChanged(nameof(Forum)); } }
        private string _forumCreator; 
        public string ForumCreator { get => _forumCreator; set { _forumCreator = value; OnPropertyChanged(nameof(ForumCreator)); } }
        private string _newComment;
        public string NewComment { get => _newComment; set { _newComment = value; OnPropertyChanged(nameof(NewComment)); } }
        public void PostComment()
        {
            Validate();
            if (!IsCommentValid()) return;
            ForumComment comment = new ForumComment()
            {
                Comment = NewComment, ForumId = Forum.Id, UserId = User.Id, UserName = User.FirstName + " " + User.LastName, UserPhoto = User.Photo
            };
            _forumService.Save(comment);
            NewComment = "";
            _forumCommentService.IsUseful(comment,Forum.City,Forum.Country);
            Forum = _forumService.GetById(Forum.Id);
        }

        public bool IsCommentValid()
        {
            if (string.IsNullOrEmpty(NewComment))
            {
                System.Windows.MessageBox.Show(TranslationSource.Instance["EmptyComment"]);
                return false;
            }
            return true;
        }
        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(NewComment)) { this.ValidationErrors["Comment"] = TranslationSource.Instance["EmptyComm"]; }
        }
    }
}
