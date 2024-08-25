using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.DTO.Tourist;
using System;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class TourRequestService
    {
        private readonly ITourRequestRepository            _iTourRequestRepository            = Injector.Injector.CreateInstance<ITourRequestRepository>();
        private readonly ITourRequestReservationRepository _iTourRequestReservationRepository = Injector.Injector.CreateInstance<ITourRequestReservationRepository>();
        private readonly ITourRepository                   _iTourRepository                   = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReservationRepository        _iTourReservationRepository        = Injector.Injector.CreateInstance<ITourReservationRepository>();

        private readonly TourMessageService _messageService = new();

        public List<TourRequest> GetAllByTouristId(int touristId)
        {
            return _iTourRequestRepository.TourRequestGetAllByTouristId(touristId);
        }

        public TourRequest MakeTourRequest(TourRequestDto tourRequestDto, int tourRequestComplexId)
        {
            TourRequest tourRequestSaved = _iTourRequestRepository.SaveAppend(new TourRequest(
                tourRequestDto.TouristId,
                tourRequestDto.Name,
                tourRequestDto.Location,
                tourRequestDto.Description,
                tourRequestDto.Language,
                tourRequestDto.SpanStart,
                tourRequestDto.SpanEnd,
                tourRequestComplexId
            ));

            foreach (ReservationDto reservation in tourRequestDto.Reservations)
            {
                TourRequestReservation tourRequestReservationSaved = _iTourRequestReservationRepository.SaveAppend(
                    new TourRequestReservation(
                        tourRequestSaved.Id,
                        reservation.FirstName,
                        reservation.LastName,
                        reservation.Email,
                        reservation.BirthYear
                    )
                );

                tourRequestSaved.Reservations.Add(tourRequestReservationSaved);
            }

            return tourRequestSaved;
        }

        public TourRequest? GetById(int id)
        {
            return _iTourRequestRepository.TourRequestGetById(id);
        }

        public List<TourRequest> GetAll()
        {
            return _iTourRequestRepository.TourRequestGetAll();
        }

        public List<TourRequest> GetAllWaiting()
        {
            return _iTourRequestRepository.TourRequestGetAllWaiting();
        }

        public List<TourRequest> GetAllWaitingFiltered(RequestsFilterDto filter)
        {
            List<TourRequest> filteredRequests = new List<TourRequest>();
            foreach (TourRequest tourRequest in _iTourRequestRepository.TourRequestGetAllWaiting())
            {
                if (filter.Location != null && filter.Location != tourRequest.Location) { continue; }
                if (filter.Language != null && filter.Language != tourRequest.Language) { continue; }
                if (filter.PeopleCount != 0 && filter.PeopleCount != tourRequest.Reservations.Count) { continue; }
                if (filter.SpanStart != null && filter.SpanEnd != null && !(filter.SpanStart <= tourRequest.SpanStart && filter.SpanEnd >= tourRequest.SpanEnd)) { continue; }
                filteredRequests.Add(tourRequest);
            }

            return filteredRequests;
        }
        public void Update(TourRequest tourRequest)
        {
            _iTourRequestRepository.Update(tourRequest);
        }

        public TourServiceReturn ApproveTourRequest(int guideId, int requestId, DateTime startsAt, int duration)
        {
            TourRequest? tourRequest = _iTourRequestRepository.TourRequestGetById(requestId);
            if (tourRequest == null) { return TourServiceReturn.NotFoundTourRequest; }
            if (!(tourRequest.SpanStart <= startsAt && tourRequest.SpanEnd >= startsAt)) { return TourServiceReturn.InvalidTourRequestTime; }

            foreach (Tour tour in _iTourRepository.GetAllByGuideId(guideId)) // check overlapping
            {
                if (tour.Status == TourStatus.Finished) { continue; } 
                if (tour.Status == TourStatus.Canceled) { continue; } 
                DateTime tourStart = tour.StartsAt;
                DateTime tourEnd = tour.StartsAt.AddHours(tour.Duration);
                DateTime reqStart = startsAt;
                DateTime reqEnd = startsAt.AddHours(duration);
                if (tourStart < reqEnd && reqStart < tourEnd) { return TourServiceReturn.FailedToApproveBusyForTourRequest; }
            }

            tourRequest.Status = TourRequestStatus.Approved; // accept request
            _iTourRequestRepository.Update(tourRequest);

            Tour newTour = new(guideId, tourRequest.Name, tourRequest.Location, tourRequest.Description, tourRequest.Language, tourRequest.Reservations.Count, startsAt, duration);
            newTour = _iTourRepository.SaveAppend(newTour); // create tour
            _messageService.SendMessageToTourist(tourRequest.TouristId, newTour); // notify tourist

            foreach (TourRequestReservation tourRequestReservation in _iTourRequestReservationRepository.GetAllByTourRequestId(tourRequest.Id))
            {
                _iTourReservationRepository.SaveAppend(new TourReservation(newTour.Id, tourRequest.TouristId,
                    tourRequestReservation.Person.FirstName, tourRequestReservation.Person.LastName, tourRequestReservation.Person.Email, tourRequestReservation.Person.BirthYear));
            }

            return TourServiceReturn.Success;
        }
    }
}
