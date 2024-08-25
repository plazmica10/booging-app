using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    internal class GuestReviewRepository : IGuestReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/og_reviews.csv";
        private readonly Serializer<GuestReview> _serializer;
        private List<GuestReview> _guestReviews;

        // Constructor
        public GuestReviewRepository()
        {
            _serializer = new Serializer<GuestReview>();
            _guestReviews = _serializer.FromCsv(FilePath);
        }

        // Get all guest reviews
        public List<GuestReview> GetAll()
        {
            _guestReviews = _serializer.FromCsv(FilePath);
            return _guestReviews;
        }

        // Save a guest review
        public GuestReview Save(GuestReview guestReview)
        {
            guestReview.Id = NextId();
            _guestReviews = _serializer.FromCsv(FilePath);
            _guestReviews.Add(guestReview);
            _serializer.ToCsv(FilePath, _guestReviews);
            return guestReview;
        }

        public GuestReview GetById(int id)
        {
            _guestReviews = _serializer.FromCsv(FilePath);
            return _guestReviews.Find(r => r.Id == id);
        }

        public List<GuestReview> GetAllForGuest(int guestId)
        {
            _guestReviews = _serializer.FromCsv(FilePath);
            return _guestReviews.FindAll(r => r.GuestId == guestId);
        }

        // Get the next id
        public int NextId()
        {
            _guestReviews = _serializer.FromCsv(FilePath);
            if (_guestReviews.Count < 1)
            {
                return 1;
            }
            return _guestReviews.Max(a => a.Id) + 1;
        }
    }
}
