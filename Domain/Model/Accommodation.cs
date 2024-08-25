using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BookingApp.Serializer;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BookingApp.Domain.Model
{
    public enum AccommodationType
    {
        Cabin,
        Apartment,
        House
    }

    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDays { get; set; }
        public int RefundPeriod { get; set; }
        public List<string> Photos { get; set; }
        public int OwnerId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public Accommodation() { }

        public string GetHeader() { return "Id|Name|Country|City|Location|Type|MaxGuests|MinDaysForReservation|RefundPeriod|Photos|OwnerId|Description|IsDeleted"; }

        public Accommodation(int id, string name, string country, string city, Location location, AccommodationType type, int maxGuests,
            int minDays, int refundPeriod, List<string> photos, int ownerId,string description)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            Location = location;
            Type = type;
            MaxGuests = maxGuests;
            MinDays = minDays;
            RefundPeriod = refundPeriod;
            Photos = photos;
            OwnerId = ownerId;
            Description = description;
            IsDeleted = false;
        }

        public string[] ToCsv()
        {
            string[] csvValues =
            {
                Id.ToString(), Name, Country, City, $"{Location}", Type.ToString(), MaxGuests.ToString(), MinDays.ToString(),
                RefundPeriod.ToString(), 
                string.Join(",",Photos), OwnerId.ToString(),Description, IsDeleted.ToString() };
            return csvValues;
        }

        public void FromCsv(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Country = values[2];
            City = values[3];
            Location = Location.Parse(values[4]);
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[5]);
            MaxGuests = Convert.ToInt32(values[6]);
            MinDays = Convert.ToInt32(values[7]);
            RefundPeriod = Convert.ToInt32(values[8]);
            Photos = values[9].Split(",").ToList();
            OwnerId = Convert.ToInt32(values[10]);
            Description = values[11];
            IsDeleted = Convert.ToBoolean(values[12]);
        }
    }
}
