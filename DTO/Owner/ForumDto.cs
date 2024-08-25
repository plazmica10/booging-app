using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Owner
{
    public class ForumDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Question { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Loc { get; set; } = "";
        public Location Location { get; set; } = new();
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserPhoto { get; set; } = "";
        public ObservableCollection<ForumComment> Comments { get; set; } = new();
        public bool IsUseful { get; set; }
        public bool IsClosed { get; set; }


        private bool _selected;
        public bool Selected { get => _selected; set { _selected = value; OnPropertyChanged(nameof(Selected)); } }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
