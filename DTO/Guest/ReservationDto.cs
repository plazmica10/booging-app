using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.WPF.ViewModel;

namespace BookingApp.DTO.Guest
{
    public class ReservationDto :  ViewModelBase
    {
        private DateTime _checkIn;
        public DateTime CheckIn { get => _checkIn; set { if (_checkIn != value) { _checkIn = value; OnPropertyChanged(nameof(CheckIn)); } } }

        private DateTime _checkOut;
        public DateTime CheckOut { get => _checkOut; set { if (_checkOut != value) { _checkOut = value; OnPropertyChanged(nameof(CheckOut)); } } }

        private int _guestCount;
        public int GuestCount { get => _guestCount; set { if (_guestCount != value) { _guestCount = value; OnPropertyChanged(nameof(GuestCount)); } } }

        private int _stayLength;
        public int StayLength { get => _stayLength; set { if (_stayLength != value) { _stayLength = value; OnPropertyChanged(nameof(StayLength)); } } }

        public ReservationDto()
        {
            _checkIn = DateTime.Today.AddDays(1);
            _checkOut = DateTime.Today.AddDays(2);
            _guestCount = 1;
            _stayLength = 1;
        }
    }
}
