using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guest;
using MenuNavigation;
using WPF;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AddForumViewModel : ValidationBase
    {
        private readonly ForumService _forumService = new ForumService();
        public AddForumViewModel(User user,NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
        }

        #region bindings
        public ICommand PostCommand => new FunctionCommand(PostForum);
        public User CurrentUser { get; set; }
        public NavigationService NavigationService { get; set; }
        public event Action ForumAdded;
        private string _question;
        public string Question { get => _question; set { _question = value;OnPropertyChanged(nameof(Question)); } }
        private string _city;
        public string City { get => _city; set { _city = value; OnPropertyChanged(nameof(City)); } }
        private string _country;
        public string Country { get => _country; set { _country = value; OnPropertyChanged(nameof(Country)); } }
        private string _title;
        public string Title { get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); }}
        private Location _selectedLocation = new();
        public Location SelectedLocation { get => _selectedLocation; set { _selectedLocation = value; OnPropertyChanged(nameof(SelectedLocation)); } }
        #endregion

        public void PostForum()
        {
            City = SelectedLocation.City;
            Country = SelectedLocation.Country;
            if(!IsPostValid()) return;
            Forum forum = new Forum()
            {
                Title = Title, Question = Question, City = SelectedLocation.City, Country = SelectedLocation.Country, UserId = CurrentUser.Id, Comments = new List<ForumComment>(), IsUseful = false, IsClosed = false
            };
            _forumService.Save(forum);
            ForumAdded?.Invoke();
            var result = MessageBox.Show($"{TranslationSource.Instance["ForumAdded"]}", TranslationSource.Instance["ForumSuccess"], MessageBoxButton.YesNo); if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new ForumView(CurrentUser, NavigationService,forum));
            }
            else { NavigationService.Navigate(new ForumsView(CurrentUser, NavigationService)); }
        }

        public bool IsPostValid()
        {
            Validate();
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Question) || string.IsNullOrEmpty(City) ||
                string.IsNullOrEmpty(Country))
            {
                System.Windows.MessageBox.Show(TranslationSource.Instance["RequiredFields"]);
                return false;
            }
            return true;
        }
        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(City)) { this.ValidationErrors["Location"] = TranslationSource.Instance["EmptyLoc"]; }
            if (string.IsNullOrWhiteSpace(Title)) { this.ValidationErrors["Title"] = TranslationSource.Instance["TitleRequired"]; }
            if (string.IsNullOrWhiteSpace(Question)) { this.ValidationErrors["Question"] = TranslationSource.Instance["EmptyQuestion"]; }
        }
    }
}
