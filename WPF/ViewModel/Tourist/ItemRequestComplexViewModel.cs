using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemRequestComplexViewModel : ViewModelBase
    {
        private readonly TourRequestComplex _tourRequestComplex;

        // hidden
        public int Id => _tourRequestComplex.Id;

        // display
        public string Name => _tourRequestComplex.Name;
        public string Body { get; set; } = "";
        public string StatusText { get; set; }

        public List<ItemRequestViewModel> Parts { get; set; } = new();

        public ItemRequestComplexViewModel(TourRequestComplex tourRequestComplex)
        {
            _tourRequestComplex = tourRequestComplex;

            switch (_tourRequestComplex.Status)
            {
                case TourRequestStatus.Waiting:  StatusText = "\u231b Waiting..."; break;
                case TourRequestStatus.Approved: StatusText = "\u2714\ufe0f Approved"; break;
                case TourRequestStatus.Rejected: StatusText = "\u274c Rejected"; break;
                default: StatusText = ""; break;
            }

            foreach (TourRequest part in tourRequestComplex.Parts)
            {
                Parts.Add(new ItemRequestViewModel(part));
            }
        }
    }
}
