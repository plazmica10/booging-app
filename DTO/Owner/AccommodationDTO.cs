using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Owner
{
    public class AccommodationDTO : INotifyPropertyChanged
    {
        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(nameof(Id)); } }

        private string _name = "";
        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private string _location = "";
        public string Location { get => _location; set { _location = value; OnPropertyChanged(nameof(Location)); } }

        private AccommodationType _type;
        public AccommodationType Type { get => _type; set { _type = value; OnPropertyChanged(nameof(Type)); } }

        private int _maxGuests;
        public int MaxGuests { get => _maxGuests; set { _maxGuests = value; OnPropertyChanged(nameof(MaxGuests)); } }

        private int _minDays;
        public int MinDays { get => _minDays; set { _minDays = value; OnPropertyChanged(nameof(MinDays)); } }

        private int _refundPeriod;
        public int RefundPeriod { get => _refundPeriod; set { _refundPeriod = value; OnPropertyChanged(nameof(RefundPeriod)); } }

        public ObservableCollection<string> Photos { get; set; } = new();

        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
