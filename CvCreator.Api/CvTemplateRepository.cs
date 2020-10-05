using CvCreator.Api.Model;
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

            try
            {
                return await dbContext.SaveChangesAsync();

            }
            catch (System.Exception e)
            {

                System.Console.WriteLine(e.Message);
                return 0;
            }
        }

    }
}
