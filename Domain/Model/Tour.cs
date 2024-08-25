using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public enum TourStatus { NotStarted, Started, Finished, Canceled }
    public class Tour : ISerializable
    {
        public int Id { get; set; } = -1;
        public int GuideId { get; set; } = -1;
        public string Name { get; set; } = "";
        public Location Location { get; set; } = new();
        public string Description { get; set; } = "";
        public Language Language { get; set; } = new();
        public int MaxTourists { get; set; } = -1;
        public int SpaceLeft { get; set; } = -1;
        public List<string> PeakPoints { get; set; } = new() { "tour start", "tour end" };
        public DateTime StartsAt { get; set; } = DateTime.Now;
        public int Duration { get; set; } = -1;
        public List<string> PictureIds { get; set; } = new();
        public TourStatus Status { get; set; } = TourStatus.NotStarted;
        public int CurrentPeakPointIndex { get; set; } = -1;

        public string GetHeader() => "Id|GuideId|Name|Location|Description|Language|MaxTourists|SpaceLeft|PeakPoints|StartsAt|Duration|PictureIds|Status|CurrentPeakPointIndex";

        public Tour(int guideId, string name, Location location, string description, Language language, int size, List<string> peakPoints, int duration, List<string> pictureIds)
        {
            GuideId = guideId;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxTourists = size;
            SpaceLeft = size;
            PeakPoints = peakPoints;
            Duration = duration;
            PictureIds = pictureIds;
        }

        public Tour(int guideId, string name, Location location, string description, Language language, int peopleCount, DateTime startsAt, int duration)
        {
            GuideId = guideId;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxTourists = peopleCount;
            SpaceLeft = 0;
            StartsAt = startsAt;
            Duration = duration;
        }

        public Tour() { }

        public string[] ToCsv() => new[] { $"{Id}", $"{GuideId}", $"{Name}", $"{Location}", $"{Description}", $"{Language}", $"{MaxTourists}", $"{SpaceLeft}", $"{string.Join(",", PeakPoints)}", StartsAt.ToString("yyyy-MM-dd HH:mm:ss"), $"{Duration}", $"{string.Join(",", PictureIds)}", $"{Status}", $"{CurrentPeakPointIndex}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            GuideId = Convert.ToInt32(values[i++]);
            Name = values[i++];
            Location = Location.Parse(values[i++]);
            Description = values[i++];
            Language = Language.Parse(values[i++]);
            MaxTourists = Convert.ToInt32(values[i++]);
            SpaceLeft = Convert.ToInt32(values[i++]);
            PeakPoints = values[i++].Split(",").ToList();
            StartsAt = DateTime.ParseExact(values[i++], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Duration = Convert.ToInt32(values[i++]);
            PictureIds = values[i++].Split(",").ToList();
            Status = (TourStatus)Enum.Parse(typeof(TourStatus), values[i++]);
            CurrentPeakPointIndex = Convert.ToInt32(values[i++]);
        }

        public override string ToString() => $"{Id}|{GuideId}|{Name}|{Location}|{Description}|{Language}|{MaxTourists}|{SpaceLeft}" +
                                             $"|{string.Join(",", PeakPoints)}|{StartsAt:yyyy-MM-dd HH:mm:ss}|{Duration}|{string.Join(",", PictureIds)}|{Status}|{CurrentPeakPointIndex}";
    }
}