using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class TourReservationService
    {
        private readonly ITourRepository            _iTourRepository            = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReservationRepository _iTourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        private readonly ITourVoucherRepository     _iTourVoucherRepository     = Injector.Injector.CreateInstance<ITourVoucherRepository>();

        private void MakeReservation(Tour tour, ReservationRequestDto request)
        {
            foreach (ReservationDto reservation in request.Reservations)
            {
                _iTourReservationRepository.SaveAppend(new TourReservation(tour.Id, request.TouristId, reservation.FirstName, reservation.LastName, reservation.Email, reservation.BirthYear));
                tour.SpaceLeft--;
            }

            _iTourRepository.Update(tour);
        }

        private void UseVoucher(ReservationRequestDto request)
        {
            if (request.VoucherId == -1) { return; }
            TourVoucher? voucher = _iTourVoucherRepository.GetById(request.VoucherId);
            if (voucher != null) { _iTourVoucherRepository.Delete(voucher); }
        }

        public TourServiceReturn ReserveTour(ReservationRequestDto request)
        {
            Tour? tour = _iTourRepository.GetById(request.TourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }
            if (tour.SpaceLeft == 0) { return TourServiceReturn.NotEnoughSpace; }
            if (request.Reservations.Count > tour.SpaceLeft) return TourServiceReturn.TooManyPeople;

            MakeReservation(tour, request);
            UseVoucher(request);

            return TourServiceReturn.Success;
        }

        public List<TourReservation> GetTourReservationsByTourId(int tourId)
        {
            return _iTourReservationRepository.GetAllByTourId(tourId);
        }
    }
}
