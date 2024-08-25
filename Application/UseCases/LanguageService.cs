using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class LanguageService
    {
        private readonly ILanguageRepository _iLanguageRepository = Injector.Injector.CreateInstance<ILanguageRepository>();

        public List<Language> GetAll() { return _iLanguageRepository.GetAll(); }
        public Language? GetById(int id) { return _iLanguageRepository.GetById(id); }
        public Language? SearchByAll(string search) { return _iLanguageRepository.SearchByAll(search); }
        public Language? SearchByName(string name) { return _iLanguageRepository.SearchByName(name); }
    }
}
