using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class TourReviewService
    {
        private readonly ITourRepository       _iTourRepository       = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReviewRepository _iTourReviewRepository = Injector.Injector.CreateInstance<ITourReviewRepository>();

        public TourServiceReturn CreateTourReview(TourReviewRequestDto tourReviewRequestDto)
        {
            Tour? tour = _iTourRepository.GetById(tourReviewRequestDto.TourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }

            _iTourReviewRepository.Save(new TourReview(
                tourReviewRequestDto.TourId,
                tourReviewRequestDto.TouristId,
                tourReviewRequestDto.RatingGuideKnowledge,
                tourReviewRequestDto.RatingGuideLanguageSkills,
                tourReviewRequestDto.RatingTour,
                tourReviewRequestDto.Comment,
                tourReviewRequestDto.PictureIds
            )); // save & overwrite if exists

            return TourServiceReturn.Success;
        }

        public List<TourReview> GetTourReviews(int tourId)
        {
            return _iTourReviewRepository.GetAllByTourId(tourId);
        }

        public TourServiceReturn MarkInvalidReview(int tourReviewId)
        {
            TourReview? tourReview = _iTourReviewRepository.GetById(tourReviewId);
            if (tourReview == null) { return TourServiceReturn.TourReviewNotFound; }

            tourReview.IsValid = false;
            _iTourReviewRepository.Update(tourReview);

            return TourServiceReturn.Success;
        }
    }
}
