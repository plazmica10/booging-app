using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class OwnerReviewRepository : IOwnerReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/og_owner-reviews.csv";
        private readonly Serializer<OwnerReview> _serializer;
        private List<OwnerReview> _ownerReviews;


        public OwnerReviewRepository()
        {
            _serializer = new Serializer<OwnerReview>();
            _ownerReviews = _serializer.FromCsv(FilePath);
        }
        public OwnerReview Save(OwnerReview review)
        {
            review.Id = NextId();
            _ownerReviews = _serializer.FromCsv(FilePath);
            _ownerReviews.Add(review);
            _serializer.ToCsv(FilePath, _ownerReviews);
            return review;
        }

        public OwnerReview GetOwnerReview(int reservationId)
        {
            _ownerReviews = _serializer.FromCsv(FilePath);
            return _ownerReviews.FirstOrDefault(a => a.ReservationId == reservationId);
        }

        public int NextId()
        {
            _ownerReviews = _serializer.FromCsv(FilePath);
            if (_ownerReviews.Count < 1)
            {
                return 1;
            }
            return _ownerReviews.Max(a => a.Id) + 1;
        }

        public int CountByOwnerId(int ownerId)
        {
            _ownerReviews = _serializer.FromCsv(FilePath);
            return _ownerReviews.Count(r => r.OwnerId == ownerId);
        }

        public double AverageRatingByOwnerId(int ownerId)
        {
            _ownerReviews = _serializer.FromCsv(FilePath);
            List<OwnerReview> reviews = _ownerReviews.FindAll(r => r.OwnerId == ownerId);
            if (reviews.Count < 1)
            {
                return 0;
            }
            return reviews.Average(r => ((r.Cleanliness + r.OwnerRating) / 2));
        }

        public List<OwnerReview> GetAllForOwner(int ownerId)
        {
            _ownerReviews = _serializer.FromCsv(FilePath);
            return _ownerReviews.FindAll(r => r.OwnerId == ownerId);
        }


    }
}
