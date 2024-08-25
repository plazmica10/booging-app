using BookingApp.Serializer;
using System;

namespace BookingApp.Domain.Model
{
    public record Location : ISerializable
    {
        public int Id { get; set; } = -1;
        public string GeoId { get; set; } = "";
        public string Country { get; set; } = "";
        public string SubCountry { get; set; } = "";
        public string City { get; set; } = "";
        public string GetHeader() => "Id|GeoId|Country|SubCountry|City";
        
        public Location() { }

        public Location(int id, string geoId, string country, string subCountry, string city)
        {
            Id = id;
            GeoId = geoId;
            Country = country;
            SubCountry = subCountry;
            City = city;
        }

        public string[] ToCsv() => new string[] { $"{Id}", GeoId, Country, SubCountry, City };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            GeoId = values[i++];
            Country = values[i++];
            SubCountry = values[i++];
            City = values[i++];
        }

        public static Location Parse(string value)
        {
            string[] values = value.Split(";");
            int i = 0;
            return new Location(
                Convert.ToInt32(values[i++]),
                values[i++],
                values[i++],
                values[i++],
                values[i++]
            );
        }

        public override string ToString() { return $"{Id};{GeoId};{Country};{SubCountry};{City}"; }
        public string ToReadableString() { return $"{Country}, {SubCountry}, {City}"; }
        public string ToSmallString() { return $"{Country}, {City}"; }
    }
}
