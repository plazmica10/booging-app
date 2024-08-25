using System;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using System.Collections.Generic;
using BookingApp.DTO.Guest;

namespace BookingApp.Application.UseCases
{
    internal class GuestReviewService
    {
        private readonly IGuestReviewRepository _guestReviewRepository;
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private  readonly  IUserRepository _userRepository;

        public GuestReviewService()
        {
            _guestReviewRepository = Injector.Injector.CreateInstance<IGuestReviewRepository>();
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }
        public void AddGuestReview(GuestReview review)
        {
            _guestReviewRepository.Save(review);
        }

        public GuestReview GetById(int id)
        {
            return _guestReviewRepository.GetById(id);
        }
        public List<GuestReview> GetAllViewableForGuest(int guestId)
        {
            List<GuestReview> reviews = _guestReviewRepository.GetAllForGuest(guestId);
            List<GuestReview> outList = new List<GuestReview>();
            foreach (var review in reviews)
            { 
                var reservation = _reservationRepository.GetById(review.ReservationId);
                if (reservation.HasGuestReviewed)
                    outList.Add(review);
            }
            return outList;
        }
        public double GetAverageRating(int guestId)
        {
            var reviews = _guestReviewRepository.GetAllForGuest(guestId);
            if (reviews.Count == 0)
                return 0;
            double sum = 0;
            foreach (var review in reviews) { sum += review.Cleanliness + review.RuleFollowing; }
            return Math.Round(sum / (reviews.Count * 2),1);
        }
        public List<ReviewsListDto> GetReviewsDto(int guestId)
        {
            List<GuestReview> reviews = GetAllViewableForGuest(guestId);
            List<ReviewsListDto> outList = new List<ReviewsListDto>();
            foreach (var review in reviews)
            {
                var reservation = _reservationRepository.GetById(review.ReservationId);
                var accommodation = _accommodationRepository.GetById(reservation.AccommodationId);
                var owner =  _userRepository.GetById(accommodation.OwnerId);
                outList.Add(new ReviewsListDto(review.Id,owner.FirstName + " " + owner.LastName, accommodation.Name, accommodation.City, accommodation.Country, reservation.CheckIn, reservation.CheckOut, review.GetRating()));
            }
            return outList;
        }
    }
}
