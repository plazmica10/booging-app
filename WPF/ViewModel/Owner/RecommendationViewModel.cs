using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Owner;
using BookingApp.WPF.View.Owner;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class RecommendationViewModel : ViewModelBase
    {
        #region Bindings

        private ObservableCollection<RecommendationDto> _recommendations;
        public ObservableCollection<RecommendationDto> Recommendations { get => _recommendations; set { _recommendations = value; OnPropertyChanged(nameof(Recommendations)); } }

        private RecommendationDto _selectedRecommendation;
        public RecommendationDto SelectedRecommendation { get => _selectedRecommendation; set { _selectedRecommendation = value; OnPropertyChanged(nameof(SelectedRecommendation)); } }

        private bool _isPopupOpen;
        public bool IsPopupOpen { get => _isPopupOpen; set { _isPopupOpen = value; OnPropertyChanged(nameof(IsPopupOpen)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;

        private readonly RecommendationService _recommendationService = new();
        private readonly AccommodationService _accommodationService = new();

        public ICommand OpenRecommendationCommand => new FunctionArgCommand(obj => { if (obj is int o) OpenRecommendation(o); });
        public ICommand OpenAccommodationDetailsCommand => new FunctionArgCommand(obj => { if (obj is int o) NavService.Navigate(new AccommodationView(CurrentUser, NavService, o)); });
        public ICommand IgnoreCommand => new FunctionCommand(Ignore);
        public ICommand DetailsCommand => new FunctionCommand(() => NavService.Navigate(new AccommodationView(CurrentUser, NavService, SelectedRecommendation.AccommodationId)));
        public ICommand OpenAccommodationCommand => new FunctionCommand(() => NavService.Navigate(new AddAccommodationView(CurrentUser, NavService)));
        public ICommand CloseAccommodationCommand => new FunctionCommand(() => IsPopupOpen = true);
        public ICommand ClosePopupCommand => new FunctionCommand(() => IsPopupOpen = false);
        public ICommand ConfirmDeleteCommand => new FunctionCommand(CloseAccommodation);


        public RecommendationViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;

            LoadRecommendations();
        }

        private void CloseAccommodation()
        {
            _accommodationService.CloseAccommodation(SelectedRecommendation.AccommodationId);
            NavService.Navigate(new AccommodationsView(CurrentUser, NavService, SelectedRecommendation.AccommodationId));
        }

        private void Ignore()
        {
            Recommendations.Remove(Recommendations.First(r => r.RecommendationId == SelectedRecommendation.RecommendationId));
            SelectedRecommendation = new();
            if (Recommendations.Count > 0)
                OpenRecommendation(Recommendations[0].RecommendationId);
        }

        private void LoadRecommendations()
        {
            Recommendations = new ObservableCollection<RecommendationDto>(_recommendationService.GetRecommendations(CurrentUser.Id));
            if (Recommendations.Count > 0)
                OpenRecommendation(Recommendations[0].RecommendationId);
        }

        private void OpenRecommendation(int reccomendationId)
        {
            foreach (var recommendation in Recommendations)
            {
                recommendation.Selected = recommendation.RecommendationId == reccomendationId;
            }
            SelectedRecommendation = Recommendations.FirstOrDefault(r => r.Selected) ?? Recommendations.First();
        }
    }
}
