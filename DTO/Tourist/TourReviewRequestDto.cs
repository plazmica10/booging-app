using System.Collections.Generic;

namespace BookingApp.DTO.Tourist
{
    public class TourReviewRequestDto
    {
        public int TourId { get; set; }
        public int TouristId { get; set; }
        public int RatingGuideKnowledge { get; set; }
        public int RatingGuideLanguageSkills { get; set; }
        public int RatingTour { get; set; }
        public string Comment { get; set; }
        public List<string> PictureIds { get; set; }

        public TourReviewRequestDto(int tourId, int touristId, int ratingGuideKnowledge, int ratingGuideLanguageSkills, int ratingTour, string comment, List<string> pictureIds)
        {
            TourId = tourId;
            TouristId = touristId;
            RatingGuideKnowledge = ratingGuideKnowledge;
            RatingGuideLanguageSkills = ratingGuideLanguageSkills;
            RatingTour = ratingTour;
            Comment = comment;
            PictureIds = pictureIds;
        }
    }
}