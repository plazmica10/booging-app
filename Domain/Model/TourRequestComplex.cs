using BookingApp.Application.UseCases;
using BookingApp.Domain.IRepository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;

namespace BookingApp.Domain.Model
{
    public class TourRequestComplex : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public string Name { get; set; } = "";

        public List<TourRequest> Parts = new();
        public TourRequestStatus Status { get; set; } = TourRequestStatus.Waiting;
        public TourRequestComplex() { }
        public TourRequestComplex(int touristId, string name) { TouristId = touristId; Name = name; }
        public string GetHeader() => "Id|TouristId|Name|Parts|Status";

        private readonly ITourRequestRepository _iTourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();

        public string[] ToCsv() => new[] { $"{Id}", $"{TouristId}", $"{Name}", $"{Parts.Count}", $"{Status}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            TouristId = Convert.ToInt32(values[i++]);
            Name = values[i++];
            Parts = _iTourRequestRepository.TourRequestGetAllByTourRequestComplexId(Id); i++;
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[i++]);
        }

        public override string ToString() => $"{Id}|{TouristId}|{Name}|{Parts.Count}|{Status}";
    }
}