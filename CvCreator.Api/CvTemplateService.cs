using CvCreator.Api.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public class CvTemplateService : ICvTemplateService
    {
        private readonly ICvTemplateRepository templateRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CvTemplateService(ICvTemplateRepository templateRepository, UserManager<ApplicationUser> userManager)
        {
            this.templateRepository = templateRepository;
            this.userManager = userManager;
        }

        public async Task<CvTemplateModel> AddAsync(Template template, string authorName)
        {
            var entity = new CvTemplateModel
            {
                BackgroundUrl = template.BackgroundUrl,
                AuthorName = authorName,
                Elements = template.Elements,
            };

            return await templateRepository.Add(entity);
        }

        public async Task<Template> GetByIdAsync(Guid id)
        {
            var item = await templateRepository.GetByIdAsync(id);

            return new Template
            {
                Id = item.Id,
                BackgroundUrl = item.BackgroundUrl,
                Elements = item.Elements
            };
        }

        public IEnumerable<Guid> GetIds()
        {
            return templateRepository.GetIds();
        }

        public IEnumerable<Guid> GetFilledTemplateIds(string authorName)
        {
            return templateRepository.GetFilledTemplateIds(authorName);
        }

        public async Task<Guid> FillTemplate(Template template, string username)
        {
            var entity = new FilledTemplate
            {
                FilledElements = template.Elements
                .Where(x => x.UserFillsOut == true)
                .Select(x => new FilledElement { FilledText = x.Content.Text, ElementId = x.Id}).ToList(),
                TemplateId = template.Id,
                UserId = username
            };
            try
            {
                var filledTemplateId = await templateRepository.FillTemplate(entity);
                return filledTemplateId;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //zwraca template i guida TemplateId!
        public async Task<(Template, Guid)> GetFilledTemplate(Guid filledTemplateId)
        {
            var filledTemplate = await templateRepository.GetFilledTemplate(filledTemplateId);
            var template = await templateRepository.GetByIdAsync(filledTemplate.TemplateId);
            var mappedFilledElements = filledTemplate.FilledElements.Select(x => new Element
            {
                Id = x.ElementId,
                Content = new Content
                {
                    Text = x.FilledText
                },
                Position = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Position,
                Size = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Size,
                UserFillsOut = true
            });
            var elements = mappedFilledElements.Concat(template.Elements.Where(x => x.UserFillsOut == false));

            return (new Template
            {
                BackgroundUrl = template.BackgroundUrl,
                Id = filledTemplate.Id,
                Elements = elements
            }, filledTemplate.TemplateId);
        }
    }
}
