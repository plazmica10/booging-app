using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ToursViewModel : ViewModelBase
    {
        public User CurrentUser;
        public NavigationService MainNavigationService;

        private readonly TourSearchService _tourSearchService = new();

        public int SearchType { get; set; } = 0;
        public string SearchText { get; set; } = "";

        public ObservableCollection<ItemTourViewModel> Tours { get; set; } = new();

        public ICommand SearchCommand { get; }
        public ICommand OpenTourCommand { get; }
        public ICommand ReserveCommand { get; }

        public ToursViewModel(User currentUser, NavigationService navigationService, List<Tour>? tours = null)
        {
            CurrentUser = currentUser;
            MainNavigationService = navigationService;

            SearchCommand = new FunctionCommand(SearchAction);
            OpenTourCommand = new RelayCommand<ItemTourViewModel>(OpenTourAction);
            ReserveCommand = new RelayCommand<ItemTourViewModel>(ReserveAction);

            // populate tours if provided
            if (tours == null || tours.Count <= 0) { tours = _tourSearchService.GetAvailableTours(); }
            ToursAddRange(tours);
        }


        private void OpenTourAction(ItemTourViewModel parameter)
        {
            MainNavigationService.Navigate(new TourView(CurrentUser, MainNavigationService, parameter.Id));
        }

        private void ReserveAction(ItemTourViewModel parameter)
        {
            MainNavigationService.Navigate(new ReserveView(CurrentUser, MainNavigationService, parameter.Id));
        }


        private void ToursAddRange(List<Tour> items)
        {
            Tours.Clear();
            foreach (Tour item in items)
            {
                Tours.Add(new ItemTourViewModel(item));
            }
        }

        public void SearchAction()
        {
            try
            {
                ToursAddRange(SearchType switch
                {
                    0 => _tourSearchService.SearchAvailableToursByAllParameters(SearchText),
                    1 => _tourSearchService.SearchAvailableToursByLocation(SearchText),
                    2 => _tourSearchService.SearchAvailableToursByDuration(int.Parse(SearchText)),
                    3 => _tourSearchService.SearchAvailableToursByLanguage(SearchText),
                    4 => _tourSearchService.SearchAvailableToursByMaxTourists(int.Parse(SearchText)),
                    _ => throw new InvalidOperationException("Invalid SearchType value.")
                });
            }
            catch (Exception)
            {
                Tours.Clear();
            }
        }
    }
}