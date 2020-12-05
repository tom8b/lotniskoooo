using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CvCreator.Api.JsReport;
using CvCreator.Api.Model;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CvCreator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICvTemplateService cvTemplateService;
        private readonly IJsReportMVCService jsReportMVCService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ValuesController(UserManager<ApplicationUser> userManager, ICvTemplateService cvTemplateService, IJsReportMVCService jsReportMVCService, IHostingEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.cvTemplateService = cvTemplateService;
            this.jsReportMVCService = jsReportMVCService;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var filledTemplateId = Guid.Parse("64b573ff-7526-49ae-8ee8-08d8993d37fa");
            var templateId = Guid.Parse("0bc6ba07-d534-4d2c-4bc2-08d8993c918f");

            var template = await cvTemplateService.GetFilledTemplate(filledTemplateId);

            var content = string.Empty;
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath.Replace("\\", "/"), "StaticFiles");

            foreach (var element in template.Elements)
            {
                var ElemenetStyleBuilder = new ElementStyleBuilder(element.Position.X, element.Position.Y, element.Size.X, element.Size.Y);
                var style = ElemenetStyleBuilder.WithZIndex(2).WithBackgroundColor("red").Build();
                var imagePath = $"file:///{uploads}/{templateId}/{element.Id}.jpg";
                var htmlElement = new HtmlElement(style, element.Content.Text, imagePath);
                content += htmlElement.GetElement();
            }

            var mainContent = $"<img style=\"; position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle \" width=594 height=840 src=\"file:///{uploads}/{templateId}/backgroundImage.jpg\" /><div>{content}</div>";
            Report report;
            try
            {
                report = await jsReportMVCService.RenderAsync(new RenderRequest()
                {
                    Template = new jsreport.Types.Template
                    {
                        Content = mainContent,
                        Engine = Engine.None,
                        Recipe = Recipe.ChromePdf,
                        Chrome = new Chrome
                        {
                            Width = "595",
                            Height = "842",
                            MarginRight = "0px",
                            MarginLeft = "0px",
                            MarginTop = "0px",
                            MarginBottom = "0px"
                        }
                    }
                });
            }
            catch (Exception e)
            {

                throw;
            }

          


            using (var file = System.IO.File.Open("report.pdf", FileMode.Create))
            {
                report.Content.CopyTo(file);
            }
            report.Content.Seek(0, SeekOrigin.Begin);

            return new string[] { "value1", "value2" };
        }

       // [Authorize]
        // GET api/values/template
        [HttpPost("template")]
        public async Task<ActionResult<GetTemplateResult>> Get([FromBody] GetTemplateRequest request)
        {
            var template = await cvTemplateService.GetByIdAsync(Guid.Parse(request.Id));
            var result = new GetTemplateResult();
            result.BackgroundUrl = "https://localhost:44371/StaticFiles/" + template.Id.ToString() + "/backgroundImage.jpg";
            result.Elements = new List<ElementWithImg>();
            foreach(var element in template.Elements)
            {
                var elementWithImg = new ElementWithImg();
                elementWithImg.Content = element.Content;
                elementWithImg.ElementKey = element.ElementKey;
                elementWithImg.Id = element.Id;
                elementWithImg.Image = "https://localhost:44371/StaticFiles/" + template.Id.ToString() + "/" + element.Id + ".jpg";
                elementWithImg.Position = element.Position;
                elementWithImg.Size = element.Size;
                elementWithImg.UserFillsOut = element.UserFillsOut;
            
                result.Elements.Add(elementWithImg);
              
           
            }

            result.Id = template.Id;
            return result;
        }

        [HttpPost("getFilledTemplate")]
        public async Task<ActionResult<Model.Template>> GetFilledTemplate([FromBody] GetTemplateRequest request)
        {
            var result = await cvTemplateService.GetFilledTemplate(Guid.Parse(request.Id));
            return result;
        }

        [HttpPost("fillTemplate")]
        public async Task<ActionResult<Model.Template>> Post([FromBody] FillTemplateRequest request)
        {
            string username = User.Identity.Name;
            await cvTemplateService.FillTemplate(new Model.Template { Elements = request.Elements, Id = request.Id }, username);

            return new Model.Template();
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] CreateTemplateRequest request)
        {
            string authorName = User.Identity.Name;
            await cvTemplateService.AddAsync(request.Template, authorName);
        }

        [HttpPost("file")]
        public async Task PostImages([FromForm]IFormCollection form)
        {
            var request = form["request"].ToString();

            CreateTemplateRequest obj;
            try
            {
                 obj = JsonConvert.DeserializeObject<CreateTemplateRequest>(request);
            }
            catch (Exception e)
            {

                throw;
            }
         
            var entity = await cvTemplateService.AddAsync(obj.Template, User.Identity.Name);

            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles");
            string folderPath = Path.Combine(uploads, entity.Id.ToString());
            string backgroundPath = Path.Combine(folderPath, "backgroundImage.jpg");
            Directory.CreateDirectory(Path.GetDirectoryName(backgroundPath));

            var keys = form.Files.Select(x => x.Name).Where(x => x != "backgroundImage");


            using (Stream fileStream = new FileStream(backgroundPath, FileMode.Create))
            {
                await form.Files["backgroundImage"].CopyToAsync(fileStream);
            }

            foreach (var picture in keys)
            {
                var imagePath = Path.Combine(folderPath, $"{entity.Elements.First(x => x.ElementKey.ToString().Equals(picture)).Id}.jpg");
                using (Stream fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await form.Files[picture].CopyToAsync(fileStream);
                }
            }
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
