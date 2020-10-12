using CvCreator.Api.Model;
using System;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public interface ICvTemplateRepository
    {
        Task<int> Add(CvTemplateModel item);
        Task<CvTemplateModel> GetByIdAsync(Guid id);
    }
}