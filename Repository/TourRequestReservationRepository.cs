using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourRequestReservationRepository : ITourRequestReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_requests_reservations.csv";
        private readonly Serializer<TourRequestReservation> _serializer = new();

        public TourRequestReservationRepository() { }

        public List<TourRequestReservation> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public TourRequestReservation? GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(t => t.Id == id);
        }

        public List<TourRequestReservation> GetAllByTourRequestId(int tourRequestId)
        {
            return _serializer.FromCsv(FilePath).FindAll(t => t.TourRequestId == tourRequestId);
        }

        public TourRequestReservation SaveAppend(TourRequestReservation tourRequestReservation)
        {
            List<TourRequestReservation> tourRequestReservations = _serializer.FromCsv(FilePath);
            tourRequestReservation.Id = tourRequestReservations.Count < 1 ? 1 : tourRequestReservations.Max(t => t.Id) + 1;
            tourRequestReservations.Add(tourRequestReservation);
            _serializer.ToCsv(FilePath, tourRequestReservations);
            return tourRequestReservation;
        }
    }
}
