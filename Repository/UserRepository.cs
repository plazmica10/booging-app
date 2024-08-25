using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer = new();

        // public List<User> GetForPeriodByUserId()
        // {
        //     return _serializer.FromCSV(FilePath);
        // }

        public bool DeleteAll()
        {
            List<User> users = new List<User>();
            return _serializer.ToCsv(FilePath, users);
        }

        public User SaveAppend(User user)
        {
            List<User> users = _serializer.FromCsv(FilePath);
            user.Id = users.Count < 1 ? 1 : users.Max(t => t.Id) + 1;
            users.Add(user);
            _serializer.ToCsv(FilePath, users);
            return user;
        }

        public User? GetById(int id)
        {
            return _serializer.FromCsv(FilePath).FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return _serializer.FromCsv(FilePath).FirstOrDefault(u => u.Username == username);
        }

        public User? GetByEmail(string email)
        {
            return _serializer.FromCsv(FilePath).FirstOrDefault(u => u.Email == email);
        }

        // public List<User> GetByRole(UserRole role)
        // {
        //     return _serializer.FromCSV(FilePath).FindAll(c => c.Role == role);
        // }

        public List<User> GetAllByRole(UserRole role)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.Role == role);
        }

        public bool Update(User user)
        {
            List<User> users = _serializer.FromCsv(FilePath);
            User? current = users.FirstOrDefault(u => u.Id == user.Id);
            if (current == null) { return false; }
            int index = users.IndexOf(current);
            users.Remove(current);
            users.Insert(index, user);
            return _serializer.ToCsv(FilePath, users);
        }
    }
}
