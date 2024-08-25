using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface ILocationRepository
    {
        public void Refresh();
        public List<Location> GetAll();
        public Location? GetById(int id);
        public Location? SearchByCityCountry(string search);
        public Location? SearchByAll(string search);
    }
}
