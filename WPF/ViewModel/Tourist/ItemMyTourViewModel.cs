using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ItemMyTourViewModel
    {
        private readonly PictureService _pictureService = new();

        private readonly Tour _tour;

        // hidden
        public int Id => _tour.Id;
        public TourStatus Status => _tour.Status;

        // display
        public string Name => _tour.Name;
        public string MainPicture { get; set; }
        public int CurrentPeakPointIndex => _tour.CurrentPeakPointIndex + 1;
        public string CurrentPeakPointName { get; set; }
        public string StatusText { get; set; }

        public Visibility RateTourButtonVisibility { get; set; } = Visibility.Collapsed;

        public ItemMyTourViewModel(Tour tour)
        {
            _tour = tour;
            CurrentPeakPointName = _tour.CurrentPeakPointIndex == -1 ? "..." : _tour.PeakPoints[_tour.CurrentPeakPointIndex];

            List<string> pictureLocations = _pictureService.GetPictureLocations(_tour.PictureIds);
            MainPicture = pictureLocations.Count > 0 ? pictureLocations[0] : "";

            switch (_tour.Status)
            {
                case TourStatus.NotStarted: StatusText = "\ud83d\udea5 Not Started ";                                            break;
                case TourStatus.Started:    StatusText = "\u25b6\ufe0f Started";                                                 break;
                case TourStatus.Canceled:   StatusText = "\u274c Canceled";                                                      break;
                case TourStatus.Finished:   StatusText = "\ud83c\udfc1 Finished"; RateTourButtonVisibility = Visibility.Visible; break;
                default:                    StatusText = "";                                                                     break;
            }
        }
    }
}
