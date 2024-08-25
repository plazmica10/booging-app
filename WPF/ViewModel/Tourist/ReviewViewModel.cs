using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ReviewViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourSearchService _tourSearchService = new();

        private readonly PictureService _pictureService = new();
        private readonly TourReviewService _tourReviewService = new();

        public string Title { get; set; }

        private readonly int _tourId;
        private int _ratingGuideKnowledge = 3;
        private int _ratingGuideLanguageSkills = 3;
        private int _ratingTour = 3;
        private string _comment = "";
        public string Comment { get => _comment; set { _comment = value; OnPropertyChanged(nameof(Comment)); } }


        // status msg
        private string _statusMessage = "";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

        public ObservableCollection<string> PictureLocations { get; set; } = new();

        public List<string> PictureIds { get; set; } = new();


        public ICommand AddPicturesCommand { get; }
        public ICommand AddReviewCommand { get; }
        public ICommand RadioCommand { get; }


        public ReviewViewModel(User currentUser, NavigationService navigationService, int tourId)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;
            _tourId = tourId;

            AddPicturesCommand = new FunctionCommand(AddPicturesAction);
            AddReviewCommand = new FunctionCommand(AddReviewAction);
            RadioCommand = new RelayCommand<string>(RadioAction);

            Tour? tour = _tourSearchService.GetById(_tourId);
            Title = "\ud83c\udfad Review: " + (tour == null ? "" : tour.Name);
        }

        private void RadioAction(string parameter)
        {
            string? convertedParam = Convert.ToString(parameter);
            if (string.IsNullOrEmpty(convertedParam)) { return; }
            switch (int.Parse(convertedParam))
            {
                case 1:  _ratingGuideKnowledge = 1; break;
                case 2:  _ratingGuideKnowledge = 2; break;
                case 3:  _ratingGuideKnowledge = 3; break;
                case 4:  _ratingGuideKnowledge = 4; break;
                case 5:  _ratingGuideKnowledge = 5; break;
                case 6:  _ratingGuideLanguageSkills = 1; break;
                case 7:  _ratingGuideLanguageSkills = 2; break;
                case 8:  _ratingGuideLanguageSkills = 3; break;
                case 9:  _ratingGuideLanguageSkills = 4; break;
                case 10: _ratingGuideLanguageSkills = 5; break;
                case 11: _ratingTour = 1; break;
                case 12: _ratingTour = 2; break;
                case 13: _ratingTour = 3; break;
                case 14: _ratingTour = 4; break;
                case 15: _ratingTour = 5; break;
            }
        }

        public void AddPicturesAction()
        {
            PictureIds.Clear();
            PictureLocations.Clear();
            List<string>? pictures = _pictureService.CreatePictures();
            if (pictures == null) { return; }

            PictureIds = pictures;

            foreach (string picture in _pictureService.GetPictureLocations(PictureIds))
            {
                PictureLocations.Add(picture);
            }
        }

        public void AddReviewAction()
        {
            TourReviewRequestDto tourReviewRequestDto = new(_tourId, CurrentUser.Id, _ratingGuideKnowledge, _ratingGuideLanguageSkills, _ratingTour, Comment, PictureIds);

            switch (_tourReviewService.CreateTourReview(tourReviewRequestDto))
            {
                case TourServiceReturn.Success:      StatusMessage = "Review posted!";   break;
                case TourServiceReturn.NotFoundTour: StatusMessage = "Can't find tour."; break;
            }
        }
    }
}
