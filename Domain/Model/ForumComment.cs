using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class ForumComment  :  ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserPhoto { get; set; } = "";
        public int ForumId { get; set; }
        private string _comment = "";
        public string Comment { get => _comment; set { _comment = value; OnPropertyChanged(nameof(Comment)); } }
        public bool IsUseful { get; set; }
        public bool IsOwner { get; set; }
        private int _reportCount;
        public int ReportCount { get => _reportCount; set { _reportCount = value; OnPropertyChanged(nameof(ReportCount)); } }
        public List<int> ReportedBy { get; set; } = new List<int> {-1};

        public string GetHeader() { return "Id|UserId|UserName|UserPhoto|ForumId|Comment|IsUseful|IsOwner|ReportCount|ReportedBy"; }
        public ForumComment() { }

        public ForumComment(int userId,string userName, string userPhoto, int forumId, string comment, int reportCount, List<int> reported)
        {
            UserId = userId;
            UserName = userName;
            UserPhoto = userPhoto;
            ForumId = forumId;
            Comment = comment;
            IsUseful = false;
            IsOwner = false;
            ReportCount = reportCount;
            ReportedBy = reported;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), UserId.ToString(), UserName, UserPhoto, ForumId.ToString(), Comment, IsUseful.ToString(), IsOwner.ToString(), ReportCount.ToString(), string.Join(",", ReportedBy)
            };
            return csvValues;
        }

        public void FromCsv(string[] csvValues)
        {
            int i = 0;
            Id = int.Parse(csvValues[i++]);
            UserId = int.Parse(csvValues[i++]);
            UserName = csvValues[i++];
            UserPhoto = csvValues[i++];
            ForumId = int.Parse(csvValues[i++]);
            Comment = csvValues[i++];
            IsUseful = bool.Parse(csvValues[i++]);
            IsOwner = bool.Parse(csvValues[i++]);
            ReportCount = int.Parse(csvValues[i++]);
            ReportedBy = csvValues[i++].Split(',').Select(int.Parse).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
