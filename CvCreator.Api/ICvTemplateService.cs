using System.Threading.Tasks;
using CvCreator.Api.Model;

namespace CvCreator.Api
{
    public interface ICvTemplateService
    {
        Task<bool> AddAsync(Template template, string authorName);
    }
}