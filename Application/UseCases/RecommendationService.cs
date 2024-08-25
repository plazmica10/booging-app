using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;

namespace BookingApp.Application.UseCases
{
    public class RecommendationService
    {
        private readonly AccommodationStatisticsService _statisticsService = new();
        private readonly AccommodationService _accommodationService = new();
        private readonly PictureService _pictureService = new();


        public RecommendationService()
        {

        }

        public List<RecommendationDto> GetRecommendations(int ownerId)
        {
            var recommendations = GetBadRecommendations(ownerId);
            recommendations.AddRange(GetGoodRecommendations(ownerId));
            int i = 0;
            foreach (var recommendation in recommendations)
            {
                recommendation.RecommendationId = i++;
            }
            return recommendations;
        }

        public List<RecommendationDto> GetBadRecommendations(int ownerId)
        {
            var accommodations = _accommodationService.GetByOwnerId(ownerId).Where(a => a.IsDeleted == false).ToList();
            List<RecommendationDto> recommendations = new();
            foreach (var accommodation in accommodations)
            {
                var yearStatistics = _statisticsService.GetYearStatisticsForOwner(ownerId, accommodation.Id);
                if (yearStatistics.Count == 0) continue;
                var monthStatistics = _statisticsService.GetMonthStatisticsForOwner(accommodation.Id, yearStatistics[0].Year);
                int totalDaysOccupied = monthStatistics.Sum(m => m.DaysOccupied);
                int numReservations = yearStatistics[0].Reservations;

                if (totalDaysOccupied < 20 || numReservations < 5)
                {
                    string reason = "";
                    if (totalDaysOccupied < 180)
                        reason += "The accommodation was occupied for less than 180 days in the last year. ";
                    if (numReservations < 30)
                        reason += "The accommodation had less than 30 reservations in the last year. ";
                    var recommendation = new RecommendationDto
                    {
                        RecommendationText = reason,
                        AccommodationId = accommodation.Id,
                        AccommodationName = accommodation.Name,
                        AccommodationLocation = $"{accommodation.Location.City}, {accommodation.Location.Country}",
                        AccommodationPhotos = _pictureService.GetPictureLocations(accommodation.Photos),
                        ReservationsInLastYear = numReservations,
                        Occupation = Math.Round((double)totalDaysOccupied / 365.0, 2),
                        Bad = true
                    };
                    recommendations.Add(recommendation);
                }

            }

            return recommendations;
        }

        public List<RecommendationDto> GetGoodRecommendations(int ownerId)
        {
            List<RecommendationDto> recommendations = new();
            List<Accommodation> accommodations = _accommodationService.GetByOwnerId(ownerId).Where(a => a.IsDeleted == false).ToList();
            List<Location> locations = accommodations.Select(a => a.Location).ToList().Distinct().ToList();
            foreach (var location in locations)
            {
                accommodations = _accommodationService.GetAtLocationForOwner(ownerId, location).Where(a => a.IsDeleted == false).ToList();
                List<Accommodation> goodAccommodations = new();
                foreach (var accommodation in accommodations)
                {
                    var yearStatistics = _statisticsService.GetYearStatisticsForOwner(ownerId, accommodation.Id);
                    if (yearStatistics.Count == 0) continue;
                    var monthStatistics = _statisticsService.GetMonthStatisticsForOwner(accommodation.Id, yearStatistics[0].Year);
                    int totalDaysOccupied = monthStatistics.Sum(m => m.DaysOccupied);
                    int numReservations = yearStatistics[0].Reservations;

                    if (totalDaysOccupied >= 20 && numReservations >= 5)
                    {
                        goodAccommodations.Add(accommodation);
                    }
                }

                if (goodAccommodations.Count > 0)
                {
                    RecommendationDto rec = new();
                    rec.RecommendedLocation = location;
                    rec.AccommodationLocation = $"{location.City}, {location.Country}";
                    foreach (var accommodation in goodAccommodations)
                    {
                        var accomm = new RecommendationAccommodationDto
                        {
                            AccommodationId = accommodation.Id,
                            AccommodationName = accommodation.Name,
                            AccommodationLocation = accommodation.Location,
                            AccommodationPhoto = _pictureService.GetPictureLocation(accommodation.Photos[0]),
                            ReservationsInLastYear = _statisticsService.GetYearStatisticsForOwner(ownerId, accommodation.Id)[0].Reservations
                        };
                        rec.RecommendedAccommodations.Add(accomm);
                    }
                    recommendations.Add(rec);
                }
            }

            return recommendations;
        }
    }
}
