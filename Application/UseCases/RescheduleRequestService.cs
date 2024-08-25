using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;

namespace BookingApp.Application.UseCases
{
    public class RescheduleRequestService
    {
        private readonly IRescheduleRequestRepository _rescheduleRequestRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IAccommodationReservationRepository _reservationRepository;

        public RescheduleRequestService()
        {
            _rescheduleRequestRepository = Injector.Injector.CreateInstance<IRescheduleRequestRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _pictureRepository = Injector.Injector.CreateInstance<IPictureRepository>();    
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }
        public List<RescheduleRequest> GetByOwnerId(int ownerId)
        {
            return _rescheduleRequestRepository.GetByOwnerId(ownerId);
        }
        public RescheduleRequest GetById(int id)
        {
            return _rescheduleRequestRepository.GetById(id);
        }
        public void Update(RescheduleRequest request)
        {
            _rescheduleRequestRepository.Update(request);
        }
        public void Delete(int reservationId)
        {
            _rescheduleRequestRepository.DeleteByReservation(reservationId);
        }
        public RescheduleRequestsPageDto GetRequestDto(int accommodationId,DateTime checkIn,DateTime checkOut)
        {
            var accommodation = _accommodationRepository.GetById(accommodationId);
            List<string> images = _pictureRepository.GetPictureLocations(accommodation.Photos);
            var rescheduleDTO = new RescheduleRequestsPageDto(accommodation.Name, accommodation.City, accommodation.Country,
                checkIn, checkOut, images);
            return rescheduleDTO;
        }

        public List<RescheduleRequestsListDto> GetRequestsShortDtos(int userId)
        {
            var requests = _rescheduleRequestRepository.GetByUserId(userId);
            List<RescheduleRequestsListDto> dtos = new();
            foreach (var request in requests)
            {
                var accommodation = _accommodationRepository.GetById(request.AccommodationId);
                var reservation = _reservationRepository.GetById(request.ReservationId);
                var dto = new RescheduleRequestsListDto(request.Id,accommodation.Name,reservation.GuestCount, reservation.CheckIn, reservation.CheckOut, request.NewStartDate, request.NewStartDate,request.Status.ToString());
                dtos.Add(dto);
            }
            return dtos;
        }
    }
}
