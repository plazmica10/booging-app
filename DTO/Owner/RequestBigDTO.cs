using System;
using System.ComponentModel;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Owner
{
    public class RequestBigDTO : INotifyPropertyChanged
    {
        private int _requestId;
        public int RequestId { get => _requestId; set { _requestId = value; OnPropertyChanged(nameof(RequestId)); } }
        
        private string _guestName = "";
        public string GuestName { get => _guestName; set { _guestName = value; OnPropertyChanged(nameof(GuestName)); } }

        private DateTime _oldCheckIn;
        public DateTime OldCheckIn { get => _oldCheckIn; set { _oldCheckIn = value; OnPropertyChanged(nameof(OldCheckIn)); } }
        
        private DateTime _oldCheckOut;
        public DateTime OldCheckOut { get => _oldCheckOut; set { _oldCheckOut = value; OnPropertyChanged(nameof(OldCheckOut)); } }

        private DateTime _newCheckIn;
        public DateTime NewCheckIn { get => _newCheckIn; set { _newCheckIn = value; OnPropertyChanged(nameof(NewCheckIn)); } }

        private DateTime _newCheckOut;
        public DateTime NewCheckOut { get => _newCheckOut; set { _newCheckOut = value; OnPropertyChanged(nameof(NewCheckOut)); } }

        private bool _datesAvailable;
        public bool DatesAvailable { get => _datesAvailable; set { _datesAvailable = value; OnPropertyChanged(nameof(DatesAvailable)); } }

        private RequestStatus _status;
        public RequestStatus Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

        private string _guestPhoto = "";
        public string GuestPhoto { get => _guestPhoto; set { _guestPhoto = value; OnPropertyChanged(nameof(GuestPhoto)); } }

        private int _accommodationId;
        public int AccommodationId { get => _accommodationId; set { _accommodationId = value; OnPropertyChanged(nameof(AccommodationId)); } }

        private string _accommodationName = "";
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }

        private string _accommodationLocation = "";
        public string AccommodationLocation { get => _accommodationLocation; set { _accommodationLocation = value; OnPropertyChanged(nameof(AccommodationLocation)); } }

        private string _accommodationPhoto = "";
        public string AccommodationPhoto { get => _accommodationPhoto; set { _accommodationPhoto = value; OnPropertyChanged(nameof(AccommodationPhoto)); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
