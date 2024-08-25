using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Owner
{
    public class RecommendationDto : INotifyPropertyChanged
    {
        public int RecommendationId { get; set; }
        public string RecommendationText { get; set; } = "";
        public int AccommodationId { get; set; }
        public string AccommodationName { get; set; } = "";
        public string AccommodationLocation { get; set; } = "";
        public List<string> AccommodationPhotos { get; set; } = new();
        public int ReservationsInLastYear { get; set; }
        public double Occupation { get; set; }
        public bool Bad { get; set; }

        public List<RecommendationAccommodationDto> RecommendedAccommodations { get; set; } = new();
        public Location RecommendedLocation { get; set; } = new();

        private bool _selected;
        public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
