using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/og_forum_comments.csv";
        private readonly Serializer<ForumComment> _serializer;

        public ForumCommentRepository()
        {
            _serializer = new Serializer<ForumComment>();
        }

        public List<ForumComment> GetAllByForumId(int forumId)
        {
            var forumComments = _serializer.FromCsv(FilePath).Where(comment => comment.ForumId == forumId).ToList();
            return forumComments;
        }

        public ForumComment GetById(int id)
        {
            var comments = _serializer.FromCsv(FilePath);
            return comments.FirstOrDefault(a => a.Id == id);
        }

        public ForumComment Save(ForumComment comment)
        {
            comment.Id = NextId();
            var comments = _serializer.FromCsv(FilePath);
            comments.Add(comment);
            _serializer.ToCsv(FilePath, comments);
            return comment;
        }

        public void Update(ForumComment comment)
        {
            var comments = _serializer.FromCsv(FilePath);
            var commentToUpdate = comments.FirstOrDefault(a => a.Id == comment.Id);
            var index = comments.IndexOf(commentToUpdate);
            comments.RemoveAt(index);
            comments.Insert(index, comment);
            _serializer.ToCsv(FilePath, comments);
        }
        public int NextId()
        {
            var comments = _serializer.FromCsv(FilePath);
            if (comments.Count < 1)
            {
                return 1;
            }
            return comments.Max(a => a.Id) + 1;
        }
    }
}
