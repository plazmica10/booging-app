using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepository;
using BookingApp.Domain.Model;
using BookingApp.Serializer;

namespace BookingApp.Repository
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/og_renovation-rec.csv";
        private readonly Serializer<RenovationRecommendation> _serializer = new();

        public List<RenovationRecommendation> GetAll()
        {
            return _serializer.FromCsv(FilePath);
        }

        public RenovationRecommendation Save(RenovationRecommendation recommendation)
        {
            var recommendations = _serializer.FromCsv(FilePath);
            recommendation.Id = recommendations.Count < 1 ? 1 : recommendations.Max(r => r.Id) + 1;
            recommendations.Add(recommendation);
            _serializer.ToCsv(FilePath, recommendations);
            return recommendation;
        }
    }
}
