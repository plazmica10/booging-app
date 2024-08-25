namespace BookingApp.DTO.Guide
{
    public class TouristsAgeStatisticsDto
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public int NumTourist { get; set; }
        public int Under18 { get; set; }
        public int Between18And50 { get; set; }
        public int Above50 { get; set; }

        public TouristsAgeStatisticsDto(int id, string tourName, int numTourist, int under18, int between18And50, int above50)
        {
            Id = id;
            TourName = tourName;
            NumTourist = numTourist;
            Under18 = under18;
            Between18And50 = between18And50;
            Above50 = above50;
        }
    }
}