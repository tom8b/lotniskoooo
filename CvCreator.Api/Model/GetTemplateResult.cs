using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class GetTemplateResult
    {
        public Guid Id { get; set; }
        public string BackgroundUrl { get; set; }
        public List<ElementWithImg> Elements { get; set; }
    }

    public class ElementWithImg
    {
        public Guid Id { get; set; }
        public int ElementKey { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
        public Content Content { get; set; }
        public bool UserFillsOut { get; set; }
        public string Image { get; set; }
    }
}
