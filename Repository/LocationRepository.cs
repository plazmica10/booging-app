using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/static_worldcities.csv";

        private readonly Serializer<Location> _serializer = new();
        private List<Location> _locations;

        public LocationRepository()
        {
            _locations = _serializer.FromCsv(FilePath);
        }

        public void Refresh()
        {
            _locations = _serializer.FromCsv(FilePath);
        }

        public List<Location> GetAll()
        {
            return _locations;
        }

        public Location? GetById(int id)
        {
            return _locations.Find(c => c.Id == id);
        }

        public Location? SearchByCityCountry(string search)
        {
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _locations.FirstOrDefault(location =>
                keywords.All(keyword =>
                    (location.City + location.Country).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1
                )
            );
        }

        public Location? SearchByAll(string search)
        {
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _locations.FirstOrDefault(location =>
                keywords.All(keyword =>
                    (location.Country + location.SubCountry + location.City)
                        .IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) != -1
                )
            );
        }
    }
}
