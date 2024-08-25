using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IOwnerReviewRepository
    {
        public OwnerReview Save(OwnerReview review);
        public OwnerReview GetOwnerReview(int reservationId);
        public int CountByOwnerId(int ownerId);
        public double AverageRatingByOwnerId(int ownerId);
        public List<OwnerReview> GetAllForOwner(int ownerId);
    }
}
