using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IForumCommentRepository
    {
        public List<ForumComment> GetAllByForumId(int forumId);
        public ForumComment GetById(int id);
        public ForumComment Save(ForumComment comment);
        public void Update(ForumComment comment);
    }
}
