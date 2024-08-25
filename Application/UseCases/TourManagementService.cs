using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guide;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Application.UseCases
{
    public class TourManagementService
    {
        private readonly ITourRepository            _iTourRepository            = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourReservationRepository _iTourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        private readonly ITourVoucherRepository     _iTourVoucherRepository     = Injector.Injector.CreateInstance<ITourVoucherRepository>();
        private readonly ITourRequestRepository     _iTourRequestRepository     = Injector.Injector.CreateInstance<ITourRequestRepository>();
        private readonly IUserRepository            _iUserRepository            =Injector.Injector.CreateInstance<IUserRepository>();
        private readonly ITourReviewRepository      _iTourReviewRepository      =Injector.Injector.CreateInstance<ITourReviewRepository>();

        private readonly TourMessageService _messageService = new();

        public TourServiceReturn CreateTour(CreateTourRequestDto tourRequestDto)
        {
            Tour newTour = new Tour(tourRequestDto.GuideId, tourRequestDto.Name, tourRequestDto.Location, tourRequestDto.Description, tourRequestDto.Language,
                tourRequestDto.Size, tourRequestDto.PeakPoints, tourRequestDto.Duration, tourRequestDto.PictureIds);

            foreach (DateTime date in tourRequestDto.DatesAndTimes)
            {
                newTour.StartsAt = date;
                newTour = _iTourRepository.SaveAppend(newTour);
            }

            NotifyTourists(newTour);

            return TourServiceReturn.Success;
        }

        private void NotifyTourists(Tour newTour)
        {
            HashSet<int> notifiedTourists = new HashSet<int>();

            foreach (TourRequest tourRequest in _iTourRequestRepository.TourRequestGetAll())
            {
                if (tourRequest.Status == TourRequestStatus.Approved) { continue; }
                if ((tourRequest.Language == newTour.Language || tourRequest.Location == newTour.Location)
                    && notifiedTourists.Add(tourRequest.TouristId)) { _messageService.SendMessageToTourist(tourRequest.TouristId, newTour); }
            }
        }

        public TourServiceReturn StartTour(int guideId, int id)
        {
            Tour? tour = _iTourRepository.GetById(id);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }
            if (tour.GuideId != guideId) { return TourServiceReturn.NotMyTour; }
            if (tour.Status != TourStatus.NotStarted) { return TourServiceReturn.FailedToStartTour; }

            if (_iTourRepository.GetAllByGuideId(guideId).Any(t => t.Status == TourStatus.Started)) { return TourServiceReturn.MyTourAlreadyStarted; }

            tour.Status = TourStatus.Started;
            tour.CurrentPeakPointIndex = 0;
            if (!_iTourRepository.Update(tour)) { return TourServiceReturn.FailedToUpdateTour; }
            return TourServiceReturn.Success;
        }

        public TourServiceReturn FinishTour(int guideId, int tourId)
        {
            Tour? tour = _iTourRepository.GetById(tourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }
            if (tour.GuideId != guideId) { return TourServiceReturn.NotMyTour; }
            tour.Status = TourStatus.Finished;

            if (!_iTourRepository.Update(tour)) { return TourServiceReturn.FailedToUpdateTour; }

            return TourServiceReturn.Success;
        }

        private void GiveVouchersForTour(int tourId, DateTime voucherExpiration)
        {
            List<int> touristIds = _iTourReservationRepository.GetAllByTourId(tourId).Select(tr => tr.TouristId).Distinct().ToList();
            foreach (int touristId in touristIds) { _iTourVoucherRepository.SaveAppend(new TourVoucher(touristId, voucherExpiration)); }
        }

        public TourServiceReturn CancelTour(int guideId, int tourId, DateTime voucherExpiration)
        {
            Tour? tour = _iTourRepository.GetById(tourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }
            if (tour.GuideId != guideId) { return TourServiceReturn.NotMyTour; }
            if (tour.Status != TourStatus.NotStarted) { return TourServiceReturn.FailedToCancelTour; }
            if ((tour.StartsAt - DateTime.Now).TotalHours < 48) { return TourServiceReturn.FailedToCancelTour48H; }

            tour.Status = TourStatus.Canceled;
            _iTourRepository.Update(tour);

            GiveVouchersForTour(tourId, voucherExpiration);

            return TourServiceReturn.Success;
        }

        public TourServiceReturn UpdateTourPeakPoint(int tourId, string peakPoint)
        {
            Tour? tour = _iTourRepository.GetById(tourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }

            tour.CurrentPeakPointIndex = tour.PeakPoints.IndexOf(peakPoint);
            if (!_iTourRepository.Update(tour)) { return TourServiceReturn.FailedToUpdateTour; }
            return TourServiceReturn.Success;
        }

        public void CheckTouristArrivalsForYearlyVoucher(int touristId)
        {
            List<TourReservation> reservationsInLastYear = new List<TourReservation>();
            foreach (TourReservation reservation in _iTourReservationRepository.GetAllByTouristId(touristId))
            {
                if (reservation.Arrived && reservation.ArrivedAt >= DateTime.Now.AddYears(-1) && !reservation.IsCounted)
                {
                    reservationsInLastYear.Add(reservation);
                }
            }

            HashSet<int> tourIds = new HashSet<int>();
            List<TourReservation> history = new List<TourReservation>();
            int vouchersToGive = 0;
            foreach (TourReservation reservation in reservationsInLastYear)
            {
                if (tourIds.Add(reservation.TourId))
                {
                    history.Add(reservation);

                    if (history.Count == 5)
                    {
                        foreach (TourReservation item in history)
                        {
                            item.IsCounted = true;
                            _iTourReservationRepository.Update(item);
                        }
                        history.Clear();
                        vouchersToGive++;
                    }
                }
            }

            for (int i = 0; i < vouchersToGive; i++)
            {
                _iTourVoucherRepository.SaveAppend(new TourVoucher(touristId, DateTime.Now.AddMonths(6)));
                _messageService.SendMessageToTourist(touristId, "Congratulations! You have earned a 6-month voucher that can be used for any tour.");
            }
        }

        public TourServiceReturn HasPersonArrived(int tourReservationId, string peakPoint)
        {
            TourReservation? tourReservation = _iTourReservationRepository.GetById(tourReservationId);
            if (tourReservation == null) { return TourServiceReturn.TourReservationNotFound; }
            Tour? tour = _iTourRepository.GetById(tourReservation.TourId);
            if (tour == null) { return TourServiceReturn.NotFoundTour; }

            tourReservation.Arrived = true;
            tourReservation.ArrivedPeakPoint = peakPoint;
            tourReservation.ArrivedAt = DateTime.Now;
            _iTourReservationRepository.Update(tourReservation);

            CheckTouristArrivalsForYearlyVoucher(tourReservation.TouristId);

            string message = tour.Name + " -> " + tourReservation.Person.FirstName + " " + tourReservation.Person.LastName + " arrived on: " + peakPoint;
            _messageService.SendMessageToTourist(tourReservation.TouristId, message);

            return TourServiceReturn.Success;
        }

        public TourServiceReturn ResignGuide(int guideId)
        {
            var tours = _iTourRepository.GetAllByGuideId(guideId);

            foreach (var tour in tours)
            {
                if (tour.Status == TourStatus.NotStarted || tour.Status == TourStatus.Started)
                {
                    tour.Status = TourStatus.Canceled;
                    _iTourRepository.Update(tour);

                    DateTime voucherExpiration = DateTime.Now.AddYears(2);
                    GiveVouchersForTour(tour.Id, voucherExpiration);
                }
            }

            return TourServiceReturn.Success;

        }

        public bool CheckForSuperGuideStatus(int guideId, int languageId)
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);

            var toursInLastYear = _iTourRepository.GetAllByGuideId(guideId)
                .Where(t => t.StartsAt >= oneYearAgo && t.Language.Id == languageId)
                .ToList();

            if (toursInLastYear.Count < 20) { return false; }

            var totalReviews = new List<TourReview>();

            foreach (var tour in toursInLastYear)
            {
                var reviews = _iTourReviewRepository.GetAllByTourId(tour.Id);
                totalReviews.AddRange(reviews);
            }

            double averageRating = totalReviews.Average(r => r.RatingGuideLanguageSkills);

            if (averageRating < 4.0) { return false; }

            return true;

        }

        public void RefreshSuperGuide(List<User>? guides, int languageId)
        {
            if (guides == null) { return; }
            foreach (var guide in guides)
            {
                if (guide.Role == UserRole.Guide)
                {
                    guide.IsSuperUser = CheckForSuperGuideStatus(guide.Id, languageId);
                    _iUserRepository.Update(guide);
                }
            }
        }
    }
}
