using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemReviewViewModel : ViewModelBase
    {
        private readonly TourReview _tourReview;

        public int Id => _tourReview.Id;
        public int TourId => _tourReview.TourId;
        public int TouristId => _tourReview.TouristId;
       
        public int RatingGuideKnowledge => _tourReview.RatingGuideKnowledge;
        public int RatingGuideLanguageSkills => _tourReview.RatingGuideLanguageSkills;
        public int RatingTour => _tourReview.RatingTour;
        public string Comment => _tourReview.Comment;
        public string PictureIds => string.Join(",", _tourReview.PictureIds);
        public string IsValid => _tourReview.IsValid.ToString();

        public ItemReviewViewModel(TourReview tourReview)
        {
            _tourReview = tourReview;
        }
    }
}
