using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class LocationService
    {
        private readonly ILocationRepository _iLocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();

        public List<Location> GetAll() { return _iLocationRepository.GetAll(); }
        public Location? GetById(int id) { return _iLocationRepository.GetById(id); }
        public Location? SearchByCityCountry(string search) { return _iLocationRepository.SearchByCityCountry(search); }
        public Location? SearchByAll(string search) { return _iLocationRepository.SearchByAll(search); }
    }
}
