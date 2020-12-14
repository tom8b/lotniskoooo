using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreHtmlToImage;
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
            return new string[] { "value1", "value2" };
        }

        private async Task GeneratePdfAsync(Guid filledTemplateId)
        {

            var (template, templateId) = await cvTemplateService.GetFilledTemplate(filledTemplateId);

            var content = string.Empty;
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath.Replace("\\", "/"), "StaticFiles");

            foreach (var element in template.Elements.OrderBy(x => x.Content.ZIndex))
            {
                var ElemenetStyleBuilder = new ElementStyleBuilder(element.Position.X, element.Position.Y, element.Size.X, element.Size.Y);
                var style = ElemenetStyleBuilder.WithZIndex(0).WithFontSize(element.Content.FontSize).WithBackgroundColor("red").Build();
                var imagePath = element.IsProfilePicture ? $"file:///{uploads}/FilledTemplate/{filledTemplateId.ToString()}/profilePicture.jpg" :  $"file:///{uploads}/{templateId}/{element.Id}.jpg";
                var htmlElement = new HtmlElement(style, element.Content.Text, imagePath, 0);
                content += htmlElement.GetElement();
            } 

            var mainContent = $"<img alt=\"\" style=\"; position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle \" width=594 height=840 src=\"file:///{uploads}/{templateId}/backgroundImage.jpg\" /><div>{content}</div>";
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

            string pdfPath = Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", "GeneratedPdfs", template.Id.ToString());
            string backgroundPath = Path.Combine(pdfPath, "cv.pdf");
            Directory.CreateDirectory(Path.GetDirectoryName(backgroundPath));

            using (var file = System.IO.File.Open(backgroundPath, FileMode.Create))
            {
                report.Content.CopyTo(file);
            }
            report.Content.Seek(0, SeekOrigin.Begin);
        }


        [HttpGet("getAllTemplates")]
        public ActionResult<GetAllTemplatesWithRatesResult> GetAllTemplates()
        {
            var result = new GetAllTemplatesWithRatesResult { Ids = new List<SingleTemplate>() };
            var ids = cvTemplateService.GetIds();
            foreach(var id in ids)
            {
                (var rate, var count) = cvTemplateService.GetRatesFor(id);
                result.Ids.Add(new SingleTemplate { Id = id, Rate = rate, RatesCount = count });
            }

            return result;
        }

        [HttpGet("getAllNotRatedTemplates")]
        public ActionResult<GetAllTemplatesResult> GetAllNotRatedTemplates()
        {
            string username = User.Identity.Name;
            return new GetAllTemplatesResult { Ids = cvTemplateService.GetIdsNotRatedBy(username) };
        }

        [HttpPost("getAuthorRanking")]
        public ActionResult<GetAuthorsRankingResult> GetAuthorRanking()
        {
            var authorsWithRanking = cvTemplateService.GetAuthorsRanking();
            var result = new GetAuthorsRankingResult() { AuthorWithRates = new List<AuthorWithRates>()} ;

            foreach(var x in authorsWithRanking)
            {
                result.AuthorWithRates.Add(new AuthorWithRates { Author = x.Key, Rate = x.Value });
            }

            return result;
        }

        [HttpGet("getAllFilledTemplates")]
        public ActionResult<GetAllTemplatesResult> GetAllFilledTemplates()
        {
            string authorName = User.Identity.Name;
            var result = new GetAllTemplatesResult { Ids = cvTemplateService.GetFilledTemplateIds(authorName) };
           return result;
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
                elementWithImg.IsProfilePicture = element.IsProfilePicture;
            
                result.Elements.Add(elementWithImg);
            }

            result.Id = template.Id;
            return result;
        }

        [HttpPost("getFilledTemplate")]
        public async Task<ActionResult<Model.Template>> GetFilledTemplate([FromBody] GetTemplateRequest request)
        {
            var (result, templateId) = await cvTemplateService.GetFilledTemplate(Guid.Parse(request.Id));
            return result;
        }

        [HttpPost("fillTemplate")]
        public async Task<ActionResult<Model.Template>> Post([FromForm]IFormCollection form)
        {
            var request = form["request"].ToString();

            FillTemplateRequest obj;
            try
            {
                obj = JsonConvert.DeserializeObject<FillTemplateRequest>(request);
            }
            catch (Exception e)
            {

                throw;
            }
            string username = User.Identity.Name;
          
            var filledTemplateId = await cvTemplateService.FillTemplate(new Model.Template { Elements = obj.Elements, Id = obj.Id }, username);

         

            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles", "FilledTemplate", filledTemplateId.ToString(), "profilePicture.jpg");
            Directory.CreateDirectory(Path.GetDirectoryName(uploads));
            if (form.Files["profilePicture"] != null)
            {
                using (Stream fileStream = new FileStream(uploads, FileMode.Create))
                {
                    await form.Files["profilePicture"].CopyToAsync(fileStream);
                }
            }

            await GeneratePdfAsync(filledTemplateId).ConfigureAwait(false);

            return new Model.Template();
        }

        [HttpPost("rateTemplate")]
        public async Task<ActionResult<Model.Template>> RateTemplate([FromBody] RateTemplateRequest request)
        {
            string username = User.Identity.Name;
            await cvTemplateService.RateTemplate(request.Id, username, request.Ocena);

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

            if (form.Files["backgroundImage"] != null)
            {
                using (Stream fileStream = new FileStream(backgroundPath, FileMode.Create))
                {
                    await form.Files["backgroundImage"].CopyToAsync(fileStream);
                }
            }
            var keys = form.Files.Select(x => x.Name).Where(x => x != "backgroundImage");

            foreach (var picture in keys)
            {
                var imagePath = Path.Combine(folderPath, $"{entity.Elements.First(x => x.ElementKey.ToString().Equals(picture)).Id}.jpg");
                try
                {
                    using (Stream fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await form.Files[picture].CopyToAsync(fileStream);
                    }
                }
                catch (Exception e)
                {

                    throw;
                } 
            }

            await GenerateTemplatePicture(entity.Id);
        }

        private async Task GenerateTemplatePicture(Guid templateId)
        { 
            var template = await cvTemplateService.GetByIdAsync(templateId);

            var content = string.Empty;
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath.Replace("\\", "/"), "StaticFiles");
            Directory.CreateDirectory(Path.GetDirectoryName(uploads));

            foreach (var element in template.Elements.OrderBy(x => x.Content.ZIndex))
            {
                var ElemenetStyleBuilder = new ElementStyleBuilder(element.Position.X, element.Position.Y, element.Size.X, element.Size.Y);
                var style = ElemenetStyleBuilder.WithZIndex(0).WithFontSize(element.Content.FontSize).WithBackgroundColor("red").Build();
                var imagePath = $"file:///{uploads}/{templateId}/{element.Id}.jpg";
                var htmlElement = new HtmlElement(style, element.Content.Text, imagePath, 0);
                content += htmlElement.GetElement();
            }

            var mainContent = $"<img alt=\"\" style=\"; position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle \" width=594 height=840 src=\"file:///{uploads}/{templateId}/backgroundImage.jpg\" /><div>{content}</div>";

            var converter = new HtmlConverter();

            var bytes = converter.FromHtmlString(mainContent, 594);
            using (var ms = new MemoryStream(bytes))
            {
                using (var fs = new FileStream(Path.Combine(uploads, templateId.ToString(), "template.jpg"), FileMode.Create))
                {
                    ms.WriteTo(fs);
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
