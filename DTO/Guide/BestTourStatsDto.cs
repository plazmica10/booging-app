using BookingApp.Domain.Model;

namespace BookingApp.DTO.Guide
{
    public class BestTourStatsDto
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; }
        public int PeopleArrived { get; set; }

        public BestTourStatsDto(string name, Location location, Language language, string description, int peopleArrived)
        {
            Name = name;
            Location = location;
            Language = language;
            Description = description;
            PeopleArrived = peopleArrived;
        }
    }
}