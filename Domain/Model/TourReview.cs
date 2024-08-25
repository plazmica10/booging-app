using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Domain.Model
{
    public class TourReview : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TourId { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public int RatingGuideKnowledge { get; set; } = -1;
        public int RatingGuideLanguageSkills { get; set; } = -1;
        public int RatingTour { get; set; } = -1;
        public string Comment { get; set; } = "";
        public List<string> PictureIds { get; set; } = new();
        public bool IsValid { get; set; } = true;

        public string GetHeader() => "Id|TourId|TouristId|RatingGuideKnowledge|RatingGuideLanguageSkills|RatingTour|Comment|PictureIds|IsValid";

        public TourReview() { }

        public TourReview(int tourId, int touristId, int ratingGuideKnowledge, int ratingGuideLanguageSkills, int ratingTour, string comment, List<string> pictureIds)
        {
            TourId = tourId;
            TouristId = touristId;
            RatingGuideKnowledge = ratingGuideKnowledge;
            RatingGuideLanguageSkills = ratingGuideLanguageSkills;
            RatingTour = ratingTour;
            Comment = comment;
            PictureIds = pictureIds;
        }

        public string[] ToCsv() => new[] { $"{Id}", $"{TourId}", $"{TouristId}", $"{RatingGuideKnowledge}", $"{RatingGuideLanguageSkills}", $"{RatingTour}", $"{Comment}", $"{string.Join(",", PictureIds)}", $"{IsValid}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TourId = Convert.ToInt32(values[i++]);
            TouristId = Convert.ToInt32(values[i++]);
            RatingGuideKnowledge = Convert.ToInt32(values[i++]);
            RatingGuideLanguageSkills = Convert.ToInt32(values[i++]);
            RatingTour = Convert.ToInt32(values[i++]);
            Comment = values[i++];
            PictureIds = values[i++].Split(",").ToList();
            IsValid = bool.Parse(values[i++]);
        }

        public override string ToString() => $"{Id}|{TourId}|{TouristId}|{RatingGuideKnowledge}|{RatingGuideLanguageSkills}|{RatingTour}|{Comment}|{string.Join(",", PictureIds)}|{IsValid}";
    }
}