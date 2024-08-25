using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_tours.csv";
        private readonly Serializer<Tour> _serializer = new();

        public TourRepository() { }

        public List<Tour> GetAll() { return _serializer.FromCsv(FilePath); }

        public Tour? GetById(int tourId) { return _serializer.FromCsv(FilePath).Find(t => t.Id == tourId); }

        public Tour SaveAppend(Tour tourItem)
        {
            List<Tour> tours = _serializer.FromCsv(FilePath);
            tourItem.Id = tours.Count < 1 ? 1 : tours.Max(t => t.Id) + 1;
            tours.Add(tourItem);
            _serializer.ToCsv(FilePath, tours);
            return tourItem;
        }

        public bool Update(Tour tour)
        {
            List<Tour> tours = _serializer.FromCsv(FilePath);
            Tour? current = tours.Find(t => t.Id == tour.Id);
            if (current == null) { return false; }
            int index = tours.IndexOf(current);
            tours.Remove(current);
            tours.Insert(index, tour);
            return _serializer.ToCsv(FilePath, tours);
        }

        public bool Delete(Tour tour)
        {
            List<Tour> tours = _serializer.FromCsv(FilePath);
            Tour? found = tours.Find(t => t.Id == tour.Id);
            if (found == null) { return false; }
            tours.Remove(found);
            return _serializer.ToCsv(FilePath, tours);
        }


        public List<Tour> GetAllAvailable()
        {
            //return _serializer.FromCSV(FilePath).FindAll(t => t.Status == TourStatus.NotStarted || t.Status == TourStatus.Started); // TODO: COMMENTED OUT FOR TESTING
            return _serializer.FromCsv(FilePath); // return all ... FOR TESTING ... should only return NotStarted & Started
        }
        public List<Tour> GetAllByGuideId(int guideId) { return _serializer.FromCsv(FilePath).FindAll(a => a.GuideId == guideId); }
        public List<Tour> GetAllByGuideIdFinished(int guideId) { return _serializer.FromCsv(FilePath).FindAll(t => t.GuideId == guideId && t.Status == TourStatus.Finished); }
        public List<Tour> GetAllByGuideIdToday(int guideId) { return _serializer.FromCsv(FilePath).FindAll(t => t.GuideId == guideId && t.StartsAt.Date == DateTime.Today); }
        public Tour? GetByGuideIdStarted(int guideId) { return _serializer.FromCsv(FilePath).Find(t => t.GuideId == guideId && t.Status == TourStatus.Started); }

        public List<Tour> SearchAvailableByAllParameters(string input)
        {
            string[] inputs = input.Split(";");
            string inputLocation    = inputs.Length >= 1 ? inputs[0] : "";
            string inputDuration    = inputs.Length >= 2 ? inputs[1] : "";
            string inputLanguage    = inputs.Length >= 3 ? inputs[2] : "";
            string inputMaxTourists = inputs.Length >= 4 ? inputs[3] : "";

            List<Tour> tours = GetAllAvailable();
            if (!string.IsNullOrEmpty(inputLocation)) { tours = MatchByLocation(tours, inputLocation); }
            if (!string.IsNullOrEmpty(inputDuration)) { tours = MatchByDuration(tours, Convert.ToInt32(inputDuration)); }
            if (!string.IsNullOrEmpty(inputLanguage)) { tours = MatchByLanguage(tours, inputLanguage); }
            if (!string.IsNullOrEmpty(inputMaxTourists)) { tours = MatchByMaxTourists(tours, Convert.ToInt32(inputMaxTourists)); } 
            return tours;
        }
        public List<Tour> SearchAvailableByLocation(string input) { return MatchByLocation(GetAllAvailable(), input); }
        public List<Tour> SearchAvailableByDuration(int duration) { return MatchByDuration(GetAllAvailable(), duration); }
        public List<Tour> SearchAvailableByLanguage(string input) { return MatchByLanguage(GetAllAvailable(), input); }
        public List<Tour> SearchAvailableByMaxTourists(int maxTourists) { return MatchByMaxTourists(GetAllAvailable(), maxTourists); }
        public List<Tour> SearchAvailableAlternative(int tourId)
        {
            List<Tour> tours = GetAllAvailable();

            Tour? tour = tours.Find(t => t.Id == tourId);
            if (tour == null) { return new List<Tour>(); }

            return tours.Where(a => a.Id != tour.Id &&
                                      a.Location.Id == tour.Location.Id &&
                                      a.SpaceLeft > 0).ToList();
        }
        private List<Tour> MatchByLocation(List<Tour> tours, string input)
        {
            return tours.FindAll(a =>
                (a.Location.Country.IndexOf(input, 0, StringComparison.OrdinalIgnoreCase) != -1 ||
                a.Location.City.IndexOf(input, 0, StringComparison.OrdinalIgnoreCase) != -1)
            );
        }
        private List<Tour> MatchByDuration(List<Tour> tours, int duration)
        {
            return tours.FindAll(a => a.Duration >= duration);
        }
        private List<Tour> MatchByLanguage(List<Tour> tours, string input)
        {
            return tours.FindAll(a =>
                (a.Language.Name.IndexOf(input, 0, StringComparison.OrdinalIgnoreCase) != -1 ||
                a.Language.Native.IndexOf(input, 0, StringComparison.OrdinalIgnoreCase) != -1)
            );
        }
        private List<Tour> MatchByMaxTourists(List<Tour> tours, int maxTourists)
        {
            return tours.FindAll(a => a.MaxTourists >= maxTourists);
        }
    }
}
