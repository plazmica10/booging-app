using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface IGuestReviewRepository
    {
        public List<GuestReview> GetAll();
        public GuestReview Save(GuestReview guestReview);
        List<GuestReview> GetAllForGuest(int guestId);
        public GuestReview GetById(int id);
    }
}
