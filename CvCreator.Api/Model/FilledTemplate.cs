using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class FilledTemplate
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string UserId { get; set; }
        public ICollection<FilledElement> FilledElements { get; set; }
    }
}
