using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class RenovationsDto : INotifyPropertyChanged
    {
        private int _renovationId;
        public int RenovationId { get => _renovationId; set { _renovationId = value; OnPropertyChanged(nameof(RenovationId)); } }

        private int _accommodationId;
        public int AccommodationId { get => _accommodationId; set { _accommodationId = value; OnPropertyChanged(nameof(AccommodationId)); } }

        private string _accommodationName = "";
        public string AccommodationName { get => _accommodationName; set { _accommodationName = value; OnPropertyChanged(nameof(AccommodationName)); } }

        private string _location = "";
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }

        private AccommodationType _type;
        public AccommodationType Type { get => _type; set { _type = value; OnPropertyChanged(nameof(Type)); } }

        private string _accommodationPhoto = "";
        public string AccommodationPhoto { get => _accommodationPhoto; set { _accommodationPhoto = value; OnPropertyChanged(nameof(AccommodationPhoto)); } }

        private DateTime _startDate;
        public DateTime StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(nameof(StartDate)); } }

        private DateTime _endDate;
        public DateTime EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(nameof(EndDate)); } }

        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

        private RenovationStatus _status;
        public RenovationStatus Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

        private bool _selected;
        public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
