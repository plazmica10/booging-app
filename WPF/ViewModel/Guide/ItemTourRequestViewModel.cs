using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemTourRequestViewModel : ViewModelBase
    {
        private readonly TourRequest _tourRequest;
        public int Id => _tourRequest.Id;
        public string Name => _tourRequest.Name;
        public string Location => _tourRequest.Location.ToSmallString();
        public string Language => _tourRequest.Language.ToReadableString();
        public string SpanStart => $"{_tourRequest.SpanStart:yyyy-MM-dd}";
        public string SpanEnd => $"{_tourRequest.SpanEnd:yyyy-MM-dd}";
        public string PeopleCount => _tourRequest.Reservations.Count.ToString();
        public ItemTourRequestViewModel(TourRequest tour)
        {
            _tourRequest = tour;
        }
    }
}
