using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AccommodationsViewModel : ViewModelBase
    {
        private AccommodationService _accommodationService = new AccommodationService();
        private PictureService _pictureService = new PictureService();
        public NavigationService NavigationService;
        public User CurrentUser;

        public AccommodationsViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavigationService = navService;
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            CreatePhotoPaths(Accommodations);
        }
        #region bindings and commands

        private ObservableCollection<Accommodation> _accommodations;
        public ObservableCollection<Accommodation> Accommodations { get => _accommodations; set { _accommodations = value; OnPropertyChanged(nameof(Accommodations)); } }
        public SearchDto SearchDto { get; set; } = new SearchDto();
        private string _selectedType = "All";
        public string SelectedType { get => _selectedType; set { _selectedType = value; OnPropertyChanged(nameof(SelectedType)); } }
        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation { get => _selectedAccommodation; set { _selectedAccommodation = value; OnPropertyChanged(nameof(SelectedAccommodation)); } }
        public ICommand SearchCommand => new FunctionCommand(SearchButton_Click);
        public ICommand ViewCommand => new FunctionArgCommand(obj =>
        {
            SelectedAccommodation = obj as Accommodation;
            ViewAction();
        });
        public ICommand RefreshCommand => new FunctionCommand(RefreshPage);
        public ICommand AnywhereCommand => new FunctionCommand(AnywhereSearch);
        private Location _selectedLocation = new();
        public Location SelectedLocation { get => _selectedLocation; set { _selectedLocation = value; OnPropertyChanged(nameof(SelectedLocation)); } }
        #endregion

        public void SearchButton_Click()
        {
            AccommodationType? type = string.IsNullOrEmpty(SelectedType) || SelectedType == "All"
                ? null
                : (AccommodationType)Enum.Parse(typeof(AccommodationType), SelectedType);
            if (SelectedLocation == null) SelectedLocation = new();
            SearchDto.Location = SelectedLocation.City + SelectedLocation.Country;
            Accommodations = new ObservableCollection<Accommodation>(
                _accommodationService.SearchByAllParameters(SearchDto.AccommodationName, SearchDto.Location, type, SearchDto.GuestCount, SearchDto.StayLength));
            CreatePhotoPaths(Accommodations);
            if (Accommodations.Count == 0)
            {
                MessageBox.Show("No accommodations found");
            }
        }
        public void ViewAction()
        {
            NavigationService.Navigate(new AccommodationView(CurrentUser, SelectedAccommodation, NavigationService));
        }

        public void RefreshPage()
        {
            NavigationService.Navigate(new AccommodationsView(CurrentUser, NavigationService));
        }
        public void CreatePhotoPaths(ObservableCollection<Accommodation> accommodations)
        {
            foreach (var accommodation in accommodations) accommodation.Photos = _pictureService.GetPictureLocations(accommodation.Photos);
        }

        public void AnywhereSearch()
        {
            AnywhereSearchView anywhereSearchView = new AnywhereSearchView(CurrentUser, NavigationService);
            anywhereSearchView.ShowDialog();
        }
    }
}
