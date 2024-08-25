using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO.Guest
{
    public class AccommodationsSearchDto
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string  City { get; set; }
        public string  Country { get; set; }
        public  string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public  string Image { get; set; }

        public AccommodationsSearchDto(){}

        public AccommodationsSearchDto(int id,string name, string city, string country, DateTime checkIn, DateTime checkOut,
            string image)
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
            CheckIn = checkIn.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            CheckOut = checkOut.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            Image = image;
        }
    }
}
