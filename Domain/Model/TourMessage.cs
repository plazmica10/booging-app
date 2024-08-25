using System;
using System.Globalization;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class TourMessage : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public int Index { get; set; } = -1;
        public string Message { get; set; } = "";
        public DateTime ReceivedAt { get; set; } = DateTime.Now;

        public string GetHeader() => "Id|TouristId|Index|Message|ReceivedAt";

        public TourMessage() { }

        public TourMessage(int touristId, string message, DateTime? receivedDateTime = null)
        {
            TouristId = touristId;
            Message = message;
            ReceivedAt = receivedDateTime ?? DateTime.Now;
        }

        public string[] ToCsv() => new[] { $"{Id}", $"{TouristId}", $"{Index}", $"{Message}", ReceivedAt.ToString("yyyy-MM-dd HH:mm:ss") };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TouristId = Convert.ToInt32(values[i++]);
            Index = Convert.ToInt32(values[i++]);
            Message = values[i++];
            ReceivedAt = DateTime.ParseExact(values[i++], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override string ToString() => $"{Id}|{TouristId}|{Index}|{Message}|{ReceivedAt:yyyy-MM-dd HH:mm:ss}";
    }
}