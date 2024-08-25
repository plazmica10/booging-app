using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.Utilities;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/og_reservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCsv(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public List<AccommodationReservation> GetForOwner(int ownerId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.OwnerId == ownerId);
        }

        public List<AccommodationReservation> GetPastReservationsByUserId(int userId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.GuestId == userId && c.CheckOut < DateTime.Now);
        }

        public List<AccommodationReservation> GetForPeriodByUserId(int userId, bool past)
        {
            if (past) return GetPastReservationsByUserId(userId);
            return _serializer.FromCsv(FilePath).FindAll(c => c.GuestId == userId && c.CheckOut >= DateTime.Now && ((c.Status == ReservationStatus.Reserved) || (c.Status == ReservationStatus.InProgress)));
        }
        public List<AccommodationReservation> GetByUserId(int userId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.GuestId == userId);
        }

        public AccommodationReservation GetById(int id)
        {
            return _serializer.FromCsv(FilePath).Find(c => c.Id == id) ?? throw new InvalidOperationException();
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCsv(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCsv(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCsv(FilePath);
            return _accommodationReservations.Count < 1 ? 1 : _accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCsv(FilePath);
            AccommodationReservation found = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            _accommodationReservations.Remove(found);
            _serializer.ToCsv(FilePath, _accommodationReservations);
        }
        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            _accommodationReservations = _serializer.FromCsv(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, accommodationReservation);
            _serializer.ToCsv(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public List<AccommodationReservation> GetByAccommodationId(int accommodationId)
        {
            return _serializer.FromCsv(FilePath).FindAll(c => c.AccommodationId == accommodationId);
        }

        public List<Tuple<DateTime, DateTime>> GetAvailableDates(int accommodationId, DateTime startDate, DateTime endDate, int stayLength)
        {
            _accommodationReservations = GetByAccommodationId(accommodationId);
            var extraDays = stayLength*2;
            var extendedStartDate = startDate.AddDays(-extraDays);
            var extendedEndDate = endDate.AddDays(extraDays);

            var allDates = AccommodationUtils.GetAllDates(extendedStartDate, extendedEndDate);
            var reservedDates = AccommodationUtils.GetReservedDates(_accommodationReservations);
            var availableDates = allDates.Except(reservedDates).ToList();
            var possibleStays = AccommodationUtils.GetPossibleStays(availableDates, stayLength);

            // filter out stays that are not within the original date range
            possibleStays = possibleStays.Where(stay => stay.Item1 >= startDate && stay.Item2 <= endDate).ToList();

            // search in the extended date range if no possible stays are found
            if (!possibleStays.Any())
            {
                possibleStays = AccommodationUtils.GetPossibleStays(availableDates, stayLength);
                possibleStays = possibleStays.Where(stay => stay.Item1 >= DateTime.Now).ToList();
            }

            return possibleStays;
        }

        public List<AccommodationReservation> GetValidReservationsByGuestId(int guestId)
        {
            var oneYearAgo = System.DateTime.Now.AddYears(-1);
            return _serializer.FromCsv(FilePath).FindAll(c => c.GuestId == guestId && c.CheckIn >= oneYearAgo && c.Status == ReservationStatus.Reserved);
        }
    }
}
