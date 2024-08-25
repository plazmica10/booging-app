using BookingApp.Serializer;
using System;

namespace BookingApp.Domain.Model
{
    public record Language : ISerializable
    {
        public int Id { get; set; } = -1;
        public string Id6391 { get; set; } = "";
        public string Id6392T { get; set; } = "";
        public string Id6392B { get; set; } = "";
        public string Name { get; set; } = "";
        public string Native { get; set; } = "";

        public string GetHeader() => "Id|639-1|639-2/T|639-2/B|Name|Native";

        public Language() { }

        public Language(int id, string id6391, string id6392T, string id6392B, string name, string native)
        {
            Id = id;
            Id6391 = id6391;
            Id6392T = id6392T;
            Id6392B = id6392B;
            Name = name;
            Native = native;
        }

        public string[] ToCsv() => new string[] { $"{Id}", Id6391, Id6392T, Id6392B, Name, Native };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            Id6391 = values[i++];
            Id6392T = values[i++];
            Id6392B = values[i++];
            Name = values[i++];
            Native = values[i++];
        }

        public static Language Parse(string value)
        {
            string[] values = value.Split(";");
            int i = 0;
            return new Language(
                Convert.ToInt32(values[i++]),
                values[i++],
                values[i++],
                values[i++],
                values[i++],
                values[i++]
            );
        }

        public string ToReadableString() => $"{Name}, {Native}";
        public override string ToString() => $"{Id};{Id6391};{Id6392T};{Id6392B};{Name};{Native}";
    }
}
