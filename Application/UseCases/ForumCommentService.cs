using BookingApp.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    public class ForumCommentService
    {
        private readonly IForumRepository _forumRepository;
        private readonly IForumCommentRepository _forumCommentRepository;
        private readonly UserService _userService = new();
        private readonly AccommodationReservationService _accommodationReservationService = new();
        private readonly AccommodationService _accommodationService = new();

        public ForumCommentService()
        {
            _forumRepository = Injector.Injector.CreateInstance<IForumRepository>();
            _forumCommentRepository = Injector.Injector.CreateInstance<IForumCommentRepository>();
        }

        public void IsUseful(ForumComment comment,string city,string country)
        {
            var useful = false;
            var reservations = _accommodationReservationService.GetAll(comment.UserId,true);
            foreach (var res in reservations)
            {
                var accom = _accommodationService.GetById(res.AccommodationId);
                if(comment.UserId == res.GuestId && accom.City == city && accom.Country == country)  useful = true;
            }
            if (useful)  comment.IsUseful = true;
            else comment.IsUseful = false;
            string temp = comment.UserPhoto;
            comment.UserPhoto = GetById(comment.Id).UserPhoto;
            _forumCommentRepository.Update(comment);
            comment.UserPhoto = temp;
        }

        public void Update(ForumComment comment)
        {
            _forumCommentRepository.Update(comment);
        }

        public ForumComment GetById(int id)
        {
            return _forumCommentRepository.GetById(id);
        }
    }
}
