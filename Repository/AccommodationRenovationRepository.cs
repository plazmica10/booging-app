using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;

namespace BookingApp.Repository
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/og_renovations.csv";

        private readonly Serializer<AccommodationRenovation> _serializer;

        private List<AccommodationRenovation> _accommodationRenovations;

        public AccommodationRenovationRepository()
        {
            _serializer = new Serializer<AccommodationRenovation>();
            _accommodationRenovations = _serializer.FromCsv(FilePath);
        }

        public List<AccommodationRenovation> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public AccommodationRenovation GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(c => c.Id == id) ?? throw new InvalidOperationException();
        }

        public List<AccommodationRenovation> GetByUserId(int userId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.AccommodationForRenovation.OwnerId == userId);
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            _accommodationRenovations = _serializer.FromCsv(FilePath);
            _accommodationRenovations.Add(accommodationRenovation);
            _serializer.ToCsv(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            _accommodationRenovations = _serializer.FromCsv(FilePath);
            var index = _accommodationRenovations.FindIndex(c => c.Id == accommodationRenovation.Id);
            if (index == -1) throw new InvalidOperationException();
            _accommodationRenovations[index] = accommodationRenovation;
            _serializer.ToCsv(FilePath, _accommodationRenovations);
            return accommodationRenovation;
        }

        public int NextId()
        {
            _accommodationRenovations = _serializer.FromCsv(FilePath);
            return _accommodationRenovations.Count < 1 ? 1 : _accommodationRenovations.Max(c => c.Id) + 1;
        }
    }
}
