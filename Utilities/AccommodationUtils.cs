using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BookingApp.Domain.Model;
using MenuNavigation;

namespace BookingApp.Utilities
{
    public static class AccommodationUtils
    {
        public static List<DateTime> GetAllDates(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToList();
        }

        public static List<DateTime> GetReservedDates(List<AccommodationReservation> reservations)
        {
            return reservations
                .Where(r => r.Status is ReservationStatus.Reserved or ReservationStatus.InProgress or ReservationStatus.Renovation)
                .SelectMany(r => GetAllDates(r.CheckIn, r.CheckOut))
                .ToList();
        }

        public static List<Tuple<DateTime, DateTime>> GetPossibleStays(List<DateTime> availableDates, int stayLength)
        {
            var firstDates = availableDates.Where(d => availableDates.Contains(d.AddDays(stayLength))).ToList();
            return firstDates.Select(d => new Tuple<DateTime, DateTime>(d, d.AddDays(stayLength))).ToList();
        }

        public static bool IsReservationValid(Tuple<DateTime, DateTime>? date, int guestCount, int maxGuests)
        {
            if (date == null)
            {
                MessageBox.Show(TranslationSource.Instance["au1"]);
                return false;
            }

            if (guestCount <= 0 || guestCount > maxGuests)
            {
                MessageBox.Show(TranslationSource.Instance["au2"]);
                return false;
            }

            return true;
        }

        public static bool AreDatesValid(DateTime checkIn, DateTime checkOut)
        {
            return checkIn > DateTime.Today && checkOut > DateTime.Today && checkIn < checkOut;
        }

        public static bool IsSearchValid(DateTime checkIn, DateTime checkOut, int stayLength, int minDays)
        {
            if (!AreDatesValid(checkIn, checkOut))
            {
                MessageBox.Show(TranslationSource.Instance["au3"]);
                return false;
            }

            if (stayLength < minDays)
            {
                MessageBox.Show(TranslationSource.Instance["au4"] + minDays + TranslationSource.Instance["au5"]);
                return false;
            }

            return true;
        }
    }
}
