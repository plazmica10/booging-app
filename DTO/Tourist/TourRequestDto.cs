using BookingApp.Domain.Model;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;

namespace BookingApp.DTO.Tourist
{
    public class TourRequestDto
    {
        public int TouristId { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public DateTime SpanStart { get; set; }
        public DateTime SpanEnd { get; set; }
        public List<ReservationDto> Reservations { get; set; } = new();

        public TourRequestDto(int touristId, string name, Location location, string description, Language language, DateTime spanStart, DateTime spanEnd, List<ItemReservationViewModel> reservations)
        {
            TouristId = touristId;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            SpanStart = spanStart;
            SpanEnd = spanEnd;
            foreach (var reservationViewModel in reservations)
            {
                Reservations.Add(new ReservationDto(reservationViewModel));
            }
        }
    }
}