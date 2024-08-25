using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class TourMessageRepository : ITourMessageRepository
    {
        private const string FilePath = "../../../Resources/Data/gt_message.csv";
        private readonly Serializer<TourMessage> _serializer = new();

        public TourMessageRepository() { }

        public List<TourMessage> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public TourMessage? GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(t => t.Id == id);
        }

        public TourMessage SaveAppend(TourMessage tourMessage)
        {
            List<TourMessage> tourMessages = _serializer.FromCsv(FilePath);
            tourMessage.Id = tourMessages.Count < 1 ? 1 : tourMessages.Max(t => t.Id) + 1;
            List<TourMessage> touristMessages = tourMessages.FindAll(c => c.TouristId == tourMessage.TouristId);
            tourMessage.Index = touristMessages.Count < 1 ? 1 : touristMessages.Max(t => t.Index) + 1;
            tourMessages.Add(tourMessage);
            _serializer.ToCsv(FilePath, tourMessages);
            return tourMessage;
        }

        public bool Delete(TourMessage tourMessage)
        {
            List<TourMessage> tourMessages = _serializer.FromCsv(FilePath);
            TourMessage? found = tourMessages.Find(t => t.Id == tourMessage.Id);
            if (found == null) { return false; }
            tourMessages.Remove(found);
            return _serializer.ToCsv(FilePath, tourMessages);
        }

        public List<TourMessage> GetAllByTouristId(int touristId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.TouristId == touristId);
        }
    }
}
