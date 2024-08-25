using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReservationSummaryViewModel : ViewModelBase
    {
        public NavigationService NavigationService;
        public ReservationSummaryViewModel(User user, ReservationsDto reservation,NavigationService navService)
        {
            CurrentUser = user;
            Reservation = reservation;
            NavigationService = navService;
            SortedImages = new ObservableCollection<string>(Reservation.Images.OrderBy(x => x));
            CurrentImage = SortedImages.First();
            PreviousCommand = new FunctionCommand(PreviousImage);
            NextCommand = new FunctionCommand(NextImage);
        }
        #region bindings and commands
        private string _currentImage;
        public string CurrentImage { get => _currentImage; set { _currentImage = value; OnPropertyChanged(nameof(CurrentImage)); } }
        public ObservableCollection<string> SortedImages { get; set; }
        private User _currentUser;
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(nameof(CurrentUser)); } }
        private ReservationsDto _reservation;
        public ReservationsDto Reservation { get => _reservation; set { _reservation = value; OnPropertyChanged(nameof(Reservation)); } }
        public Frame MainFrame;
        public ICommand PreviousCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand ExportCommand => new FunctionCommand(ExportToPDF);
        public int ImageCount { get { return SortedImages?.Count ?? 0; } }
        #endregion
        private void PreviousImage() { CurrentImage = ImageControl.GetPreviousImage(SortedImages, CurrentImage); }
        private void NextImage() { CurrentImage = ImageControl.GetNextImage(SortedImages, CurrentImage); }
        public void ExportToPDF() { ExportUtils.ExportToPDF(CurrentUser, Reservation); }

    }
}
