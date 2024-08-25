using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourVoucherRepository : ITourVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_vouchers.csv";
        private readonly Serializer<TourVoucher> _serializer = new();

        public TourVoucherRepository() { }
        private void RemoveExpired()
        {
            List<TourVoucher> vouchers = _serializer.FromCsv(FilePath);
            List<TourVoucher> newVouchers = new List<TourVoucher>();
            foreach(TourVoucher voucher in vouchers)
            {
                if (DateTime.Now >= voucher.Expiration) { continue; }
                newVouchers.Add(voucher);
            }
            _serializer.ToCsv(FilePath, newVouchers);
        }

        public List<TourVoucher> GetAll()
        {
            RemoveExpired();
            return _serializer.FromCsv(FilePath);
        }

        public TourVoucher? GetById(int id)
        {
            return GetAll().Find(t => t.Id == id);
        }

        public List<TourVoucher> GetAllByTouristId(int touristId)
        {
            return GetAll().FindAll(c => c.TouristId == touristId);
        }
        
        public TourVoucher SaveAppend(TourVoucher tourVoucher)
        {
            List<TourVoucher> tourVouchers = _serializer.FromCsv(FilePath);
            tourVoucher.Id = tourVouchers.Count < 1 ? 1 : tourVouchers.Max(t => t.Id) + 1;
            List<TourVoucher> touristVouchers = tourVouchers.FindAll(c => c.TouristId == tourVoucher.TouristId);
            tourVoucher.Index = touristVouchers.Count < 1 ? 1 : touristVouchers.Max(t => t.Index) + 1;
            tourVouchers.Add(tourVoucher);
            _serializer.ToCsv(FilePath, tourVouchers);
            return tourVoucher;
        }

        public bool Delete(TourVoucher tourVoucher)
        {
            List<TourVoucher> tourVouchers = _serializer.FromCsv(FilePath);
            TourVoucher? found = tourVouchers.Find(t => t.Id == tourVoucher.Id);
            if (found == null) { return false; }
            tourVouchers.Remove(found);
            return _serializer.ToCsv(FilePath, tourVouchers);
        }
    }
}
