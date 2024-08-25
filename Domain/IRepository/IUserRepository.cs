using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepository
{
    public interface IUserRepository
    {
        public bool DeleteAll();
        public User SaveAppend(User user);
        public User? GetByUsername(string username);
        public User? GetByEmail(string username);
        public User? GetById (int id);
        public List<User> GetAllByRole(UserRole role);
        public bool Update(User user);

    }
}
