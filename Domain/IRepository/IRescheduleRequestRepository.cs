using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    public interface IRescheduleRequestRepository
    {
        public List<RescheduleRequest> GetAll();
        public RescheduleRequest GetById(int id);
        public List<RescheduleRequest> GetByUserId(int userId);
        public List<RescheduleRequest> GetByOwnerId(int ownerId);
        public List<RescheduleRequest> GetByAccommodationId(int accommodationId);
        public RescheduleRequest Save(RescheduleRequest rescheduleRequest);
        public void Update(RescheduleRequest rescheduleRequest);
        public int NextId();
        void DeleteByReservation(int reservationId);
    }
}
