using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;

namespace BookingApp.Domain.IRepository
{
    internal interface IRenovationRecommendationRepository
    {
        public List<RenovationRecommendation> GetAll();
        public RenovationRecommendation Save(RenovationRecommendation recommendation);
    }
}
