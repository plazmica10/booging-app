using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Application.UseCases;
using BookingApp.DTO.Owner;

namespace BookingApp.Utilities
{
    public static class OwnerInboxUtils
    {
        public static RequestBigDTO FillSelectedRequest(InboxRequestDTO requestDto)
        {
            RescheduleRequestService rescheduleRequestService = new();
            AccommodationService accommodationService = new();
            AccommodationReservationService accommodationReservationService = new();
            UserService userService = new();
            PictureService pictureService = new();

            var request = rescheduleRequestService.GetById(requestDto.RequestId);
            var reservation = accommodationReservationService.GetById(request.ReservationId);
            var accommodation = accommodationService.GetById(reservation.AccommodationId);
            var guest = userService.GetById(request.UserId);

            return new RequestBigDTO { RequestId = request.Id, GuestName = guest.FirstName + " " + guest.LastName, OldCheckIn = reservation.CheckIn, OldCheckOut = reservation.CheckOut, NewCheckIn = request.NewStartDate, NewCheckOut = request.NewEndDate, DatesAvailable = AccommodationReservationUtils.AreDatesAvailable(accommodation, request), Status = request.Status, GuestPhoto = requestDto.UserPhoto, AccommodationId = accommodation.Id, AccommodationName = accommodation.Name, AccommodationLocation = $"{accommodation.Location.City}, {accommodation.Location.Country}", AccommodationPhoto = pictureService.GetPictureLocation(accommodation.Photos.First()) };
        }
    }
}
