using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Application.UseCases
{
    public class TourInfoService
    {
        private readonly ITourRepository        _iTourRepository        = Injector.Injector.CreateInstance<ITourRepository>();
        private readonly ITourVoucherRepository _iTourVoucherRepository = Injector.Injector.CreateInstance<ITourVoucherRepository>();
        private readonly ITourMessageRepository _iTourMessageRepository = Injector.Injector.CreateInstance<ITourMessageRepository>();

        public List<string> GetTourPeakPoints(int tourId)
        {
            Tour? tour = _iTourRepository.GetById(tourId);
            if (tour == null) { return new List<string>(); }
            return tour.PeakPoints;
        }

        public List<TourVoucher> GetTouristVouchers(int touristId)
        {
            return _iTourVoucherRepository.GetAllByTouristId(touristId);
        }

        public List<TourMessage> GetTouristMessagesDescending(int touristId)
        {
            return _iTourMessageRepository.GetAllByTouristId(touristId).OrderByDescending(message => message.Index).ToList();
        }
    }
}
