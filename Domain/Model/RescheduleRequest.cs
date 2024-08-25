using System;
using System.Globalization;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Denied
    }

    public class RescheduleRequest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
        public int AccommodationId { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public string OwnersComment { get; set; }
        public RequestStatus Status { get; set; }
        
        public string GetHeader() { return "Id|GuestId|OwnerId|ReservationId|AccommodationId|NewStartDate|NewEndDate|OwnersComment|Status"; }

        public RescheduleRequest() { }

        public RescheduleRequest(int id,int userId, int ownerId, int reservationId, int accommodationId, DateTime newStartDate, DateTime newEndDate,
            string ownersComment, RequestStatus status)
        {
            Id = id;
            UserId = userId;
            OwnerId = ownerId;
            ReservationId = reservationId;
            AccommodationId = accommodationId;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            OwnersComment = ownersComment;
            Status = status;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                OwnerId.ToString(),
                ReservationId.ToString(),
                AccommodationId.ToString(),
                NewStartDate.ToString(CultureInfo.InvariantCulture),
                NewEndDate.ToString(CultureInfo.InvariantCulture),
                OwnersComment,
                Status.ToString()
            };
            return csvValues;
        }

        public void FromCsv(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            OwnerId = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
            AccommodationId = Convert.ToInt32(values[4]);
            NewStartDate = DateTime.ParseExact(values[5], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            NewEndDate = DateTime.ParseExact(values[6], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            OwnersComment = values[7];
            Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), values[8]);
        }
    }
}
