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

        public IEnumerable<Guid> GetIdsNotRatedBy(string username)
        {
            var allIds = templateRepository.GetIds();
            var userRates = templateRepository.GetUserRatesIds(username);

            return allIds.Where(x => !userRates.Contains(x));
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
                .Select(x => new FilledElement { FilledText = x.Content.Text, ElementId = x.Id, IsProfilePicture = x.IsProfilePicture}).ToList(),
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

        public async Task RateTemplate(Guid templateId, string username, int rate)
        {
            int rateChecked;
            if (rate == 0)
            {
                rateChecked = -1;
            }
            else
            {
                rateChecked = 1;
            }

            var userRatesTemplate = await templateRepository.GetUserRate(templateId, username);

            if(userRatesTemplate == null)
            {
                await templateRepository.AddUserRate(templateId, username, rate);
            }
            else
            {
                userRatesTemplate.Rate = rateChecked;
                await templateRepository.UpdateUserRate(userRatesTemplate);
            }
        }

        public (int, int) GetRatesFor(Guid templateId)
        {
            return templateRepository.GetRateFor(templateId);
        }

        public string GetAuthorFor(Guid templateId)
        {
            return templateRepository.GetAuthorFor(templateId);
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
                    Text = x.FilledText,
                    FontSize = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Content.FontSize,
                    ZIndex = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Content.ZIndex
                },
                Position = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Position,
                Size = template.Elements.FirstOrDefault(y => y.Id.Equals(x.ElementId)).Size,
                UserFillsOut = true,
                IsProfilePicture = x.IsProfilePicture
            });
            var elements = mappedFilledElements.Concat(template.Elements.Where(x => x.UserFillsOut == false));

            return (new Template
            {
                BackgroundUrl = template.BackgroundUrl,
                Id = filledTemplate.Id,
                Elements = elements
            }, filledTemplate.TemplateId);
        }

        public Dictionary<string, int> GetAuthorsRanking()
        {
            var cvs = templateRepository.GetAuthorsRanking();
            var dictionary = new Dictionary<string, int>();
            foreach(var x in cvs)
            {
                var authorName = templateRepository.GetAuthorFor(x.templateId);
                var authorNameNotNull = authorName == null ? " " : authorName;
                if(dictionary.Keys.Contains(authorNameNotNull))
                {
                    dictionary[authorNameNotNull] += x.rate;
                }
                else
                {
                    dictionary.Add(authorNameNotNull, x.rate);
                }
            }

            return dictionary;
        }
    }
}
