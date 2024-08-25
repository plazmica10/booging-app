using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> TourRequestGetAll();
        public List<TourRequestComplex> TourRequestComplexGetAll();
        public TourRequest? TourRequestGetById(int id);
        public TourRequestComplex? TourRequestComplexGetById(int id);
        public TourRequest SaveAppend(TourRequest tourRequest);
        public TourRequestComplex SaveAppend(TourRequestComplex tourRequest);
        public bool Update(TourRequest tourRequest);
        public bool Update(TourRequestComplex tourRequest);
        public List<TourRequest> TourRequestGetAllByTouristId(int touristId);
        public List<TourRequest> TourRequestGetAllByTourRequestComplexId(int tourRequestComplexId);
        public List<TourRequestComplex> TourRequestComplexGetAllByTouristId(int touristId);
        public List<TourRequest> TourRequestGetAllWaiting();
    }
}
