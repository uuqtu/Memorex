using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorex.Model
{
    public class KnoledgeElement
    {
        public string Link { get; set; }
        public string Searchwords { get; set; }

        public string Category { get; set; }
        public string Id { get; set; }

        public int MatchingScore { get; private set; }
        public KnoledgeElement(string key, string value, string id, string category)
        {
            Link = key;
            Searchwords = value;
            Id = id;
            Category = category;
        }

        public void CalculateMatchingScore(string search)
        {
            MatchingScore = LevenshteinDistance.Compute(search, this.Searchwords);
        }
    }
}
