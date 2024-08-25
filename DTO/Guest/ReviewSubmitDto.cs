using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.WPF.ViewModel;

namespace BookingApp.DTO.Guest
{
    public class ReviewSubmitDto  : ViewModelBase
    {
        public string _comment;
        public string Comment { get => _comment; set { _comment = value; OnPropertyChanged(nameof(Comment)); } }

        public int _ownerRating;
        public int OwnerRating { get => _ownerRating; set { _ownerRating = value; OnPropertyChanged(nameof(OwnerRating)); } }

        public int _cleanliness;
        public int Cleanliness { get => _cleanliness; set { _cleanliness = value; OnPropertyChanged(nameof(Cleanliness)); } }

        private readonly ObservableCollection<string> _PhotoLocations;
        public IEnumerable<string> PhotoLocations => _PhotoLocations;
    }
}
