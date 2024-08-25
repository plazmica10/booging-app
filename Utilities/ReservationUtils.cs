using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MenuNavigation;

namespace BookingApp.Utilities
{
    public static class ReservationUtils
    {
        public static bool ValidateReview(int ownerRating, int cleanliness, string comment, DateTime checkOut)
        {
            if (ownerRating == 0 || cleanliness == 0)
            {
                MessageBox.Show(TranslationSource.Instance["RateOwnandProp"]);
                return false;
            }

            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show(TranslationSource.Instance["LeaveComm"]);
                return false;
            }
            return true;
        }

        public static bool ExpiredReviewDate(DateTime checkOut)
        {
            return (DateTime.Now - checkOut).TotalDays > 5;
        }

        public static bool ValidateCancellation(DateTime checkIn, DateTime checkOut, int refundPeriod)
        {
            var timeUntilCheckIn = checkIn - DateTime.Now;
            if (checkIn < DateTime.Today)
            {
                MessageBox.Show(TranslationSource.Instance["CancelResProg"], TranslationSource.Instance["CancelRes"], MessageBoxButton.OK);
                return false;
            }
            if (timeUntilCheckIn.TotalHours < 24)
            {
                MessageBox.Show(TranslationSource.Instance["24H"], TranslationSource.Instance["CancelRes"], MessageBoxButton.OK);
                return false;
            }

            if (timeUntilCheckIn.TotalDays < refundPeriod)
            {
                MessageBox.Show(TranslationSource.Instance["ref1"] + refundPeriod + TranslationSource.Instance["ref2"], TranslationSource.Instance["CancelRes"], MessageBoxButton.OK);
                return false;
            }
            return true;
        }
        public static bool ValidateReschedulingDates(DateTime start, DateTime end, AccommodationReservation reservation)
        {
            if (!ValidateDates(start, end))
            {
                return false;
            }

            if (start == reservation.CheckIn && end == reservation.CheckOut)
            {
                MessageBox.Show(TranslationSource.Instance["inv1"]);
                return false;
            }
            return true;
        }
        public static bool ValidateDates(DateTime start, DateTime end)
        {
            if (start <= DateTime.Today || end < start || end <= DateTime.Today || end == start)
            {
                MessageBox.Show(TranslationSource.Instance["inv2"]);
                return false;
            }
            return true;
        }

    }
}
