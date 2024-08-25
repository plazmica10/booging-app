using System.Collections.Generic;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemRequestViewModel : ViewModelBase
    {
        private readonly TourRequest _tourRequest;

        // hidden
        public int Id => _tourRequest.Id;

        // display
        public string Name => _tourRequest.Name;
        public string Location => _tourRequest.Location.ToSmallString();
        public string Language => _tourRequest.Language.ToReadableString();
        public string SpanStart => $"{_tourRequest.SpanStart:yyyy-MM-dd}";
        public string SpanEnd => $"{_tourRequest.SpanEnd:yyyy-MM-dd}";
        public string PeopleCount => _tourRequest.Reservations.Count.ToString();
        public string StatusText { get; set; }

        public ItemRequestViewModel(TourRequest tour)
        {
            _tourRequest = tour;

            switch (_tourRequest.Status)
            {
                case TourRequestStatus.Waiting:  StatusText = "\u231b Waiting..."; break;
                case TourRequestStatus.Approved: StatusText = "\u2714\ufe0f Approved"; break;
                case TourRequestStatus.Rejected: StatusText = "\u274c Rejected"; break;
                default: StatusText = ""; break;
            }
        }
    }
}
