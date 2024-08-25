using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourReviewRepository
    {
        public List<TourReview> GetAll();
        public TourReview? GetById(int id);
        public TourReview Save(TourReview tourReview);
        public TourReview SaveAppend(TourReview tourReview);
        public bool Update(TourReview tourReview);
        public List<TourReview> GetAllByTourId(int tourId);
        public TourReview? GetByTourIdTouristId(int tourId, int touristId);
    }
}
