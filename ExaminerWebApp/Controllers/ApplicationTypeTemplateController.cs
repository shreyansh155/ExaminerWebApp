using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Kendo.Mvc.UI;
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

        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public async Task<ActionResult> GetAll([FromBody] PaginationSet<ApplicationTypeTemplate> pager)
        {
            return Json(await _applicationTypeService.GetAll(pager));
        }

        public async Task<IActionResult> ShowTemplateModal()
        {
            return await Task.FromResult(PartialView("Modal/_TemplateModal"));
        }

        public async Task<IActionResult> AddTemplate(ApplicationTypeTemplate model)
        {
            if (ModelState.IsValid)
            {
                if (await _applicationTypeService.ApplicationTemplateExists(model.Name))
                {
                    return Json(new { success = false, errors = "This application template already exists" });
                }
                else
                {
                    model.CreatedBy = "1";
                    model.CreatedDate = DateTime.UtcNow;

                    await _applicationTypeService.Add(model);
                    return Json(new { success = true });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteTemplate(int id)
        {
            return Json(new { success = await _applicationTypeService.DeleteTemplate(id) });
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicationTemplate(int id)
        {
            return Json(await Task.FromResult(new { redirectUrl = Url.Action("Index", "TemplatePhaseStep", new { id }) }));
        }
    }
}