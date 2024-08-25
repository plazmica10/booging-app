using BookingApp.Domain.IRepository;
using BookingApp.Repository;
using System;
using System.Collections.Generic;

namespace BookingApp.Injector
{
    public class Injector
    {
        private static readonly Dictionary<Type, Func<object>> Implementations = new()
        {
            [typeof(IUserRepository)] = () => new UserRepository(),
            [typeof(ILocationRepository)] = () => new LocationRepository(),
            [typeof(ILanguageRepository)] = () => new LanguageRepository(),
            [typeof(IPictureRepository)] = () => new PictureRepository(),

            [typeof(IAccommodationRepository)] = () => new AccommodationRepository(),
            [typeof(IAccommodationReservationRepository)] = () => new AccommodationReservationRepository(),
            [typeof(IRescheduleRequestRepository)] = () => new RescheduleRequestRepository(),
            [typeof(IOwnerReviewRepository)] = () => new OwnerReviewRepository(),
            [typeof(IGuestReviewRepository)] = () => new GuestReviewRepository(),
            [typeof(IRenovationRecommendationRepository)] = () => new RenovationRecommendationRepository(),
            [typeof(IAccommodationRenovationRepository)] = () => new AccommodationRenovationRepository(),
            [typeof(IForumRepository)] = () => new ForumRepository(),
            [typeof(IForumCommentRepository)] = () => new ForumCommentRepository(),

            [typeof(ITourRepository)] = () => new TourRepository(),
            [typeof(ITourReservationRepository)] = () => new TourReservationRepository(),
            [typeof(ITourVoucherRepository)] = () => new TourVoucherRepository(),
            [typeof(ITourMessageRepository)] = () => new TourMessageRepository(),
            [typeof(ITourReviewRepository)] = () => new TourReviewRepository(),
            [typeof(ITourRequestRepository)] = () => new TourRequestRepository(),
            [typeof(ITourRequestReservationRepository)] = () => new TourRequestReservationRepository()
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);
            if (Implementations.TryGetValue(type, out Func<object>? factory))
            {
                return (T)factory.Invoke();
            }
            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
