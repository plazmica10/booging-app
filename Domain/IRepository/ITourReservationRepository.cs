using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();
        public TourReservation? GetById(int id);
        public TourReservation SaveAppend(TourReservation tourReservation);
        public bool Update(TourReservation tourReservation);
        public List<TourReservation> GetAllByTourId(int tourId);
        public List<TourReservation> GetAllByTouristId(int touristId);

        public List<TourReservation> GetAllByTourIdArrived(int tourId);
    }
}
