using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly IForumCommentRepository _forumCommentRepository;
        private readonly UserService _userService = new UserService();
        private readonly AccommodationReservationService _accommodationReservationService =  new AccommodationReservationService();
        private readonly AccommodationService _accommodationService = new AccommodationService();
        private  readonly ForumCommentService  _forumCommentService = new ForumCommentService();
        private readonly PictureService _pictureService = new();

        public ForumService()
        {
            _forumRepository = Injector.Injector.CreateInstance<IForumRepository>();
            _forumCommentRepository = Injector.Injector.CreateInstance<IForumCommentRepository>();
        }
        public List<Forum> GetAll()
        {
            var forums = _forumRepository.GetAll();
            foreach (var forum in forums)
            {
                forum.Comments = _forumCommentRepository.GetAllByForumId(forum.Id);
                foreach (var comment in forum.Comments)
                {
                    comment.UserPhoto = _pictureService.GetPictureLocation(comment.UserPhoto);
                }
            }
            return forums;
        }

        public List<Forum> GetByLocation(List<(string City, string Country)> locations)
        {
            var forums = GetAll();
            List<Forum> outs = new();
            foreach (var forum in forums)
            {
                foreach (var location in locations)
                {
                    if (forum.City == location.City && forum.Country == location.Country)
                    {
                        outs.Add(forum);
                        break;
                    }
                }
            }
            return outs;
        }
        public Forum GetById(int id)
        {
            var forum =  _forumRepository.GetById(id);
            forum.Comments = _forumCommentRepository.GetAllByForumId(forum.Id);
            return forum;
        }
        public Forum Save(Forum forum)
        {
            return _forumRepository.Save(forum);
        }
        public Forum Update(Forum forum)
        {
            return _forumRepository.Update(forum);
        }
        public List<Forum> GetAllByGuestId(int id)
        {
            var guestForums  = _forumRepository.GetAllByGuestId(id);
            foreach (var forum in guestForums)
            {
                forum.Comments = _forumCommentRepository.GetAllByForumId(forum.Id);
            }
            return guestForums;
        }
        public List<Forum> GetAllOther(int guestId)
        {
            var forums = _forumRepository.GetAll().Where(forum => forum.UserId != guestId).ToList();
            foreach (var forum in forums)
            {
                forum.Comments = _forumCommentRepository.GetAllByForumId(forum.Id);
            }
            return forums;
        }
        public ForumComment Save(ForumComment comment)
        {
            return _forumCommentRepository.Save(comment);
        }
        public void IsUseful(Forum forum)
        {
            var owners = _accommodationService.GetOwnerIdsForLocation(forum.City, forum.Country);
            var guests = _accommodationService.GetGuestIdsForLocation(forum.City, forum.Country);
            var guestCount = 0;
            var ownerCount = 0;
            var useful = false;

            if (forum.Comments.Count == 0) return;

            foreach (var comment in forum.Comments)
            {
                _forumCommentService.IsUseful(comment, forum.City, forum.Country);
                if (guests.Contains(comment.UserId)) guestCount++;
                if (owners.Contains(comment.UserId)) ownerCount++;
            }
            forum.IsUseful = guestCount >= 20 && ownerCount >= 10;
            
            Update(forum);
        }
        public bool IsValid(Forum forum,Accommodation accommodation,int guestResId,int commUserId)
        {
            return guestResId == commUserId && forum.City == accommodation.City && forum.Country == accommodation.Country;
        }

    }
}
