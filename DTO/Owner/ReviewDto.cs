using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class ReviewDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string GuestName { get; set; } = "";
        public string GuestPhoto { get; set; } = "";
        public string AccommodationName { get; set; } = "";
        public int AccommodationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Cleanliness { get; set; }
        public int OwnerRating { get; set; }
        public string Comment { get; set; } = "";
        public List<string> Photos { get; set; } = new();
        public string AccommodationLocation { get; set; } = "";
        public string AccommodationPhoto { get; set; } = "";
        public int GuestCount { get; set; }
        private bool _selected;
        public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
