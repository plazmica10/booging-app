using BookingApp.Domain.Model;
using System.ComponentModel;

namespace BookingApp.DTO.Owner
{
    public class InboxRequestDTO : INotifyPropertyChanged
    {
        public int RequestId { get; set; }
        public string UserName { get; set; } = "";
        public string UserPhoto { get; set; } = "1";
        public RequestStatus Status { get; set; }
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
