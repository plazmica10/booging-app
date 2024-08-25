using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    public class TourMessageService
    {
        private readonly ITourMessageRepository _iTourMessageRepository = Injector.Injector.CreateInstance<ITourMessageRepository>();


        public void SendMessageToTourist(int touristId, string message)
        {
            _iTourMessageRepository.SaveAppend(new TourMessage(touristId, message));
        }

        public void SendMessageToTourist(int touristId, Tour tour)
        {
            SendMessageToTourist(touristId, TourInfoMessage(tour));
        }

        public string TourInfoMessage(Tour newTour)
        {
            string message = "New Tour Alert!" + "&#x0a;" +
                             "Name: " + newTour.Name + "&#x0a;" +
                             "Description: " + newTour.Description + "&#x0a;" +
                             "Language: " + newTour.Language.ToReadableString() + "&#x0a;" +
                             "Location: " + newTour.Location.ToReadableString() +
                             "";
            return message;
        }
    }
}
