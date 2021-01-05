using CvCreator.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvCreator.Api
{
    public class CvTemplateRepository : ICvTemplateRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CvTemplateRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CvTemplateModel> Add(CvTemplateModel item)
        {
            try
            {
                var result = await dbContext.AddAsync(item);

                await dbContext.SaveChangesAsync();

                return result.Entity;
            }
            catch (Exception e)
            {

                throw;
            } 
        }

        public async Task<CvTemplateModel> GetByIdAsync(Guid id)
        {
            return await dbContext.CvTemplateModel
                .Include(x => x.Elements)
                    .ThenInclude(x => x.Size)
                .Include(x => x.Elements)
                    .ThenInclude(x => x.Position)
                .Include(x => x.Elements)
                    .ThenInclude(x => x.Content)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public IEnumerable<Guid> GetIds()
        {
            return dbContext.CvTemplateModel.Select(x => x.Id);
        }

        public IEnumerable<Guid> GetFilledTemplateIds(string authorName)
        {
            return dbContext.FilledTemplate.Where(x => x.UserId.Equals(authorName)).Select(x => x.Id);
        }

        public async Task<Guid> FillTemplate(FilledTemplate item)
        {
            var entity = await dbContext.AddAsync(item);

            await dbContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        public async Task<FilledTemplate> GetFilledTemplate(Guid id)
        {
            return await dbContext.FilledTemplate.Include(x => x.FilledElements).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<UserRatesTemplate> GetUserRate(Guid templateId, string username)
        {
            return await dbContext.UserRatesTemplate.FirstOrDefaultAsync(x => x.TemplateId.Equals(templateId) && x.Username.Equals(username));
        }

        public IEnumerable<Guid> GetUserRatesIds(string username)
        {
            return dbContext.UserRatesTemplate.Where(x => x.Username.Equals(username)).Select(x => x.TemplateId);
        }

        public List<(Guid templateId, int rate)> GetAuthorsRanking()
        {
            var res = dbContext.UserRatesTemplate.GroupBy(x => x.TemplateId, (templateId, items) => new { TemplateId = templateId, Sum = items.Sum(y => y.Rate) }).OrderByDescending(z => z.Sum);

            List<(Guid templateId, int rate)> result = new List<(Guid templateId, int rate)>();

            foreach(var x in res)
            {
                result.Add((x.TemplateId, x.Sum));
            }

            return result;
        }

        public string GetAuthorFor(Guid templateId)
        {
           var result = dbContext.CvTemplateModel.FirstOrDefault(x => x.Id.Equals(templateId))?.AuthorName;
            return result;
        }

        public (int, int) GetRateFor(Guid templateId) // zwraca rate oraz ilosc glosow
        {
            var rate = dbContext.UserRatesTemplate.Where(x => x.TemplateId.Equals(templateId)).Sum(x => x.Rate);
            var ratesCount = dbContext.UserRatesTemplate.Count(x => x.TemplateId.Equals(templateId));

            return (rate, ratesCount); 
        }

        public async Task AddUserRate(Guid templateId, string username, int rate)
        {
            await dbContext.AddAsync(new UserRatesTemplate { Rate = rate, TemplateId = templateId, Username = username });
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserRate(UserRatesTemplate userRatesTemplate)
        {
            dbContext.Update(userRatesTemplate);
            await dbContext.SaveChangesAsync();
        }
    }
}
