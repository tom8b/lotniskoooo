using CvCreator.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public class CvTemplateService : ICvTemplateService
    {
        private readonly ICvTemplateRepository templateRepository;

        public CvTemplateService(ICvTemplateRepository templateRepository)
        {
            this.templateRepository = templateRepository;
        }

        public async Task<bool> AddAsync(Template template, string authorName)
        {
            var entity = new CvTemplateModel
            {
                BackgroundUrl = template.BackgroundUrl,
                AuthorName = authorName,
                Elements = template.Elements,
            };

            return await templateRepository.Add(entity) > 0 ? true : false;
        }
    }
}
