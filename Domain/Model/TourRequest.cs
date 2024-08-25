using BookingApp.Domain.IRepository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;

namespace BookingApp.Domain.Model
{
    public enum TourRequestStatus { Waiting, Rejected, Approved }

    public class TourRequest : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public string Name { get; set; } = "";
        public Location Location { get; set; } = new();
        public string Description { get; set; } = "";
        public Language Language { get; set; } = new();
        public DateTime SpanStart { get; set; } = new();
        public DateTime SpanEnd { get; set; } = new();
        public TourRequestStatus Status { get; set; } = TourRequestStatus.Waiting;
        public List<TourRequestReservation> Reservations { get; set; } = new();
        public int TourRequestComplexId { get; set; } = -1;
        public TourRequest() { }

        public TourRequest(int touristId, string name, Location location, string description, Language language, DateTime spanStart, DateTime spanEnd, int tourRequestComplexId)
        {
            TouristId = touristId;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            SpanStart = spanStart;
            SpanEnd = spanEnd;
            TourRequestComplexId = tourRequestComplexId;
        }

        public string GetHeader() => "Id|TouristId|Name|Location|Description|Language|SpanStart|SpanEnd|Status|TourRequestComplexId";

        public string[] ToCsv() => new[] { $"{Id}", $"{TouristId}", $"{Name}", $"{Location}", $"{Description}", $"{Language}", $"{SpanStart:yyyy-MM-dd}", $"{SpanEnd:yyyy-MM-dd}", $"{Status}", $"{TourRequestComplexId}" };

        private readonly ITourRequestReservationRepository _iTourRequestReservationRepository = Injector.Injector.CreateInstance<ITourRequestReservationRepository>();

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TouristId = Convert.ToInt32(values[i++]);
            Name = values[i++];
            Location = Location.Parse(values[i++]);
            Description = values[i++];
            Language = Language.Parse(values[i++]);
            SpanStart = DateTime.ParseExact(values[i++], "yyyy-MM-dd", null);
            SpanEnd = DateTime.ParseExact(values[i++], "yyyy-MM-dd", null);
            Reservations = _iTourRequestReservationRepository.GetAllByTourRequestId(Id);
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[i++]);
            TourRequestComplexId = Convert.ToInt32(values[i++]);
        }

        public override string ToString() => $"{Id}|{TouristId}|{Name}|{Location}|{Description}|{Language}|{SpanStart:yyyy-MM-dd}|{SpanEnd:yyyy-MM-dd}|{Status}|{TourRequestComplexId}";
    }
}