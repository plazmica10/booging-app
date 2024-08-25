using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private const string FilePath = "../../../Resources/Data/static_languages.csv";

        private readonly Serializer<Language> _serializer = new();
        private List<Language> _languages;

        public LanguageRepository()
        {
            _languages = _serializer.FromCsv(FilePath);
        }

        public void Refresh()
        {
            _languages = _serializer.FromCsv(FilePath);
        }

        public List<Language> GetAll()
        {
            return _languages;
        }

        public Language? GetById(int id)
        {
            return _languages.Find(c => c.Id == id);
        }

        public Language? SearchByAll(string search)
        {
            var keywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return _languages.FirstOrDefault(language =>
                keywords.All(keyword =>
                    (language.Name + language.Native).IndexOf(keyword, StringComparison.OrdinalIgnoreCase) != -1
                )
            );
        }

        public Language? SearchByName(string name)
        {
            return _languages.FirstOrDefault(language =>
                language.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1
            );
        }

        public Language? SearchByNative(string native)
        {
            return _languages.FirstOrDefault(language =>
                language.Native.IndexOf(native, StringComparison.OrdinalIgnoreCase) != -1
            );
        }

    }
}
