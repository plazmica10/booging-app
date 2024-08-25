using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using LiveCharts;
using LiveCharts.Wpf;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class ProfileViewModel : ViewModelBase
    {
        #region Bindings

        private SeriesCollection _pieChartSeries = new();
        public SeriesCollection PieChartSeries { get => _pieChartSeries; set { _pieChartSeries = value; OnPropertyChanged(nameof(PieChartSeries)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;
        public ProfileDto Profile { get; set; } = new();

        private readonly OwnerReviewService _ownerReviewService = new();
        private readonly UserService _userService = new();
        private readonly PictureService _pictureService = new();

        public ProfileViewModel(User user, NavigationService navService)
        {
            _userService.RefreshSuper();
            CurrentUser = _userService.GetById(user.Id) ?? user;
            NavService = navService;

            FillProfile();
            DrawPieChart();
        }

        private void DrawPieChart()
        {
            List<int> reviews = _ownerReviewService.GetRatingsForOwner(CurrentUser.Id);
            reviews.Sort();

            var groupedReviews = reviews.GroupBy(r => r)
                .Select(group => new { Score = group.Key, Count = group.Count() });

            PieChartSeries.Clear();
            foreach (var group in groupedReviews)
            {
                PieChartSeries.Add(new PieSeries { Title = $"{group.Score} star;", Values = new ChartValues<int> { group.Count } });
            }
        }

        private void FillProfile()
        {
            Profile.Id = CurrentUser.Id;
            Profile.Name = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
            Profile.Email = CurrentUser.Email;
            Profile.PhoneNumber = CurrentUser.Phone;
            Profile.Photo = _pictureService.GetPictureLocation(CurrentUser.Photo);
            Profile.IsSuperUser = CurrentUser.IsSuperUser;
            Profile.AverageRating = _ownerReviewService.AverageRatingByOwnerId(CurrentUser.Id);
            Profile.TotalReviews = _ownerReviewService.CountByOwnerId(CurrentUser.Id);
        }
    }
}
