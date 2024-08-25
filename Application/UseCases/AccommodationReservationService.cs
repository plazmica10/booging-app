using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    internal class AccommodationReservationService
    {
        private readonly IAccommodationRepository _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
        private readonly IAccommodationReservationRepository _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        private readonly IRescheduleRequestRepository _rescheduleRequestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();

        public void UpdateReservations()
        {
            List<AccommodationReservation> reservations = _reservationRepository.GetAll();
            DateTime today = DateTime.Now;

            foreach (var res in reservations)
            {
                UpdateForInProgress(res, today);
                UpdateForPendingReview(res, today);
                UpdateForCompleted(res, today);
            }
        }

        public ObservableCollection<AccommodationReservation> GetPendingReviewOwnerReservations(int ownerId)
        {
            List<AccommodationReservation> ownerReservations = GetAllReservationsForOwner(ownerId);
            return new ObservableCollection<AccommodationReservation>(ownerReservations.Where(r => r.Status == ReservationStatus.PendingReview));
        }

        public List<AccommodationReservation> GetAllReservationsForOwner(int ownerId)
        {
            List<Accommodation> ownerAccommodations = _accommodationRepository.GetByOwnerId(ownerId);
            return _reservationRepository.GetAll().Where(r => ownerAccommodations.Any(a => a.Id == r.AccommodationId)).ToList();
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _reservationRepository.GetByAccommodationId(accommodationId);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _reservationRepository.GetAll();
        }
        public List<AccommodationReservation> GetAll(int userId,bool past)
        {
            return _reservationRepository.GetForPeriodByUserId(userId,past);
        }

        public AccommodationReservation? GetById(int id)
        {
            return _reservationRepository.GetById(id);
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            return _reservationRepository.Save(accommodationReservation);
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            return _reservationRepository.Update(accommodationReservation);
        }

        public List<Tuple<DateTime, DateTime>> GetAvailableDates(int accommodationId, DateTime startDate,
            DateTime endDate, int stayLength)
        {
            return _reservationRepository.GetAvailableDates(accommodationId, startDate.Date, endDate.Date, stayLength);
        }

        public List<RescheduleRequest> GetRequestsByUserId(int userId)
        {
            return _rescheduleRequestRepository.GetByUserId(userId);
        }

        public RescheduleRequest SaveRequest(RescheduleRequest rescheduleRequest)
        {
            return _rescheduleRequestRepository.Save(rescheduleRequest);
        }

        private void UpdateForInProgress(AccommodationReservation res, DateTime today)
        {
            if (res.Status == ReservationStatus.Reserved)
            {
                if (res.CheckIn.Date <= today.Date)
                {
                    res.Status = ReservationStatus.InProgress;
                    _reservationRepository.Update(res);
                }
            }
        }

        private void UpdateForPendingReview(AccommodationReservation res, DateTime today)
        {
            if (res.Status == ReservationStatus.InProgress)
            {
                if (res.CheckOut.Date < today.Date)
                {
                    res.Status = ReservationStatus.PendingReview;
                    _reservationRepository.Update(res);
                }
            }
        }

        private void UpdateForCompleted(AccommodationReservation res, DateTime today)
        {
            if (res.Status == ReservationStatus.PendingReview)
            {
                if (res.CheckOut.AddDays(5).Date < today.Date)
                {
                    res.Status = ReservationStatus.Completed;
                    _reservationRepository.Update(res);
                }
            }
        }

    }
}