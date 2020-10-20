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
            var imagePath = _hostingEnvironment.ContentRootPath + "\\" + "no-picture.jpg";

            var ElemenetStyleBuilder = new ElementStyleBuilder(10, 300, 200, 200);
            var style = ElemenetStyleBuilder.WithBackgroundColor("red").Build();
            var element = new HtmlElement(style, "testowy tekst");
            var report = await jsReportMVCService.RenderAsync(new RenderRequest()
            {
                Template = new jsreport.Types.Template
                {
                    Content = $"<html>{element.GetElement()}</html>",
                    Engine = Engine.None,
                    Recipe = Recipe.ChromePdf
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
            return result;
        }

        [HttpPost("fillTemplate")]
        public async Task<ActionResult<Model.Template>> Post([FromBody] FillTemplateRequest request)
        {
            return new Model.Template();
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
