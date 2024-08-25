using BookingApp.Application.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApp.WPF.View.Owner;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AddAccommodationViewModel : ViewModelBase
    {
        #region Bindings

        private string _name = "";
        public string AccommName { get => _name; set { _name = value; OnPropertyChanged(nameof(AccommName)); } }

        public IEnumerable<string> AccommodationTypes => Enum.GetNames(typeof(AccommodationType));

        private string? _selectedType;
        public string? SelectedType { get => _selectedType; set { _selectedType = value; OnPropertyChanged(nameof(SelectedType)); TypeVisible = false; ShowPopup = false; } }

        private int _maxGuests = 1;
        public int MaxGuests { get => _maxGuests; set { _maxGuests = value; OnPropertyChanged(nameof(MaxGuests)); } }

        private int _minDays = 1;
        public int MinDays { get => _minDays; set { _minDays = value; OnPropertyChanged(nameof(MinDays)); } }

        private int _refundPeriod = 1;
        public int RefundPeriod { get => _refundPeriod; set { _refundPeriod = value; OnPropertyChanged(nameof(RefundPeriod)); } }

        private Location _selectedLocation = new();
        public Location SelectedLocation { get => _selectedLocation; set { _selectedLocation = value; OnPropertyChanged(nameof(SelectedLocation)); } }

        private string _description = "";
        public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

        private bool _typeVisible = true;
        public bool TypeVisible { get => _typeVisible; set { _typeVisible = value; OnPropertyChanged(nameof(TypeVisible)); } }

        private bool _showPopup;
        public bool ShowPopup { get => _showPopup; set { _showPopup = value; OnPropertyChanged(nameof(ShowPopup)); } }

        public ObservableCollection<string> PhotoLocations { get; set; } = new();

        private string _currentPhoto = "";
        public string CurrentPhoto { get => _currentPhoto; set { _currentPhoto = value; OnPropertyChanged(nameof(CurrentPhoto)); } }

        #endregion

        public User CurrentUser;
        public NavigationService NavService;
        private List<string> _photos = new();


        private readonly AccommodationService _accommService = new();
        private readonly PictureService _pictureService = new();

        public ICommand SaveCommand => new FunctionCommand(SaveAction);
        public ICommand CancelCommand => new FunctionCommand(CancelAction);
        public ICommand AddPhotoCommand => new FunctionCommand(AddPhotoAction);
        public ICommand PopupCommand => new FunctionCommand(() => ShowPopup = !ShowPopup);
        public ICommand NextPhotoCommand => new FunctionCommand(NextPhoto);
        public ICommand PrevPhotoCommand => new FunctionCommand(PrevPhoto);

        // Constructor
        public AddAccommodationViewModel(User user, NavigationService navService)
        {
            CurrentUser = user;
            NavService = navService;
        }

        private void SaveAction()
        {
            if (!ValidateRequiredFields()) return;

            _accommService.Save(BuildAccommodation());
            MessageBox.Show("Accommodation added successfully.");
            NavService.Navigate(new AccommodationsView(CurrentUser, NavService));
        }

        private void CancelAction()
        {
            NavService.GoBack();
        }

        private void AddPhotoAction()
        {
            List<string>? photos = _pictureService.CreatePictures();
            if (photos == null) { return; }

            foreach (string photo in photos)
            {
                _photos.Add(photo);
                PhotoLocations.Add(_pictureService.GetPictureLocation(photo));
                CurrentPhoto = PhotoLocations.Last();
            }
        }

        private void NextPhoto()
        {
            if (PhotoLocations.Count == 0) return;
            int index = PhotoLocations.IndexOf(CurrentPhoto);
            if (index == PhotoLocations.Count - 1)
                CurrentPhoto = PhotoLocations[0];
            else
                CurrentPhoto = PhotoLocations[index + 1];
        }

        private void PrevPhoto()
        {
            if (PhotoLocations.Count == 0) return;
            int index = PhotoLocations.IndexOf(CurrentPhoto);
            if (index == 0)
                CurrentPhoto = PhotoLocations.Last();
            else
                CurrentPhoto = PhotoLocations[index - 1];
        }

        // helper methods
        private bool ValidateRequiredFields()
        {
            if (string.IsNullOrEmpty(AccommName) || string.IsNullOrEmpty(SelectedType) || MaxGuests == 0 ||
                MinDays == 0 || RefundPeriod == 0 || SelectedLocation.Id == -1)
            {
                MessageBox.Show("Please fill in all required fields.");
                return false;
            }
            return true;
        }

        private Accommodation BuildAccommodation()
        {
            return new Accommodation { Name = AccommName, Country = SelectedLocation.Country, City = SelectedLocation.City, Location = SelectedLocation, Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), SelectedType), MaxGuests = MaxGuests, MinDays = MinDays, RefundPeriod = RefundPeriod, Photos = _photos.ToList(), OwnerId = CurrentUser.Id, Description = Description };
        }
    }
}
