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
        private readonly ITemplatePhaseService _templatePhaseService;
        public ApplicationTypeTemplateController(IApplicationTypeService applicationTypeService, ITemplatePhaseService templatePhaseService)
        {
            _applicationTypeService = applicationTypeService;
            _templatePhaseService = templatePhaseService;
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

        public async Task<IActionResult> EditPage(int id)
        {
            var model = await _applicationTypeService.GetById(id);
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

        public async Task<ActionResult> GetPhase(int templateId)
        {
            Object phases = await Task.Run(() => _applicationTypeService.GetPhaseByTemplate(templateId));
            return Json(phases);
        }

        [Route("/ApplicationTypeTemplate/EditPage/ApplicationTypeTemplate/GetPhaseStep")]
        public async Task<ActionResult> GetPhaseStep(int templateId, int phaseId)
        {
            try
            {
                var phases = await _applicationTypeService.GetPhaseStepsAsync(templateId, phaseId);
                var obj = Json(phases);
                return obj;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return Json(new { success = false });
            }
        }

        public async Task<ActionResult> GetPhaseSteps(int templateId, int phaseId)
        {
            try
            {
                var phases = await _applicationTypeService.GetPhaseStepsByTemplateAsync(templateId, phaseId);
                var obj = Json(phases);
                return obj;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return Json(new { success = false });
            }
        }

        public IActionResult OpenTemplatePhase(int templateId)
        {
            PhaseViewModel model = new()
            {
                TemplateId = templateId
            };
            return PartialView("Modal/_AddPhase", model);
        }

        public IQueryable<Phase> PhaseList(int templateId)
        {
            return _applicationTypeService.PhaseList(templateId);
        }

        [HttpPost]
        public IActionResult AddTemplatePhase([FromBody] PhaseViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, errors = "Model is null" });
            }

            if (ModelState.IsValid)
            {
                List<TemplatePhaseStep>? gridDataItems = model.GridData?.Where(x => x.Ordinal != null).Select(data => new TemplatePhaseStep
                {
                    StepId = data.Id,
                    Ordinal = data.Ordinal,
                    IsInTemplatePhaseSteps = data.IsInTemplatePhaseSteps
                }).ToList();

                ApplicationTypeTemplatePhase obj = new()
                {
                    TemplateId = model.TemplateId,
                    Ordinal = model.Ordinal,
                    PhaseId = model.PhaseId,
                    TemplatePhaseSteps = gridDataItems
                };
                _templatePhaseService.AddTemplatePhaseStep(obj);
                return Json(new { success = true });
            }

            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public ActionResult UpdatePhaseOrdinal(int phaseId, int templateId, int ordinal)
        {
            _templatePhaseService.UpdateOrdinal(templateId, phaseId, ordinal);
            return Json(new { success = true });
        }
    }
}
