using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Application.UseCases
{
    public class TourSearchService
    {
        private readonly ITourRepository            _iTourRepository            = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReservationRepository _iTourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();

        public Tour? GetById(int tourId) { return _iTourRepository.GetById(tourId); }
        public List<Tour> GetAvailableTours() { return _iTourRepository.GetAllAvailable(); }
        public List<Tour> SearchAvailableToursByAllParameters(string input) { return _iTourRepository.SearchAvailableByAllParameters(input); }
        public List<Tour> SearchAvailableToursByLocation(string input) { return _iTourRepository.SearchAvailableByLocation(input); }
        public List<Tour> SearchAvailableToursByDuration(int input) { return _iTourRepository.SearchAvailableByDuration(input); }
        public List<Tour> SearchAvailableToursByLanguage(string input) { return _iTourRepository.SearchAvailableByLanguage(input); }
        public List<Tour> SearchAvailableToursByMaxTourists(int maxTourists) { return _iTourRepository.SearchAvailableByMaxTourists(maxTourists); }
        public List<Tour> SearchAvailableToursAlternative(int tourId) { return _iTourRepository.SearchAvailableAlternative(tourId); }
        public List<Tour> GetMyToursForTourist(int touristId)
        {
            List<TourReservation> tourReservations = _iTourReservationRepository.GetAllByTouristId(touristId);

            List<Tour> tours = (from reservation in tourReservations
                                join tour in _iTourRepository.GetAll() on reservation.TourId equals tour.Id
                                select tour).Distinct().ToList();

            return tours;
        }

        public List<Tour> GetAllByGuideIdToday(int guideId) { return _iTourRepository.GetAllByGuideIdToday(guideId); }
        public List<Tour> GetAllByGuideId(int guideId) { return _iTourRepository.GetAllByGuideId(guideId); }
        public List<Tour> GetAllByGuideIdFinished(int guideId) { return _iTourRepository.GetAllByGuideIdFinished(guideId); }
        public Tour? GetStartedByGuideId(int guideId) { return _iTourRepository.GetByGuideIdStarted(guideId); }
    }
}
