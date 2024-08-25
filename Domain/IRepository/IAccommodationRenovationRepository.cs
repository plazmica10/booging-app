using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IAccommodationRenovationRepository
    {
        public List<AccommodationRenovation> GetAll();
        public AccommodationRenovation GetById(int id);
        public List<AccommodationRenovation> GetByUserId(int userId);
        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation);
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
    }
}
