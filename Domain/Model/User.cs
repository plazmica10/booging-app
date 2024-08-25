using System;
using System.Reflection.Metadata;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum UserRole { Unknown, Owner, Guest, Guide, Tourist }

    public class User : ISerializable
    {
        public int Id { get; set; } = -1;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.Unknown;
        public bool IsSuperUser { get; set; } = false;
        public int BonusPoints { get; set; } = 0;
        public string Photo { get; set; } = "";
        public bool HasFirstLoginHappened { get; set; } = false;
        public string GetHeader() { return "Id|Username|Password|FirstName|LastName|Email|Phone|Role|IsSuperUser|BonusPoints|Photo|HasFirstLoginHappened"; }

        public User() { }

        public User(string username, string password, string firstName, string lastName, string email, string phone, UserRole role, bool isSuperUser, int bonusPoints, string photo, bool hasFirstLoginHappened)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Role = role;
            IsSuperUser = isSuperUser;
            BonusPoints = bonusPoints;
            Photo = photo;
            HasFirstLoginHappened = hasFirstLoginHappened;
        }
        public User(User other)
        {
            Id = other.Id;
            Username = other.Username;
            Password = other.Password;
            FirstName = other.FirstName;
            LastName = other.LastName;
            Email = other.Email;
            Phone = other.Phone;
            Role = other.Role;
            IsSuperUser = other.IsSuperUser;
            BonusPoints = other.BonusPoints;
            Photo = other.Photo;
            HasFirstLoginHappened = other.HasFirstLoginHappened;
        }

        public string[] ToCsv() => new[] { $"{Id}", Username, Password, FirstName, LastName, Email, Phone, $"{Role}", $"{IsSuperUser}", $"{BonusPoints}", Photo, $"{HasFirstLoginHappened}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            Username = values[i++];
            Password = values[i++];
            FirstName = values[i++];
            LastName = values[i++];
            Email = values[i++];
            Phone = values[i++];
            Role = (UserRole)Enum.Parse(typeof(UserRole), values[i++]);
            IsSuperUser = bool.Parse(values[i++]);
            BonusPoints = Convert.ToInt32(values[i++]);
            Photo = values[i++];
            HasFirstLoginHappened = bool.Parse(values[i++]);
        }

        public override string ToString() => $"{Id}|{Username}|{Password}|{FirstName}|{LastName}|{Email}|{Phone}|{Role}|{IsSuperUser}|${BonusPoints}|{Photo}|{HasFirstLoginHappened}";

        public void BecomeSuperGuest()
        {
            IsSuperUser = true;
            BonusPoints = 5;
        }

        public void LoseSuperGuest()
        {
            IsSuperUser = false;
            BonusPoints = 0;
        }
    }
}
