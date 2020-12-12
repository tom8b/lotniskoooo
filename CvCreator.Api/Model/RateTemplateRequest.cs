using System;

namespace CvCreator.Api.Model
{
    public class RateTemplateRequest
    {
        public Guid Id { get; set; } // id Templatki
        public int Ocena { get; set; } // 1 - dobra - 0- zla
    }
}
