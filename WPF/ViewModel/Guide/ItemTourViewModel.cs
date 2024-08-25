using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class ItemTourViewModel : ViewModelBase
    {
        private readonly Tour _tour;
        public int Id => _tour.Id;
        public int GuideId => _tour.GuideId;
        public string Name => _tour.Name;
        public string Location => _tour.Location.ToSmallString();
        public string Description => _tour.Description.ToString();
        public string Language => _tour.Language.ToReadableString();
        public string MaxTourists => _tour.MaxTourists.ToString();
        public string SpaceLeft => _tour.SpaceLeft.ToString();
        public string StartsAt => $"{_tour.StartsAt:yyyy-MM-dd HH:mm:ss}";
        public string Duration => _tour.Duration.ToString();
        public string Status => _tour.Status.ToString();
        public string CurrentPeakPointIndex => _tour.CurrentPeakPointIndex.ToString();
        public List<string> PeakPoints => _tour.PeakPoints;

        public ItemTourViewModel(Tour tour)
        {
            _tour = tour;
        }
    }
}
