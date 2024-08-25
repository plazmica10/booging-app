using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public List<Forum> GetAllByGuestId(int id);
        public Forum GetById(int id);
        public Forum Save(Forum forum);
        public Forum Update(Forum forum);
    }
}
