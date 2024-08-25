using System;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class TourVoucher : ISerializable
    {
        public int Id { get; set; } = -1;
        public int TouristId { get; set; } = -1;
        public int Index { get; set; } = -1;
        public string Hash { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.Now;
        public DateTime Expiration { get; set; } = DateTime.Now.AddMonths(6);
        public string GetHeader() => "Id|Hash|TouristId|Index|ReceivedAt|Expiration";

        private string GenerateHash()
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        public TourVoucher()
        {
            Hash = GenerateHash();
        }
        public TourVoucher(int touristId)
        {
            Hash = GenerateHash();
            TouristId = touristId;
        }
        public TourVoucher(int touristId, DateTime expiration)
        {
            Hash = GenerateHash();
            TouristId = touristId; Expiration = expiration;
        }

        public string[] ToCsv() => new[] {  $"{Id}", $"{Hash}", $"{TouristId}", $"{Index}", $"{ReceivedAt:yyyy-MM-dd HH:mm:ss}", $"{Expiration:yyyy-MM-dd HH:mm:ss}" };

        public void FromCsv(string[] values)
        {
            int i = 0;
            Id = Convert.ToInt32(values[i++]);
            Hash = values[i++];
            TouristId = Convert.ToInt32(values[i++]);
            Index = Convert.ToInt32(values[i++]);
            ReceivedAt = DateTime.ParseExact(values[i++], "yyyy-MM-dd HH:mm:ss", null);
            Expiration = DateTime.ParseExact(values[i++], "yyyy-MM-dd HH:mm:ss", null);
        }

        public override string ToString() => $"{Id}|{Hash}|{TouristId}|{Index}|{ReceivedAt:yyyy-MM-dd HH:mm:ss}|{Expiration:yyyy-MM-dd HH:mm:ss}|";
    }
}