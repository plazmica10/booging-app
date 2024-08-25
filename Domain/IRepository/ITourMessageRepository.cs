using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourMessageRepository
    {
        public List<TourMessage> GetAll();
        public TourMessage? GetById(int id);
        public TourMessage SaveAppend(TourMessage tourVoucher);
        public bool Delete(TourMessage tourVoucher);
        public List<TourMessage> GetAllByTouristId(int touristId);
    }
}
