using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.DTO.Tourist;
using BookingApp.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.Application.UseCases
{
    public class TourStatisticsService
    {
        private readonly ITourRepository            _iTourRepository            = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReservationRepository _iTourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        private readonly ITourRequestRepository     _iTourRequestRepository     = Injector.Injector.CreateInstance<ITourRequestRepository>();

        private TouristsAgeStatisticsDto AnalyzeTouristAges(Tour tour)
        {
            List<TourReservation> peopleArrived = _iTourReservationRepository.GetAllByTourIdArrived(tour.Id);
            int under18Count = 0;
            int between18And50Count = 0;
            int over50Count = 0;
            foreach (var reservation in peopleArrived)
            {
                int age = DateTime.Now.Year - reservation.Person.BirthYear;
                if      (age < 18)  { under18Count++;        }
                else if (age <= 50) { between18And50Count++; }
                else                { over50Count++;         }
            }
            return new TouristsAgeStatisticsDto(tour.Id, tour.Name, peopleArrived.Count, under18Count, between18And50Count, over50Count);
        }

        public List<TouristsAgeStatisticsDto> GetStatisticsForYear(int guideId, int year)
        {
            List<TouristsAgeStatisticsDto> allStatsForYear = new();
            foreach (Tour tour in _iTourRepository.GetAllByGuideIdFinished(guideId))
            {
                if (year != 0 && tour.StartsAt.Year != year) { continue; }
                allStatsForYear.Add(AnalyzeTouristAges(tour));
            }
            return allStatsForYear;
        }


        public BestTourStatsDto? BestTourForYear(int guideId, int year)
        {
            int max = 0;
            Tour? bestTour = null;
            foreach (Tour tour in _iTourRepository.GetAllByGuideId(guideId))
            {
                if (year != 0 && tour.StartsAt.Year != year) { continue; }

                int touristsNum = _iTourReservationRepository.GetAllByTourIdArrived(tour.Id).Count;
                if (touristsNum > max) { max = touristsNum; bestTour = tour; }
            }
            if (bestTour == null) { return null; }
            return new BestTourStatsDto(bestTour.Name, bestTour.Location, bestTour.Language, bestTour.Description, max);
        }

        public TouristRequestsStatisticsDto GetTouristStatistics(int touristId, int year)
        {
            List<TourRequest> requests = _iTourRequestRepository.TourRequestGetAllByTouristId(touristId);

            ConcurrentDictionary<Location, int> locationCounters = new ConcurrentDictionary<Location, int>();
            int locationMax = 0;
            ConcurrentDictionary<Language, int> languageCounters = new ConcurrentDictionary<Language, int>();
            int languageMax = 0;

            int waitingCount  = 0;
            int rejectedCount = 0;
            int approvedCount = 0;
            int requestsCount = 0;
            int approvedPeopleCountTotal = 0;

            foreach (TourRequest request in requests)
            {
                if (year != 0 && request.SpanStart.Year != year) { continue; }
                switch (request.Status)
                {
                    case TourRequestStatus.Waiting:  waitingCount++;  break;
                    case TourRequestStatus.Rejected: rejectedCount++; break;
                    case TourRequestStatus.Approved: approvedCount++; approvedPeopleCountTotal += request.Reservations.Count; break;
                }
                requestsCount++;
                locationCounters.AddOrUpdate(request.Location, 1, (_, count) =>
                {
                    int newValue = count + 1;
                    if (locationMax < newValue) locationMax = newValue;
                    return newValue;
                });
                languageCounters.AddOrUpdate(request.Language, 1, (_, count) =>
                {
                    int newValue = count + 1;
                    if (languageMax < newValue) languageMax = newValue;
                    return newValue;
                });
            }

            float approvedPeopleCountMean = 0.0f;
            if (approvedCount > 0) { approvedPeopleCountMean = (float)approvedPeopleCountTotal / approvedCount; }
            return new TouristRequestsStatisticsDto(waitingCount, rejectedCount, approvedCount, requestsCount, locationCounters, locationMax, languageCounters, languageMax, approvedPeopleCountMean);
        }

        public GuideRequestsStatisticsDto GetGuideStatistics(int year, Location? location, Language? language)
        {
            ConcurrentDictionary<int, int> yearCounter = new ConcurrentDictionary<int, int>();
            ConcurrentDictionary<int, int> monthCounter = new ConcurrentDictionary<int, int>();

            List<TourRequest> filteredTourRequests = new List<TourRequest>();

            foreach (TourRequest request in _iTourRequestRepository.TourRequestGetAll())
            {
                if (year != 0 && request.SpanEnd.Year != year) { continue; }
                if (location != null && location != request.Location) { continue; }
                if (language != null && language != request.Language) { continue; }

                filteredTourRequests.Add(request);

                yearCounter.AddOrUpdate(request.SpanStart.Year, 1, (_, count) => count + 1);
                monthCounter.AddOrUpdate(request.SpanStart.Month, 1, (_, count) => count + 1);
            }
             
            return new GuideRequestsStatisticsDto(filteredTourRequests, yearCounter, monthCounter);
        }

        public Location? GetMostRequestedLocation() {

            ConcurrentDictionary<Location, int> locationCounters = new ConcurrentDictionary<Location, int>();
            foreach (TourRequest request in _iTourRequestRepository.TourRequestGetAll())
            {
                locationCounters.AddOrUpdate(request.Location, 1, (_, count) => count + 1);
            }
            List<KeyValuePair<Location, int>> countersToSort = locationCounters.ToList();
            countersToSort.Sort((x, y) => y.Value.CompareTo(x.Value));
            if (countersToSort.Count > 0) { return countersToSort[0].Key; }
            return null;
        }

        public Language? GetMostRequestedLanguage()
        {
            ConcurrentDictionary<Language, int> languageCounters = new ConcurrentDictionary<Language, int>();
            foreach (TourRequest request in _iTourRequestRepository.TourRequestGetAll())
            {
                languageCounters.AddOrUpdate(request.Language, 1, (_, count) => count + 1);
            }
            List<KeyValuePair<Language, int>> countersToSort = languageCounters.ToList();
            countersToSort.Sort((x, y) => y.Value.CompareTo(x.Value));
            if (countersToSort.Count > 0) { return countersToSort[0].Key; }
            return null;
        }
    }
}
