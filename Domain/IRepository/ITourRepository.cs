using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();
        public Tour? GetById(int tourId);
        public Tour SaveAppend(Tour tourItem);
        public bool Update(Tour tour);
        public bool Delete(Tour tour);
        public List<Tour> GetAllAvailable();
        public List<Tour> GetAllByGuideId(int guideId);
        public List<Tour> GetAllByGuideIdFinished(int guideId);
        public List<Tour> GetAllByGuideIdToday(int guideId);
        public Tour? GetByGuideIdStarted(int guideId);
        public List<Tour> SearchAvailableByAllParameters(string input);
        public List<Tour> SearchAvailableByLocation(string input);
        public List<Tour> SearchAvailableByDuration(int duration);
        public List<Tour> SearchAvailableByLanguage(string input);
        public List<Tour> SearchAvailableByMaxTourists(int maxTourists);
        public List<Tour> SearchAvailableAlternative(int tourId);
    }
}
