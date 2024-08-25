using BookingApp.Domain.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BookingApp.DTO.Guide
{
    public class GuideRequestsStatisticsDto
    {
        public List<TourRequest> FilteredTourRequests { get; set; }
        public ConcurrentDictionary<int, int> YearCounter { get; set; }
        public ConcurrentDictionary<int, int> MonthCounter { get; set; }
        public GuideRequestsStatisticsDto(List<TourRequest> filteredTourRequests, ConcurrentDictionary<int, int> yearCounter, ConcurrentDictionary<int, int> monthCounter)
        {
            FilteredTourRequests = filteredTourRequests;
            YearCounter = yearCounter;
            MonthCounter = monthCounter;
        }
    }
}
