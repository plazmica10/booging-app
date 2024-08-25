using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;

namespace BookingApp.Application.UseCases
{
    public class AccommodationService
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserRepository _userRepository;
        public AccommodationService()
        {
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _pictureRepository = Injector.Injector.CreateInstance<IPictureRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll();
        }

        public Accommodation? GetById(int id)
        {
            return _accommodationRepository.GetById(id);
        }

        public List<Accommodation> GetByOwnerId(int ownerId)
        {
            return _accommodationRepository.GetByOwnerId(ownerId);
        }

        public List<Accommodation> SearchByAllParameters(string name, string location, AccommodationType? type,
            int guestCount, int stayLength)
        {
            return _accommodationRepository.SearchByAllParameters(name, location, type, guestCount, stayLength);
        }

        public Accommodation Save(Accommodation accommodation)
        {
            return _accommodationRepository.Save(accommodation);
        }

        public Accommodation GetByName(string name)
        {
            return _accommodationRepository.GetAll().FirstOrDefault(a => a.Name == name);
        }
        public List<ReservationsDto> GetReservationsDto(int userId,bool past)
        {
            List<ReservationsDto> reservationsViewDTOs = new List<ReservationsDto>();
            var reservations = _reservationRepository.GetForPeriodByUserId(userId,past);
            foreach (var reservation in reservations)
            {
                var accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
                List<string> images = _pictureRepository.GetPictureLocations(accommodation.Photos);
                User owner = _userRepository.GetById(accommodation.OwnerId);
                reservationsViewDTOs.Add(new ReservationsDto(reservation.Id,accommodation.Name,accommodation.City,accommodation.Country,reservation.CheckIn,reservation.CheckOut,images,accommodation.Id,owner,reservation.GuestCount,reservation.HasGuestReviewed));
            }
            return reservationsViewDTOs;
        }
        public List<AccommodationsSearchDto> GetAllAvailableAccommodations(DateTime checkIn, DateTime checkOut, int guestCount, int stayLength)
        {
            if (!AccommodationUtils.AreDatesValid(checkIn, checkOut)) return new List<AccommodationsSearchDto>();
            List<AccommodationsSearchDto> accommodations = new List<AccommodationsSearchDto>();
            List<Accommodation> allAccommodations = _accommodationRepository.GetAll();
            foreach (var accommodation in allAccommodations)
            {
                if(accommodation.MaxGuests < guestCount || accommodation.MinDays > stayLength) continue;
                var availableDates = _reservationRepository.GetAvailableDates(accommodation.Id, checkIn, checkOut, stayLength);
                if (availableDates.Count > 0)
                {
                    List<string> images = _pictureRepository.GetPictureLocations(accommodation.Photos);
                    foreach (var date in availableDates) accommodations.Add(new AccommodationsSearchDto(accommodation.Id,accommodation.Name, accommodation.City, accommodation.Country, date.Item1, date.Item2, images[0]));
                }
            }
            return accommodations;
        }

        public List<int> GetOwnerIdsForLocation(string city, string country)
        {
            List<int> owners = new List<int>();
            List<Accommodation> accommodations = _accommodationRepository.GetByLocation(city, country);
            foreach (var accommodation in accommodations)
            {
                if (!owners.Contains(accommodation.OwnerId)) owners.Add(accommodation.OwnerId);
            }
            return owners;
        }

        public List<int> GetGuestIdsForLocation(string city, string country)
        {
            List<int> guests = new List<int>();
            List<AccommodationReservation> reservations = _reservationRepository.GetAll();
            foreach (var reservation in reservations)
            {
                Accommodation accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
                if (accommodation.City == city && accommodation.Country == country)
                {
                    if (!guests.Contains(reservation.GuestId)) guests.Add(reservation.GuestId);
                }
            }
            return guests;
        }

        public List<Accommodation> GetAtLocationForOwner(int ownerId, Location location)
        {
            var accommodations = GetByOwnerId(ownerId);
            return accommodations.Where(a => a.Location.City == location.City && a.Location.Country == location.Country).ToList();
        }

        public void CloseAccommodation(int accommodationId)
        {
            Accommodation accommodation = _accommodationRepository.GetById(accommodationId);
            accommodation.IsDeleted = true;
            _accommodationRepository.Update(accommodation);
            var reservations = _reservationRepository.GetByAccommodationId(accommodationId);
            foreach (var reservation in reservations)
            {
                if (reservation.Status == ReservationStatus.Reserved || reservation.Status == ReservationStatus.InProgress || reservation.Status == ReservationStatus.Renovation)
                    reservation.Status = ReservationStatus.Canceled;
                _reservationRepository.Update(reservation);
            }
        }

        public void Update(Accommodation accommodation)
        {
            _accommodationRepository.Update(accommodation);
        }
    }
}
