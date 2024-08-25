using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class NewForumDto : INotifyPropertyChanged
    {
        public int ForumId { get; set; }
        public string GuestName { get; set; } = "";
        public string GuestPhoto { get; set; } = "";
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
