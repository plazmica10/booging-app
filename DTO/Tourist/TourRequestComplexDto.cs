using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BookingApp.DTO.Tourist
{
    public class TourRequestComplexDto
    {
        public int TouristId { get; set; }
        public string Name { get; set; } = "";
        public ObservableCollection<TourRequestDto> Parts { get; set; } = new();
        public TourRequestComplexDto(int touristId, string name)
        {
            TouristId = touristId;
            Name = name;
        }
    }
}