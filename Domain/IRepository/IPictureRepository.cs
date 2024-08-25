using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface IPictureRepository
    {
        public string? CreatePicture();
        public List<string>? CreatePictures();
        public string GetPictureLocation(string id);
        public List<string> GetPictureLocations(List<string> ids);

    }
}
