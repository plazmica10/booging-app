using System;
using System.Collections.Generic;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    public class AccommodationRenovationService
    {
        private readonly IAccommodationRenovationRepository _renovationRepository = Injector.Injector.CreateInstance<IAccommodationRenovationRepository>();
        private readonly IAccommodationReservationRepository _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();

        public AccommodationRenovation GetById(int id)
        {
            return _renovationRepository.GetById(id);
        }

        public List<AccommodationRenovation> GetByUserId(int userId)
        {
            return _renovationRepository.GetByUserId(userId);
        }

        public void SaveRenovation(AccommodationRenovation renovation)
        {
            _renovationRepository.Save(renovation);
        }

        public void UpdateRenovation(AccommodationRenovation renovation)
        {
            _renovationRepository.Update(renovation);
        }

        public void UpdateRenovationsStatus()
        {
            var renovations = _renovationRepository.GetAll();
            DateTime today = DateTime.Now;

            foreach (var renovation in renovations)
            {
                UpdateForInProgress(renovation, today);
                UpdateForCompleted(renovation, today);
            }
        }

        private void UpdateForInProgress(AccommodationRenovation renovation, DateTime today)
        {
            if (renovation.Status == RenovationStatus.Pending && renovation.StartDate.Date <= today.Date)
            {
                renovation.Status = RenovationStatus.InProgress;
                _renovationRepository.Update(renovation);
            }
        }

        private void UpdateForCompleted(AccommodationRenovation renovation, DateTime today)
        {
            if (renovation.Status == RenovationStatus.InProgress && renovation.EndDate.Date < today.Date)
            {
                renovation.Status = RenovationStatus.Completed;
                _renovationRepository.Update(renovation);
                renovation.Reservation.Status = ReservationStatus.RenovationOver;
                _reservationRepository.Update(renovation.Reservation);
            }
        }
    }
}
