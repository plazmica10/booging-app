using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Command;
using BookingApp.Domain.Model;
using BookingApp.DTO.Guest;
using BookingApp.Utilities;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class RequestDetailsViewModel : ViewModelBase
    {
        public User User { get; set; }
        public NavigationService NavService { get; set; }
        public RequestDto Details { get; set; }

        private string _currentImage;
        public string CurrentImage { get => _currentImage; set { _currentImage = value; OnPropertyChanged(nameof(CurrentImage)); } }
        public ICommand NextCommand => new FunctionCommand(NextImage);
        public ICommand PreviousCommand => new FunctionCommand(PreviousImage);
        public int ImageCount { get { return SortedImages?.Count ?? 0; } }
        public ObservableCollection<string> SortedImages { get; set; }

        public RequestDetailsViewModel(User user, NavigationService navService,RequestDto req)
        {
            this.User = user;
            this.NavService = navService;
            this.Details = req;
            SortedImages = new ObservableCollection<string>(req.Images.OrderBy(x => x));
            CurrentImage = SortedImages.First();
        }
        private void NextImage() { CurrentImage = ImageControl.GetNextImage(SortedImages, CurrentImage); }
        private void PreviousImage() { CurrentImage = ImageControl.GetPreviousImage(SortedImages, CurrentImage); }
    }
}
