using System.Collections.Generic;
using BookingApp.Domain.IRepository;

namespace BookingApp.Application.UseCases
{
    public class PictureService
    {
        private readonly IPictureRepository iPictureRepository;

        public PictureService() { iPictureRepository = Injector.Injector.CreateInstance<IPictureRepository>(); }
        public string? CreatePicture() { return iPictureRepository.CreatePicture(); }
        public List<string>? CreatePictures() { return iPictureRepository.CreatePictures(); }
        public string GetPictureLocation(string id) { return iPictureRepository.GetPictureLocation(id); }
        public List<string> GetPictureLocations(List<string> ids) { return iPictureRepository.GetPictureLocations(ids); }
    }
}
