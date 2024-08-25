using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel;

namespace BookingApp.DTO.Guest
{
    public class SearchDto : ViewModelBase
    {
        private string _accommodationName;
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }
        private string _location;
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }
        private int _guestCount;
        public int GuestCount { get => _guestCount; set { _guestCount = value; OnPropertyChanged(nameof(GuestCount)); } }
        private int _stayLength;
        public int StayLength { get => _stayLength; set { _stayLength = value; OnPropertyChanged(nameof(StayLength)); } }
        public List<string> AccommodationTypes
        {
            get
            {
                var types = new List<string> { "All" };
                types.AddRange(Enum.GetNames(typeof(AccommodationType)));
                return types;
            }
        }

        public SearchDto()
        {
            _accommodationName = "";
            _location = "";
            _guestCount = 1;
            _stayLength = 1;
        }
    }
}
