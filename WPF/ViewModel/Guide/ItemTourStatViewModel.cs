using BookingApp.DTO.Guide;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemTourStatViewModel : ViewModelBase
    {
        private readonly TouristsAgeStatisticsDto _touristsAgeStatsDto;

        public int Id => _touristsAgeStatsDto.Id;
        public string TourName => _touristsAgeStatsDto.TourName;
        public string NumTourist => _touristsAgeStatsDto.NumTourist.ToString();
        public string Under18 => _touristsAgeStatsDto.Under18.ToString();
        public string Between18And50 => _touristsAgeStatsDto.Between18And50.ToString();
        public string Above50 => _touristsAgeStatsDto.Above50.ToString();
        public TouristsAgeStatisticsDto Statistics => _touristsAgeStatsDto;
        public ItemTourStatViewModel(TouristsAgeStatisticsDto touristsAgeStatsDto)
        {
            _touristsAgeStatsDto = touristsAgeStatsDto;
        }
    }
}
