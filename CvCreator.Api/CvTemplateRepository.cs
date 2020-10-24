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

        public async Task<int> Add(CvTemplateModel item)
        {
            await dbContext.AddAsync(item);

            return await dbContext.SaveChangesAsync();
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
    }
}
