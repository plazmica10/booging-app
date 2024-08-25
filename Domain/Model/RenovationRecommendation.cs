using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class RenovationRecommendation : ISerializable
    { 
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int Urgency { get; set; }
        
        public RenovationRecommendation() { }

        public string GetHeader() { return "Id|ReservationId|ReviewId|Comment|Urgency"; }

        public RenovationRecommendation(int id, int reservationId, int reviewId, string comment, int urgency)
        {
            Id = id;
            ReservationId = reservationId;
            ReviewId = reviewId;
            Comment = comment;
            Urgency = urgency;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), ReservationId.ToString(), ReviewId.ToString(), Comment, Urgency.ToString()
            };
            return csvValues;
        }

        public void FromCsv(string[] csvValues)
        {
            int i = 0;
            Id = int.Parse(csvValues[i++]);
            ReservationId = int.Parse(csvValues[i++]);
            ReviewId = int.Parse(csvValues[i++]);
            Comment = csvValues[i++];
            Urgency = int.Parse(csvValues[i++]);
        }


    }


}
