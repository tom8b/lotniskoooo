using System.Collections.Generic;
using System.Threading.Tasks;
using CvCreator.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CvCreator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICvTemplateService cvTemplateService;

        public ValuesController(UserManager<ApplicationUser> userManager, ICvTemplateService cvTemplateService)
        {
            this.userManager = userManager;
            this.cvTemplateService = cvTemplateService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize]
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return User.Identity.Name;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] CreateTemplateRequest request)
        {
            string authorName = User.Identity.Name;
            await cvTemplateService.AddAsync(request.Template, authorName);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
