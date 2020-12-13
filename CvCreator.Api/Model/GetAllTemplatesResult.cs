using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class GetAllTemplatesResult
    {
        public IEnumerable<Guid> Ids { get; set; }
    }

    public class GetAllTemplatesWithRatesResult
    {
        public List<SingleTemplate> Ids { get; set; }
    }

    public class SingleTemplate
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public int RatesCount { get; set; }
    }
}
