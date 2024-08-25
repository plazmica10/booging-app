using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class Forum : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Question { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public Location Location { get; set; } = new();
        public int  UserId { get; set; }
        public List<ForumComment> Comments { get; set; } = new();
        public bool IsUseful { get; set; }
        public bool IsClosed { get; set; }
        public bool IsNotified { get; set; }

        public  string GetHeader() { return "Id|Title|Question|City|Country|Location|UserId|IsUseful|IsClosed|IsNotified"; }
        public Forum(){}

        public Forum(string question, string title, string city, string country, Location location, int userId)
        {
            Title = title;
            Question = question;
            City = city;
            Country = country;
            Location = location;
            UserId = userId;
            Comments = new List<ForumComment>();
            IsUseful = false;
            IsClosed = false;
            IsNotified = false;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(),Title, Question, City, Country, $"{Location}", UserId.ToString(), IsUseful.ToString(), IsClosed.ToString(), IsNotified.ToString()
            };
            return csvValues;
        }

        public void FromCsv(string[] csvValues)
        {
            int i = 0;
            Id = int.Parse(csvValues[i++]);
            Title = csvValues[i++];
            Question = csvValues[i++];
            City = csvValues[i++];
            Country = csvValues[i++];
            Location = Location.Parse(csvValues[i++]);
            UserId = int.Parse(csvValues[i++]);
            IsUseful = Convert.ToBoolean(csvValues[i++]);
            IsClosed = Convert.ToBoolean(csvValues[i++]);
            IsNotified = Convert.ToBoolean(csvValues[i++]);
        }

    }
}
