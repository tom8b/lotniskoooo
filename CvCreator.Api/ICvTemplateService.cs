using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CvCreator.Api.Model;

namespace CvCreator.Api
{
    public interface ICvTemplateService
    {
        Task<CvTemplateModel> AddAsync(Template template, string authorName);
        Task<Template> GetByIdAsync(Guid id);
        Task<Guid> FillTemplate(Template template, string username);
        Task<(Template, Guid)> GetFilledTemplate(Guid filledTemplateId);
        IEnumerable<Guid> GetIds();
        IEnumerable<Guid> GetFilledTemplateIds(string authorName);
        Task RateTemplate(Guid templateId, string username, int rate);
        IEnumerable<Guid> GetIdsNotRatedBy(string username);
        Dictionary<string, int> GetAuthorsRanking();
        (int, int) GetRatesFor(Guid templateId);
    }
}