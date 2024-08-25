using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using BookingApp.DTO.Guest;

namespace BookingApp.Application.UseCases
{
    public class OwnerReviewService
    {
        private readonly IOwnerReviewRepository _ownerReviewRepository;
        private readonly AccommodationService _accommodationService = new AccommodationService();
        private readonly IAccommodationReservationRepository _reservationRepository;
        private readonly IRenovationRecommendationRepository _renovationRecommendationRepository;
        private readonly PictureService _pictureService = new PictureService();
        private readonly UserService _userService = new UserService();
        private  readonly RescheduleRequestService _rescheduleRequestService = new RescheduleRequestService();

        public OwnerReviewService()
        {
            _ownerReviewRepository = Injector.Injector.CreateInstance<IOwnerReviewRepository>();
            _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            _renovationRecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
        }

        public int CountByOwnerId(int ownerId)
        {
            return _ownerReviewRepository.CountByOwnerId(ownerId);
        }

        public double AverageRatingByOwnerId(int ownerId)
        {
            return _ownerReviewRepository.AverageRatingByOwnerId(ownerId);
        }

        public List<int> GetRatingsForOwner(int ownerId)
        {
            var reviews = _ownerReviewRepository.GetAllForOwner(ownerId);
            List<int> ratings = new();
            foreach (var review in reviews)
            {
                ratings.Add(review.OwnerRating);
            }

            return ratings;
        }

        public List<OwnerReview> GetAllForOwner(int ownerId)
        {
            return _ownerReviewRepository.GetAllForOwner(ownerId);
        }

        public List<OwnerReview> GetAllViewableForOwner(int ownerId)
        {
            List<OwnerReview> reviews = _ownerReviewRepository.GetAllForOwner(ownerId);
            List<OwnerReview> outList = new List<OwnerReview>();
            foreach (var review in reviews)
            {
                var reservation = _reservationRepository.GetById(review.ReservationId);
                if (reservation.Status == ReservationStatus.Reviewed)
                    outList.Add(review);
            }
            return outList;
        }
        public OwnerReview Save(OwnerReview ownerReview)
        {
            return _ownerReviewRepository.Save(ownerReview);
        }
        public OwnerReview GetByReservationId(int reservationId)
        {
            return _ownerReviewRepository.GetOwnerReview(reservationId);
        }

        public RenovationRecommendation SaveRenovationRecommendation(RenovationRecommendation renovation)
        {
           return _renovationRecommendationRepository.Save(renovation);
        }

        public RequestDto GetRequest(RescheduleRequestsListDto req)
        {
            var accom = _accommodationService.GetByName(req.AccommodationName);
            List<string> images = _pictureService.GetPictureLocations(accom.Photos);
            var owner = _userService.GetById(accom.OwnerId);
            var request = _rescheduleRequestService.GetById(req.RequestId);
            return new RequestDto()
            {
                AccommodationName = req.AccommodationName,
                Owner  = owner.FirstName + " " + owner.LastName,
                City = accom.City,
                Country = accom.Country,
                GuestCount = req.GuestCount,
                OldStartDate = req.CheckIn,
                OldEndDate = req.CheckOut,
                NewStartDate = req.NewCheckIn,
                NewEndDate = req.NewCheckOut,
                Status = req.Status,
                Images = images,
                OwnersComment = request.OwnersComment
            };
        }
    }
}