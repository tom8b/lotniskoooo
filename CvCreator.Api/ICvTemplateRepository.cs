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
        Task<UserRatesTemplate> GetUserRate(Guid templateId, string username);
        Task AddUserRate(Guid templateId, string username, int rate);
        Task UpdateUserRate(UserRatesTemplate userRatesTemplate);
        IEnumerable<Guid> GetUserRatesIds(string username);
    }
}