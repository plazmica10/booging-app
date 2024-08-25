using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemTourViewModel : ViewModelBase
    {
        private readonly PictureService _pictureService = new();

        private readonly Tour _tour;

        // hidden
        public int Id => _tour.Id;


        // display
        public string MainPicture { get; set; }
        public string Name => _tour.Name;
        public string Location => _tour.Location.ToSmallString();
        public string Description => _tour.Description.ToString();
        public string Language => _tour.Language.ToReadableString();
        public string MaxTourists => _tour.MaxTourists.ToString();
        public string SpaceLeft => _tour.SpaceLeft.ToString();
        public string StartsAt => $"{_tour.StartsAt:yyyy-MM-dd HH:mm:ss}";
        public string Duration => _tour.Duration.ToString() + "h";
        public string StatusText { get; set; }

        public ItemTourViewModel(Tour tour)
        {
            _tour = tour;

            // status text
            switch (_tour.Status)
            {
                case TourStatus.NotStarted: StatusText = "\ud83d\udea5 Not Started "; break;
                case TourStatus.Started:    StatusText = "\u25b6\ufe0f Started";      break;
                case TourStatus.Canceled:   StatusText = "\u274c Canceled";           break;
                case TourStatus.Finished:   StatusText = "\ud83c\udfc1 Finished";     break;
                default:                    StatusText = "...";                       break;
            }

            List<string> pictureLocations = _pictureService.GetPictureLocations(_tour.PictureIds);
            MainPicture = pictureLocations.Count > 0 ? pictureLocations[0] : "";
        }
    }
}
