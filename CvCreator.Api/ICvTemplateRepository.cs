using CvCreator.Api.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public interface ICvTemplateRepository
    {
        Task<CvTemplateModel> Add(CvTemplateModel item);
        Task<CvTemplateModel> GetByIdAsync(Guid id);
        Task<Guid> FillTemplate(FilledTemplate item);
        Task<FilledTemplate> GetFilledTemplate(Guid id);
        IEnumerable<Guid> GetIds();
        IEnumerable<Guid> GetFilledTemplateIds(string authorName);
    }
}