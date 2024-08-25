using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemMessageViewModel : ViewModelBase
    {
        private readonly TourMessage _message;

        //public int Id => _message.Id;
        //public int TouristId => _message.TouristId;
        public int Index => _message.Index;
        public string Message => _message.Message.Replace("&#x0a;", "\n");
        public string ReceivedAt => $"{_message.ReceivedAt:yyyy-MM-dd HH:mm:ss}";

        public ItemMessageViewModel(TourMessage message) { _message = message; }
    }
}
