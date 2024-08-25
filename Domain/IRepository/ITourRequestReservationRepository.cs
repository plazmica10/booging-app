using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourRequestReservationRepository
    {
        public List<TourRequestReservation> GetAll();
        public TourRequestReservation? GetById(int id);
        public List<TourRequestReservation> GetAllByTourRequestId(int tourRequestId);
        public TourRequestReservation SaveAppend(TourRequestReservation tourRequestReservation);
    }
}
