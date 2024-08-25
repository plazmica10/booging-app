using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class RescheduleRequestRepository : IRescheduleRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/og_requests.csv";
        private readonly Serializer<RescheduleRequest> _serializer;
        private  List<RescheduleRequest> _rescheduleRequests;
        public RescheduleRequestRepository()
        {
            _serializer = new Serializer<RescheduleRequest>();
        }

        public List<RescheduleRequest> GetAll()
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            return _rescheduleRequests;
        }

        public RescheduleRequest GetById(int id)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            return _rescheduleRequests.Find(a => a.Id == id);
        }

        public List<RescheduleRequest> GetByUserId(int userId)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            return _rescheduleRequests.FindAll(a => a.UserId == userId);
        }

        public List<RescheduleRequest> GetByOwnerId(int ownerId)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            return _rescheduleRequests.FindAll(a => a.OwnerId == ownerId);
        }

        public List<RescheduleRequest> GetByAccommodationId(int accommodationId)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            return _rescheduleRequests.FindAll(a => a.AccommodationId == accommodationId);
        }

        public RescheduleRequest Save(RescheduleRequest rescheduleRequest)
        {
            rescheduleRequest.Id = NextId();
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            _rescheduleRequests.Add(rescheduleRequest);
            _serializer.ToCsv(FilePath, _rescheduleRequests);
            return rescheduleRequest;
        }

        public void Update(RescheduleRequest rescheduleRequest)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            var index = _rescheduleRequests.FindIndex(a => a.Id == rescheduleRequest.Id);
            _rescheduleRequests[index] = rescheduleRequest;
            _serializer.ToCsv(FilePath, _rescheduleRequests);
        }

        public int NextId()
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            if (_rescheduleRequests.Count < 1)
            {
                return 1;
            }
            return _rescheduleRequests.Max(a => a.Id) + 1;
        }

        public void DeleteByReservation(int reservationId)
        {
            _rescheduleRequests = _serializer.FromCsv(FilePath);
            _rescheduleRequests.RemoveAll(a => a.ReservationId == reservationId);
            _serializer.ToCsv(FilePath, _rescheduleRequests);
        }
    }
}
