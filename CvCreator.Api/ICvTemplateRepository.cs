using CvCreator.Api.Model;
using System;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public interface ICvTemplateRepository
    {
        Task<CvTemplateModel> Add(CvTemplateModel item);
        Task<CvTemplateModel> GetByIdAsync(Guid id);
        Task<int> FillTemplate(FilledTemplate item);
        Task<FilledTemplate> GetFilledTemplate(Guid id);
    }
}