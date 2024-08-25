using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;

namespace BookingApp.Application.UseCases
{
    public class AccommodationStatisticsService
    {
        private readonly IAccommodationReservationRepository _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        private readonly IRescheduleRequestRepository _rescheduleRequestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();


        public List<YearStatisticsDto> GetYearStatisticsForOwner(int ownerId, int accommodationId)
        {
            List<YearStatisticsDto> yearStatistics = new List<YearStatisticsDto>();
            var res = _reservationRepository.GetForOwner(ownerId).Where(r => r.AccommodationId == accommodationId).ToList();
            var reservations = res.Where(r => r.Status != ReservationStatus.Renovation && r.Status != ReservationStatus.RenovationOver).ToList();
            var groupedReservations = reservations.GroupBy(r => r.CheckOut.Year);
            foreach (var group in groupedReservations)
            {
                YearStatisticsDto yearStatistic = new YearStatisticsDto
                {
                    Year = group.Key, Reservations = group.Count(), Cancellations = group.Count(r => r.Status == ReservationStatus.Canceled),
                    ReschedulingCount = FillReschedule(ownerId, accommodationId, group.Key), RenovationCount = FillRenovation(reservations, group.Key)
                };
                yearStatistics.Add(yearStatistic);
            }

            return yearStatistics.OrderByDescending(stat => stat.Year).ToList();
        }

        private int FillReschedule(int ownerId, int accommodationId, int year)
        {
            return _rescheduleRequestRepository.GetByOwnerId(ownerId).Count(r => r.AccommodationId == accommodationId && r.NewEndDate.Year == year);
        }

        private int FillRenovation(List<AccommodationReservation> reservations, int year)
        {
            var reservationIds = reservations.Where(r => r.CheckOut.Year == year).Select(r => r.Id).ToList();
            var rec = _renovationRecommendationRepository.GetAll().Where(r => reservationIds.Contains(r.ReservationId));
            return rec.Count();
        }

        public int GetBestYear(int ownerId, int accommodationId)
        {
            var res = _reservationRepository.GetForOwner(ownerId).Where(r => r.AccommodationId == accommodationId).ToList();
            var reservations = res.Where(r => r.Status != ReservationStatus.Renovation && r.Status != ReservationStatus.RenovationOver).ToList();
            var yearDays = reservations.GroupBy(r => r.CheckIn.Year)
                .Select(g => new { Year = g.Key, TotalDays = g.Sum(r => (r.CheckOut - r.CheckIn).Days) });

            var bestYear = yearDays.MaxBy(y => y.TotalDays)?.Year;

            return bestYear ?? 0;
        }

        public List<MonthStatisticsDto> GetMonthStatisticsForOwner(int accommodationId, int year)
        {
            List<MonthStatisticsDto> monthStatistics = new List<MonthStatisticsDto>();
            var res = _reservationRepository.GetByAccommodationId(accommodationId).Where(r => r.CheckOut.Year == year).ToList();
            var reservations = res.Where(r => r.Status != ReservationStatus.Renovation && r.Status != ReservationStatus.RenovationOver).ToList();
            var groupedReservations = reservations.GroupBy(r => r.CheckOut.Month);
            foreach (var group in groupedReservations)
            {
                MonthStatisticsDto monthStatistic = new MonthStatisticsDto
                {
                    Month = group.Key, MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key),
                    Reservations = group.Count(), Cancellations = group.Count(r => r.Status == ReservationStatus.Canceled),
                    ReschedulingCount = FillRescheduleForMonth(accommodationId, group.Key, year), RenovationCount = FillRenovationForMonth(reservations, group.Key, year),
                    Occupation = Math.Round((double)group.Sum(r => (r.CheckOut - r.CheckIn).Days) / DateTime.DaysInMonth(year, group.Key) * 100, 2),
                    DaysOccupied = group.Sum(r => (r.CheckOut - r.CheckIn).Days)
                };
                monthStatistics.Add(monthStatistic);
            }
            return monthStatistics;
        }

        private int FillRescheduleForMonth(int accommodationId, int month, int year)
        {
            return _rescheduleRequestRepository.GetByAccommodationId(accommodationId).Count(r => r.NewEndDate.Month == month && r.NewEndDate.Year == year);
        }

        private int FillRenovationForMonth(List<AccommodationReservation> reservations, int month, int year)
        {
            var reservationIds = reservations.Where(r => r.CheckOut.Month == month && r.CheckOut.Year == year).Select(r => r.Id).ToList();
            var rec = _renovationRecommendationRepository.GetAll().Where(r => reservationIds.Contains(r.ReservationId));
            return rec.Count();
        }

        public string GetBestMonth(int accommodationId, int year)
        {
            var res = _reservationRepository.GetByAccommodationId(accommodationId).Where(r => r.CheckOut.Year == year).ToList();
            var reservations = res.Where(r => r.Status != ReservationStatus.Renovation && r.Status != ReservationStatus.RenovationOver).ToList();
            var monthDays = reservations.GroupBy(r => r.CheckOut.Month)
                .Select(g => new { Month = g.Key, TotalDays = g.Sum(r => (r.CheckOut - r.CheckIn).Days) });

            var bestMonth = monthDays.MaxBy(y => y.TotalDays)?.Month;

            return bestMonth.HasValue ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(bestMonth.Value) : "No data";
        }
    }
}
