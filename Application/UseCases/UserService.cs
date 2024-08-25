using System.Collections.Generic;
using System.Windows;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;

namespace BookingApp.Application.UseCases
{
    public class UserService
    {
        private readonly IUserRepository _userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        private readonly IOwnerReviewRepository _ownerReviewRepository = Injector.Injector.CreateInstance<IOwnerReviewRepository>();
        private readonly IAccommodationReservationRepository _reservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        public void DeleteAll() { _userRepository.DeleteAll(); }
        public User SaveAppend(User user) { return _userRepository.SaveAppend(user); }
        public User? GetById(int id) { return _userRepository.GetById(id); }
        public User? Login(string usernameOrEmail, string password)
        {
            User? user = _userRepository.GetByUsername(usernameOrEmail);
            if (user == null) { user = _userRepository.GetByEmail(usernameOrEmail); }
            if (user == null) { MessageBox.Show("Wrong username/email!");  return null; }
            if (user.Password != password) { MessageBox.Show("Wrong password!");  return null; }

            _userRepository.Update(new User(user){ HasFirstLoginHappened = true});
            return user;
        }
        public void Update(User user) { _userRepository.Update(user); }
        public void RefreshSuper()
        {
            RefreshSuperOwner(_userRepository.GetAllByRole(UserRole.Owner));
        }

        public void RefreshSuperOwner(List<User>? owners)
        {
            if (owners == null) { return; }
            foreach (var owner in owners)
            {
                if (_ownerReviewRepository.CountByOwnerId(owner.Id) >= 50 &&
                    _ownerReviewRepository.AverageRatingByOwnerId(owner.Id) >= 4.5)
                {
                    owner.IsSuperUser = true;
                    _userRepository.Update(owner);
                }
                else
                {
                    owner.IsSuperUser = false;
                    _userRepository.Update(owner);
                }
            }
        }
        public void RefreshSuperGuest()
        {
            var guests = _userRepository.GetAllByRole(UserRole.Guest);
            foreach (var guest in guests)
            {
                var validReservations = _reservationRepository.GetValidReservationsByGuestId(guest.Id);
                if(validReservations == null) { continue; }
                if (validReservations.Count > 10 && !guest.IsSuperUser)
                {
                    guest.BecomeSuperGuest();
                    _userRepository.Update(guest);
                }
                else if (validReservations.Count <= 10 && guest.IsSuperUser)
                {
                    guest.LoseSuperGuest();
                    _userRepository.Update(guest);
                }
            }
        }
    }
}
