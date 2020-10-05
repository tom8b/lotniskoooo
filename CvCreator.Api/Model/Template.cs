using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class Template
    {
        public string BackgroundUrl { get; set; }
        public IEnumerable<Element> Elements { get; set; }
    }
}
