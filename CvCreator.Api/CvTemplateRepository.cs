using CvCreator.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<int> FillTemplate(FilledTemplate item)
        {
            await dbContext.AddAsync(item);

            return await dbContext.SaveChangesAsync();
        }

        public async Task<FilledTemplate> GetFilledTemplate(Guid id)
        {
            return await dbContext.FilledTemplate.Include(x => x.FilledElements).FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
