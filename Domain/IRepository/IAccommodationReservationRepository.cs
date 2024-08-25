using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;

namespace BookingApp.Domain.IRepository
{
    public interface IAccommodationReservationRepository
    {
        public List<AccommodationReservation> GetAll();
        public List<AccommodationReservation> GetForOwner(int ownerId);
        public List<AccommodationReservation> GetByUserId(int userId);
        public List<AccommodationReservation> GetForPeriodByUserId(int userId,bool Past);
        public AccommodationReservation GetById(int id);
        public AccommodationReservation Save(AccommodationReservation accommodationReservation);
        public int NextId();
        public void Delete(AccommodationReservation accommodationReservation);
        public AccommodationReservation Update(AccommodationReservation accommodationReservation);
        public List<AccommodationReservation> GetByAccommodationId(int accommodationId);

        public List<Tuple<DateTime, DateTime>> GetAvailableDates(int accommodationId, DateTime startDate,
            DateTime endDate, int stayLength);

        public List<AccommodationReservation> GetValidReservationsByGuestId(int guestId);
    }
}
