using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Tourist;
using System.Collections.Generic;

namespace BookingApp.Application.UseCases
{
    public class TourRequestComplexService
    {
        private readonly ITourRequestRepository _iTourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();

        private readonly TourRequestService _tourRequestService = new();
        
        public List<TourRequestComplex> GetAllByTouristId(int touristId)
        {
            return _iTourRequestRepository.TourRequestComplexGetAllByTouristId(touristId);
        }

        public TourRequestComplex MakeTourRequestComplex(TourRequestComplexDto tourRequestComplexDto)
        {
            TourRequestComplex tourRequestComplexSaved = _iTourRequestRepository.SaveAppend(new TourRequestComplex(
                tourRequestComplexDto.TouristId,
                tourRequestComplexDto.Name
            ));

            foreach (TourRequestDto tourRequestDto in tourRequestComplexDto.Parts)
            {
                TourRequest newPart = _tourRequestService.MakeTourRequest(tourRequestDto, tourRequestComplexSaved.Id);
                tourRequestComplexSaved.Parts.Add(newPart);
            }
            
            return tourRequestComplexSaved;
        }
    }
}
