using System;
using System.Collections.Generic;

namespace CvCreator.Api.Model
{
    public class GetAllTemplatesResult
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
