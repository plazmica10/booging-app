using BookingApp.Application.UseCases;
using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using System.Linq;

namespace BookingApp.Utilities
{
    public static class AccommodationReservationUtils
    {
        public static List<HomeDTO> FillHomeDto(List<AccommodationReservation> reservations, UserService _userService, AccommodationService _accommodationService, PictureService _pictureService)
        {
            var ret = new List<HomeDTO>();
            foreach (var reservation in reservations)
            {
                if (reservation.Status != ReservationStatus.Reserved && reservation.Status != ReservationStatus.InProgress && reservation.Status != ReservationStatus.PendingReview) continue;
                if (reservation.CheckIn > DateTime.Now.AddDays(3)) continue;
                var guest = _userService.GetById(reservation.GuestId);
                var accommodation = _accommodationService.GetById(reservation.AccommodationId);
                var dto = new HomeDTO
                {
                    ReservationId = reservation.Id, GuestName = $"{guest.FirstName} {guest.LastName}",
                    GuestPhoto = _pictureService.GetPictureLocation(guest.Photo), AccommodationName = accommodation.Name,
                    CheckIn = reservation.CheckIn, CheckOut = reservation.CheckOut,
                };
                if (reservation.Status == ReservationStatus.PendingReview) dto.ReservationKind = HomeDtoKind.PendingReview;
                else if (reservation.CheckOut <= DateTime.Now && reservation.Status == ReservationStatus.InProgress) dto.ReservationKind = HomeDtoKind.CheckingOut;
                else if (reservation.Status == ReservationStatus.InProgress) dto.ReservationKind = HomeDtoKind.CurrentlyHosting;
                else if (reservation.CheckIn <= DateTime.Now.AddDays(3)) dto.ReservationKind = HomeDtoKind.ArrivingSoon;
                ret.Add(dto);
            }
            return ret;
        }

        public static bool AreDatesAvailable(Accommodation accommodation, RescheduleRequest request)
        {
            var _accommodationReservationService = new AccommodationReservationService();
            List<AccommodationReservation> reservations = _accommodationReservationService.GetByAccommodationId(accommodation.Id);
            reservations.RemoveAll(r => r.Id == request.ReservationId);

            List<DateTime> reservedDates = AccommodationUtils.GetReservedDates(reservations);

            var askingDates = AccommodationUtils.GetAllDates(request.NewStartDate, request.NewEndDate);

            return askingDates.All(d => !reservedDates.Contains(d));
        }
    }
}
