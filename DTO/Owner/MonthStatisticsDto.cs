namespace BookingApp.DTO.Owner
{
    public class MonthStatisticsDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = "";
        public int Reservations { get; set; }
        public int Cancellations { get; set; }
        public int ReschedulingCount { get; set; }
        public int RenovationCount { get; set; }
        public double Occupation { get; set; }
        public int DaysOccupied { get; set; }
    }
}
