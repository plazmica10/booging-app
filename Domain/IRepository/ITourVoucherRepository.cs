using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourVoucherRepository
    {
        public List<TourVoucher> GetAll();
        public TourVoucher? GetById(int id);
        public List<TourVoucher> GetAllByTouristId(int touristId);
        public TourVoucher SaveAppend(TourVoucher tourVoucher);
        public bool Delete(TourVoucher tourVoucher);
    }
}
