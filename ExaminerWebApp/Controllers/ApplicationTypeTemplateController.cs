using AspNetCoreHero.ToastNotification.Abstractions;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class ApplicationTypeTemplateController : BaseController
    {
        private readonly IApplicationTypeService _applicationTypeService;
        private readonly INotyfService _notyf;

        public ApplicationTypeTemplateController(IApplicationTypeService applicationTypeService, INotyfService notyf)
        {
            _notyf = notyf;
            _applicationTypeService = applicationTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetAll(int pageSize, int pageNumber, string search)
        {
            IQueryable<ApplicationTypeTemplate> data = await Task.Run(() => _applicationTypeService.GetAll(search));
            IQueryable<ApplicationTypeTemplateModel> result = GetApplicationTemplates(data);
            return Json(await Pagination<ApplicationTypeTemplate>.CreateAsync(data, pageNumber, pageSize));
        }
        public IActionResult ShowTemplateModal()
        {
            return PartialView("Modal/_TemplateModal");
        }
        public IActionResult AddTemplate(ApplicationTypeTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                if (_applicationTypeService.ApplicationTemplateExists(model.Name))
                {
                    return Json(new { success = false, errors = "The entered application template already exists" });
                }
                else
                {
                    ApplicationTypeTemplate applicationType = new()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Instruction = model.Instruction,
                        CreatedBy = "1",
                        CreatedDate = DateTime.UtcNow,
                    };
                    try
                    {
                        var obj = _applicationTypeService.Add(applicationType);
                        _notyf.Success("Application Template has been successfully added!");
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, errors = ex.Message });
                    }
                    return Json(new { success = true });
                }
            }
            return View(model);
        }

        public IActionResult GetApplicationTemplate(int id)
        {
            return Json(new { redirectUrl = Url.Action("EditPage", "ApplicationTypeTemplate", new { id }) });
        }



        public IActionResult EditPage(int id)
        {
            var model = _applicationTypeService.GetById(id);
            ApplicationTypeTemplateModel obj = new()
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Instruction = model.Instruction,
            };

            return View("EditTemplate", obj);
        }

        public ActionResult EditTemplate(ApplicationTypeTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                // if (_applicationTypeService.ApplicationTemplateExists(model.Name)) { }
                ApplicationTypeTemplate applicationType = new()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Instruction = model.Instruction,
                    ModifiedBy = "2",
                    ModifiedDate = DateTime.UtcNow,
                };
                _applicationTypeService.Update(applicationType);
                _notyf.Success("Application Template has been successfully updated!");
            }
            return View(model);
        }

        public bool DeleteTemplate(int id)
        {
            try
            {
                _applicationTypeService.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
