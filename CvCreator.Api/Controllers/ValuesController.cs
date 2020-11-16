using System;
using System.Collections.Generic;
using System.IO;
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
            var template = await cvTemplateService.GetFilledTemplate(Guid.Parse("ddf84c9e-7666-4b05-ea64-08d8803a1df7"));

            var content = string.Empty;

            foreach (var element in template.Elements)
            {
                var ElemenetStyleBuilder = new ElementStyleBuilder(element.Position.X, element.Position.Y, element.Size.X, element.Size.Y);
                var style = ElemenetStyleBuilder.WithBackgroundColor("red").Build();

                var htmlElement = new HtmlElement(style, element.Content.Text);
                content += htmlElement.GetElement();
            }
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath.Replace("\\", "/"), "StaticFiles");
            //var mainContent = $"<img width=595 height=842 src=\"file:///{uploads}/837e7244-857e-4ade-d4e8-08d880388689/TLO_CV.jpg\" /><div>{content}</div>";
            var mainContent = $"<img style=\"position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle \" width=594 height=840 src=\"file:///{uploads}/9b7588a1-e7e7-4a43-cd05-08d87e93d78d/TLO_CV.jpg\" /><div>{content}</div>";

            var report = await jsReportMVCService.RenderAsync(new RenderRequest()
            {
                Template = new jsreport.Types.Template
                {
                    Content = mainContent,
                    Engine = Engine.None,
                    Recipe = Recipe.ChromePdf,
                    Chrome = new Chrome
                    {
                        Width="595", Height="842",
                        MarginRight="0px",
                        MarginLeft="0px",
                        MarginTop="0px",
                        MarginBottom="0px"                     
                    }
                }
            });


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
        public async Task<ActionResult<Model.Template>> Get([FromBody] GetTemplateRequest request)
        {
            var result = await cvTemplateService.GetByIdAsync(Guid.Parse(request.Id));
            result.BackgroundUrl = "https://localhost:44371/StaticFiles/" + result.Id.ToString() + "/backgroundImage.jpg";
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
            foreach (var item in form.Files)
            {
            string authorName = User.Identity.Name;

            }
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
            string filePath = Path.Combine(folderPath, "backgroundImage.jpg");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await form.Files["backgroundImage"].CopyToAsync(fileStream);
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
