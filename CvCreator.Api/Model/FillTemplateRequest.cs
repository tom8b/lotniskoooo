using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class FillTemplateRequest
    {
        public Guid Id { get; set; }
        public IEnumerable<Element> Elements { get; set; }
    }
}
