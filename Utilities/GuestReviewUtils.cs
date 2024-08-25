using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Utilities
{
    public static class GuestReviewUtils
    {
        public static bool ValidateReview(int cleanliness, int ruleFollowing, string comment)
        {
            if (cleanliness == 0 || ruleFollowing == 0 || string.IsNullOrWhiteSpace(comment))
                return false;

            return true;
        }
    }
}
