using BookingApp.Domain.Model;
using System.Collections.Concurrent;

namespace BookingApp.DTO.Tourist
{
    public class TouristRequestsStatisticsDto
    {
        public int WaitingCount  { get; set; }
        public float WaitingPercentage { get; set; }
        public int RejectedCount { get; set; }
        public float RejectedPercentage { get; set; }
        public int ApprovedCount { get; set; }
        public float ApprovedPercentage { get; set; }
        public int RequestsCount { get; set; }
        public ConcurrentDictionary<Location, int> LocationCounters { get; set; }
        public int LocationMax { get; set; }
        public ConcurrentDictionary<Language, int> LanguageCounters { get;set; }
        public int LanguageMax { get; set; }
        public float ApprovedPeopleCountMean { get; set; }

        public TouristRequestsStatisticsDto(int waitingCount, int rejectedCount, int approvedCount, int requestsCount,
            ConcurrentDictionary<Location, int> locationCounters, int locationMax,
            ConcurrentDictionary<Language, int> languageCounters, int languageMax, float approvedPeopleCountMean)
        {
            WaitingCount = waitingCount;
            RejectedCount = rejectedCount;
            ApprovedCount = approvedCount;
            RequestsCount = requestsCount;

            if (requestsCount > 0)
            {
                WaitingPercentage = (float)waitingCount / requestsCount;
                RejectedPercentage = (float)rejectedCount / requestsCount;
                ApprovedPercentage = (float)approvedCount / requestsCount;
            }

            LocationCounters = locationCounters;
            LocationMax = locationMax;
            LanguageCounters = languageCounters;
            LanguageMax = languageMax;
            ApprovedPeopleCountMean = approvedPeopleCountMean;
        }
    }
}
