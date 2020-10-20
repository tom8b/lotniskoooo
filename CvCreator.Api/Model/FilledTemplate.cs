using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class FilledTemplate
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<FilledElement> FilledElements { get; set; }
    }
}
