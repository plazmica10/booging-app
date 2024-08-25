using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;

namespace BookingApp.DTO.Guide
{
    public class CreateTourRequestDto
    {
        public int GuideId { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public int Size { get; set; }
        public List<string> PeakPoints { get; set; }
        public List<DateTime> DatesAndTimes { get; set; }
        public int Duration { get; set; }
        public List<string> PictureIds { get; set; }

        public CreateTourRequestDto(int guideId, string name, Location location, string description, Language language, int size, List<string> peakPoints, List<DateTime> datesAndTimes, int duration, List<string> pictureIds)
        {
            GuideId = guideId;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            Size = size;
            PeakPoints = peakPoints;
            DatesAndTimes = datesAndTimes;
            Duration = duration;
            PictureIds = pictureIds;
        }
    }
}