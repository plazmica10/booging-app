using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_requests.csv";
        private const string FilePathComplex = "../../../Resources/Data/gt_requests_complex.csv";

        private readonly Serializer<TourRequest> _serializer = new();
        private readonly Serializer<TourRequestComplex> _serializerComplex = new();

        public TourRequestRepository() { }

        private void TourRequestUpdate()
        {
            List<TourRequest> requests = _serializer.FromCsv(FilePath);
            foreach (TourRequest request in requests)
            {
                if (request.Status == TourRequestStatus.Waiting && (request.SpanStart - DateTime.Now).TotalHours < 48)
                {
                    request.Status = TourRequestStatus.Rejected;
                }
            }
            _serializer.ToCsv(FilePath, requests);
        }

        private void TourRequestComplexUpdate()
        {
            List<TourRequestComplex> requests = _serializerComplex.FromCsv(FilePathComplex);
            foreach (TourRequestComplex request in requests)
            {
                foreach (TourRequest part in request.Parts)
                {
                    if (part.Status == TourRequestStatus.Rejected)
                    {
                        request.Status = TourRequestStatus.Rejected;
                        break;
                    }
                }
            }
            _serializerComplex.ToCsv(FilePathComplex, requests);
        }

        public List<TourRequest> TourRequestGetAll()
        {
            TourRequestUpdate();
            return _serializer.FromCsv(FilePath);
        }

        public List<TourRequestComplex> TourRequestComplexGetAll()
        {
            TourRequestUpdate();
            TourRequestComplexUpdate();
            return _serializerComplex.FromCsv(FilePathComplex);
        }

        public TourRequest? TourRequestGetById(int id)
        {
            return TourRequestGetAll().Find(t => t.Id == id);
        }

        public TourRequestComplex? TourRequestComplexGetById(int id)
        {
            return TourRequestComplexGetAll().Find(t => t.Id == id);
        }

        public TourRequest SaveAppend(TourRequest tourRequest)
        {
            List<TourRequest> tourRequests = _serializer.FromCsv(FilePath);
            tourRequest.Id = tourRequests.Count < 1 ? 1 : tourRequests.Max(t => t.Id) + 1;
            tourRequests.Add(tourRequest);
            _serializer.ToCsv(FilePath, tourRequests);
            return tourRequest;
        }

        public TourRequestComplex SaveAppend(TourRequestComplex tourRequest)
        {
            List<TourRequestComplex> tourRequests = _serializerComplex.FromCsv(FilePathComplex);
            tourRequest.Id = tourRequests.Count < 1 ? 1 : tourRequests.Max(t => t.Id) + 1;
            tourRequests.Add(tourRequest);
            _serializerComplex.ToCsv(FilePathComplex, tourRequests);
            return tourRequest;
        }

        public bool Update(TourRequest tourRequest)
        {
            List<TourRequest> tourRequests = _serializer.FromCsv(FilePath);
            TourRequest? current = tourRequests.Find(t => t.Id == tourRequest.Id);
            if (current == null) { return false; }
            int index = tourRequests.IndexOf(current);
            tourRequests.Remove(current);
            tourRequests.Insert(index, tourRequest);
            return _serializer.ToCsv(FilePath, tourRequests);
        }

        public bool Update(TourRequestComplex tourRequest)
        {
            List<TourRequestComplex> tourRequests = _serializerComplex.FromCsv(FilePathComplex);
            TourRequestComplex? current = tourRequests.Find(t => t.Id == tourRequest.Id);
            if (current == null) { return false; }
            int index = tourRequests.IndexOf(current);
            tourRequests.Remove(current);
            tourRequests.Insert(index, tourRequest);
            return _serializerComplex.ToCsv(FilePathComplex, tourRequests);
        }

        public List<TourRequest> TourRequestGetAllByTouristId(int touristId) // tourist call
        {
            return TourRequestGetAll().FindAll(t => t.TouristId == touristId && t.TourRequestComplexId == -1);
        }

        public List<TourRequestComplex> TourRequestComplexGetAllByTouristId(int touristId) // tourist call
        {
            return TourRequestComplexGetAll().FindAll(t => t.TouristId == touristId);
        }

        public List<TourRequest> TourRequestGetAllByTourRequestComplexId(int tourRequestComplexId) // model call
        {
            return TourRequestGetAll().FindAll(t => t.TourRequestComplexId == tourRequestComplexId);
        }
        
        public List<TourRequest> TourRequestGetAllWaiting() // guide call
        {
            return TourRequestGetAll().FindAll(t => t.Status == TourRequestStatus.Waiting);
        }
    }
}
