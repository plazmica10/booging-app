using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.WPF.View.Guest;

namespace BookingApp.Repository
{
    public class ForumRepository  : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/og_forums.csv";
        private readonly Serializer<Forum> _serializer;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
        }

        public List<Forum> GetAll()
        {
            var forums = _serializer.FromCsv(FilePath);
            return forums;
        }

        public List<Forum> GetAllByGuestId(int id)
        {
            var forums = _serializer.FromCsv(FilePath);
            return forums.FindAll(f => f.UserId == id);
        }

        public Forum GetById(int id)
        {
            return _serializer.FromCsv(FilePath).FirstOrDefault(f => f.Id == id) ?? throw new InvalidOperationException();
        }

        public Forum Update(Forum forum)
        {
            var forums = _serializer.FromCsv(FilePath);
            var current = forums.Find(f => f.Id == forum.Id);
            int index =  forums.IndexOf(current);
            forums.Remove(current);
            forums.Insert(index, forum);
            _serializer.ToCsv(FilePath, forums);
            return forum;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            var forums = _serializer.FromCsv(FilePath);
            forums.Add(forum);
            _serializer.ToCsv(FilePath, forums);
            return forum;
        }
        public int NextId()
        {
            var forums = _serializer.FromCsv(FilePath);
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(a => a.Id) + 1;
        }
    }
}
