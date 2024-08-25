using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface ILanguageRepository
    {
        public void Refresh();
        public List<Language> GetAll();
        public Language? GetById(int id);
        public Language? SearchByAll(string search);
        public Language? SearchByName(string name);
        public Language? SearchByNative(string native);
    }
}
