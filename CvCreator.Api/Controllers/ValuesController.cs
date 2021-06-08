using System.Collections.Generic;
using System.Threading.Tasks;
using CvCreator.Api.Model;
using CvCreator.Api.Requests;
using CvCreator.Api.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CvCreator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IInMemoryStorage _inMemoryStorage;

        public ValuesController(UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IInMemoryStorage inMemoryStorage)
        {
            this.userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _inMemoryStorage = inMemoryStorage;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //await GenerateTemplatePicture(Guid.Parse("9ce2d491-1bdd-4621-aee4-08d8be4090cb"));
            return new string[] { "value1", "value2" };
        }

        // [Authorize]
        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest loginRequest)
        {
            return new LoginResponse
            {
                Admin_Permission = true,
                Client_ID = 1,
                Email = "asddas",
                Login = "loginnn",
                Name = " nameee",
                Password = "passwrd",
                Surname = "surname"
            };
        }

        [HttpPost("addArticle")]
        public void AddArticle(AddArticleRequest request)
        {
            var article = new Article
            {
                Author = request.Author, Content = request.Content, Description = request.Description,
                Title = request.Title
            };

            _inMemoryStorage.AddArticle(article);
        }

        [HttpGet("allArticles")]
        public List<Article> GetAllArticles()
        {
            return _inMemoryStorage.GetArticles;
        }
    }
}
