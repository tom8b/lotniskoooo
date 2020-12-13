using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class GetAuthorsRankingResult
    {
        public List<AuthorWithRates> AuthorWithRates { get; set; }
    }

    public class AuthorWithRates
    {
        public string Author { get; set; }
        public int Rate { get; set; }
    };
}
