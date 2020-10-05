using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class CvTemplateModel
    {
        public Guid Id { get; set; }
        public string AuthorName { get; set; }
        public string BackgroundUrl { get; set; }
        public IEnumerable<Element> Elements { get; set; }
    }
}
