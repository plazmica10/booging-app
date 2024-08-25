using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/og_accommodations.csv";
        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;
        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCsv(FilePath);
        }

        public List<Accommodation> GetAll()
        {
            _accommodations = _serializer.FromCsv(FilePath);
            return _accommodations;
        }

        public Accommodation GetById(int id)
        {
            _accommodations = _serializer.FromCsv(FilePath);
            return _accommodations.FirstOrDefault(a => a.Id == id) ?? throw new InvalidOperationException();
        }

        public List<Accommodation> GetByOwnerId(int ownerId)
        {
            _accommodations = _serializer.FromCsv(FilePath);
            return _accommodations.FindAll(a => a.OwnerId == ownerId);
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations = _serializer.FromCsv(FilePath);
            _accommodations.Add(accommodation);
            _serializer.ToCsv(FilePath, _accommodations);
            return accommodation;
        }

        public void Update(Accommodation accommodation)
        {
            _accommodations = _serializer.FromCsv(FilePath);
            var index = _accommodations.FindIndex(a => a.Id == accommodation.Id);
            if (index == -1) throw new InvalidOperationException();
            _accommodations[index] = accommodation;
            _serializer.ToCsv(FilePath, _accommodations);
        }

        public int NextId()
        {
            _accommodations = _serializer.FromCsv(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(a => a.Id) + 1;
        }

        public List<Accommodation> GetByLocation(string city, string country)
        {
            _accommodations = _serializer.FromCsv(FilePath);
            return _accommodations.FindAll(a => a.City == city && a.Country == country);
        }

        //search methods will be reused in the future
        public List<Accommodation> SearchByName(List<Accommodation> accommodations, string name)
        {
            return accommodations.FindAll(a => a.Name.ToLower().Contains(name.ToLower()));
        }

        public List<Accommodation> SearchByLocation(List<Accommodation> accommodations, string location)
        {
            return  accommodations.FindAll(a=> location.ToLower().Contains(a.City.ToLower()) || location.ToLower().Contains(a.Country.ToLower()));
            //accommodations.FindAll(a => a.City.ToLower().Contains(location.ToLower()) || a.Country.ToLower().Contains(location.ToLower()));
        }

        public List<Accommodation> SearchByType(List<Accommodation> accommodations, AccommodationType? type)
        {
            return accommodations.FindAll(a => a.Type == type);
        }

        public List<Accommodation> SearchByGuestCount(List<Accommodation> accommodations, int guestCount)
        {
            return accommodations.FindAll(a => a.MaxGuests >= guestCount);
        }

        public List<Accommodation> SearchByStayLength(List<Accommodation> accommodations,int stayLength)
        {
            return accommodations.FindAll(a => a.MinDays <= stayLength);
        }

        public List<Accommodation> SearchByAllParameters(string name, string location, AccommodationType? type, int guestCount, int stayLength)
        {
            _accommodations = _serializer.FromCsv(FilePath);
            if (type != null) _accommodations = SearchByType(_accommodations, type.Value);
            if(!string.IsNullOrEmpty(location)) _accommodations = _accommodations.FindAll(a => location.ToLower().Contains(a.City.ToLower()) || location.ToLower().Contains(a.Country.ToLower()));
            if (!string.IsNullOrEmpty(location))_accommodations = SearchByLocation(_accommodations, location);
            if (guestCount > 0) _accommodations = SearchByGuestCount(_accommodations, guestCount);
            if (stayLength > 0) _accommodations = SearchByStayLength(_accommodations, stayLength);
            return _accommodations;
        }
    }
}
