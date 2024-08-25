namespace BookingApp.Application.UseCases
{
    public enum TourServiceReturn
    {
        Success,
        NotFoundTour,
        NotFoundTourRequest,
        NotEnoughSpace,
        TooManyPeople,
        NotMyTour,
        FailedToApproveBusyForTourRequest,
        FailedToStartTour,
        MyTourAlreadyStarted,
        FailedToUpdateTour,
        TourReservationNotFound,
        FailedToCancelTour,
        FailedToCancelTour48H,
        TourReviewNotFound,
        InvalidLocationId,
        InvalidLanguageId,
        InvalidTourRequestTime,
        UnknownFailure
    }
}
