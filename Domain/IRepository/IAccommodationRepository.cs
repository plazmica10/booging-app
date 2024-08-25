using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation? GetById(int id);
        public List<Accommodation> GetByOwnerId(int ownerId);
        List<Accommodation> SearchByName(List<Accommodation> accommodations, string name);
        List<Accommodation> SearchByLocation(List<Accommodation> accommodations, string location);
        List<Accommodation> SearchByType(List<Accommodation> accommodations, AccommodationType? type);
        List<Accommodation> SearchByGuestCount(List<Accommodation> accommodations, int guestCount);
        List<Accommodation> SearchByStayLength(List<Accommodation> accommodations, int stayLength);
        List<Accommodation> SearchByAllParameters(string name, string location, AccommodationType? type, int guestCount, int stayLength);
        public Accommodation Save(Accommodation accommodation);
        public void Update(Accommodation accommodation);
        List<Accommodation> GetByLocation(string city, string country);
    }
}
