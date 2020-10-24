using System;

namespace CvCreator.Api.Model
{
    public class FilledElement
    {
        public Guid Id { get; set; }
        public Guid ElementId { get; set; }
        public string FilledText { get; set; }
    }
}
