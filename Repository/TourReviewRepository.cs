using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourReviewRepository : ITourReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_reviews.csv";
        private readonly Serializer<TourReview> _serializer = new();

        public TourReviewRepository() { }

        public List<TourReview> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public TourReview? GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(t => t.Id == id);
        }

        public TourReview Save(TourReview tourReview)
        {
            List<TourReview> tourReviews = _serializer.FromCsv(FilePath);

            TourReview? current = tourReviews.Find(t => t.TourId == tourReview.TourId && t.TouristId == tourReview.TouristId);
            if (current != null)
            {
                tourReview.Id = current.Id;
                int index = tourReviews.IndexOf(current);
                tourReviews.Remove(current);
                tourReviews.Insert(index, tourReview);
            }
            else
            {
                tourReview.Id = tourReviews.Count < 1 ? 1 : tourReviews.Max(t => t.Id) + 1;
                tourReviews.Add(tourReview);
            }

            _serializer.ToCsv(FilePath, tourReviews);
            return tourReview;
        }

        public TourReview SaveAppend(TourReview tourReview)
        {
            List<TourReview> tourReviews = _serializer.FromCsv(FilePath);
            tourReview.Id = tourReviews.Count < 1 ? 1 : tourReviews.Max(t => t.Id) + 1;
            tourReviews.Add(tourReview);
            _serializer.ToCsv(FilePath, tourReviews);
            return tourReview;
        }

        public bool Update(TourReview tourReview)
        {
            List<TourReview> tourReviews = _serializer.FromCsv(FilePath);
            TourReview? current = tourReviews.Find(t => t.Id == tourReview.Id);
            if (current == null) { return false; }
            int index = tourReviews.IndexOf(current);
            tourReviews.Remove(current);
            tourReviews.Insert(index, tourReview);
            return _serializer.ToCsv(FilePath, tourReviews);
        }

        public List<TourReview> GetAllByTourId(int tourId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.TourId == tourId);
        }

        public TourReview? GetByTourIdTouristId(int tourId, int touristId)
        {
            return _serializer.FromCsv(FilePath).Find(t => t.TourId == tourId && t.TouristId == touristId);
        }
     
    }
}
