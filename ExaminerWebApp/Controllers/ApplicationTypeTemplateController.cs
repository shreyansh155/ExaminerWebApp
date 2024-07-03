using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Implementation;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class ApplicationTypeTemplateController : BaseController
    {
        private readonly IApplicationTypeService _applicationTypeService;
        public ApplicationTypeTemplateController(IApplicationTypeService applicationTypeService)
        {
            _applicationTypeService = applicationTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<ApplicationTypeTemplate> data = await Task.Run(() => _applicationTypeService.GetAll());
            IQueryable<ApplicationTypeTemplateModel> result = GetApplicationTemplates(data);
            return Json(await Pagination<ApplicationTypeTemplate>.CreateAsync(data, pageNumber, pageSize));
        }
        public IActionResult AddTemplate()
        {
            return PartialView("Modal/_TemplateModal");
        }
    }
}
