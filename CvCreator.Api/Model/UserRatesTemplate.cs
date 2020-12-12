using System;

namespace CvCreator.Api.Model
{
    public class UserRatesTemplate
    {
        public Guid Id { get; set; }
        public string Username { get; set; } // user ktory ocenia
        public int Rate { get; set; } // 0 lub 1 (0 to negatywna)
        public Guid TemplateId { get; set; }
    }
}
