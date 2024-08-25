using BookingApp.Domain.Model;
using System;

namespace BookingApp.DTO.Guide
{
    public class RequestsFilterDto
    {
        public Location? Location { get; set; }
        public Language? Language { get; set; }
        public int? PeopleCount { get; set; }
        public DateTime? SpanStart { get; set; }
        public DateTime? SpanEnd { get; set; }
        public RequestsFilterDto(Location? location, Language? language, int? peopleCount, DateTime? spanStart, DateTime? spanEnd)
        {
            Location = location;
            Language = language;
            PeopleCount = peopleCount;
            SpanStart = spanStart;
            SpanEnd = spanEnd;
        }
    }
}
