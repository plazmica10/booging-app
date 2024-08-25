using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_reservations.csv";
        private readonly Serializer<TourReservation> _serializer = new();

        public TourReservationRepository() { }

        public List<TourReservation> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public TourReservation? GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(t => t.Id == id);
        }

        public TourReservation SaveAppend(TourReservation tourReservation)
        {
            List<TourReservation> tourReservations = _serializer.FromCsv(FilePath);
            tourReservation.Id = tourReservations.Count < 1 ? 1 : tourReservations.Max(t => t.Id) + 1;
            tourReservations.Add(tourReservation);
            _serializer.ToCsv(FilePath, tourReservations);
            return tourReservation;
        }

        public bool Update(TourReservation tourReservation)
        {
            List<TourReservation> tourReservations = _serializer.FromCsv(FilePath);
            TourReservation? current = tourReservations.Find(c => c.Id == tourReservation.Id);
            if (current == null) { return false; }
            int index = tourReservations.IndexOf(current);
            tourReservations.Remove(current);
            tourReservations.Insert(index, tourReservation);
            return _serializer.ToCsv(FilePath, tourReservations);
        }

        public List<TourReservation> GetAllByTourId(int tourId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.TourId == tourId);
        }

        public List<TourReservation> GetAllByTouristId(int touristId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.TouristId == touristId);
        }
        public List<TourReservation> GetAllByTourIdArrived(int tourId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.TourId == tourId && c.Arrived);
        }
    }
}
