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
            return Json(await Pagination<ApplicationTypeTemplateModel>.CreateAsync(result, pageNumber, pageSize));
        }

        public IActionResult ShowTemplateModal()
        {
            return PartialView("Modal/_TemplateModal");
        }

        public async Task<IActionResult> AddTemplate(ApplicationTypeTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                if (_applicationTypeService.ApplicationTemplateExists(model.Name))
                {
                    return Json(new { success = false, errors = "This application template already exists" });
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

                    await _applicationTypeService.Add(applicationType);
                    return Json(new { success = true });
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteTemplate(int id)
        {
            return Json(new { success = await _applicationTypeService.DeleteTemplate(id) });
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

        public async Task<ActionResult> EditTemplate(ApplicationTypeTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _applicationTypeService.EditApplicationTemplateExists(model.Id, model.Name))
                {
                    return Json(new { success = false, errors = "This application template already exists" });
                }
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

        public async Task<ActionResult> GetPhase(int templateId)
        {
            Object obj = await Task.Run(() => _applicationTypeService.GetPhaseByTemplate(templateId));
            return Json(obj);
        }

        [Route("/ApplicationTypeTemplate/EditPage/ApplicationTypeTemplate/GetPhaseStep")]
        public async Task<ActionResult> GetPhaseStep(int templateId, int phaseId)
        {
            var phases = await _applicationTypeService.GetPhaseStepsAsync(templateId, phaseId);
            var obj = Json(phases);
            return obj;
        }

        public async Task<ActionResult> GetPhaseSteps(int templateId, int phaseId)
        {
            var phases = await _applicationTypeService.GetPhaseStepsByTemplateAsync(templateId, phaseId);
            var obj = Json(phases);
            return obj;
        }

        public async Task<IActionResult> OpenTemplatePhase(int templateId)
        {
            int ordinal = await _templatePhaseService.GetNewPhaseOrdinal(templateId);
            PhaseViewModel model = new()
            {
                TemplateId = templateId,
                Ordinal = ordinal,
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
                List<TemplatePhaseStep>? gridDataItems = model.GridData?.Where(x => x.IsInTemplatePhaseSteps == true).Select(data => new TemplatePhaseStep
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

        public IActionResult EditPhase(int templatePhaseId, int ordinal, string phaseName)
        {
            EditPhase model = new()
            {
                TemplatePhaseId = templatePhaseId,
                Ordinal = ordinal,
                PhaseName = phaseName
            };
            return PartialView("Modal/_EditPhase", model);
        }

        public async Task<IActionResult> EditTemplatePhase([FromBody] EditPhase model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.UpdateOrdinal(model.TemplatePhaseId, model.Ordinal);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> AddTemplatePhaseStep(int templatePhaseId)
        {
            int ordinal = await _templatePhaseService.GetNewStepOrdinal(templatePhaseId);
            AddEditStepModel model = new()
            {
                TemplatePhaseId = templatePhaseId,
                Ordinal = ordinal,
            };
            return PartialView("Modal/_AddStep", model);
        }

        public IActionResult EditStep(int id)
        {
            TemplatePhaseStep templatePhaseStep = _templatePhaseService.GetTemplatePhaseStep(id);

            AddEditStepModel model = new()
            {
                Id = id, //template phase step id 
                TemplatePhaseId = templatePhaseStep.TemplatePhaseId,
                StepId = templatePhaseStep.StepId,
                StepTypeId = templatePhaseStep?.Step?.StepTypeId,
                Instruction = templatePhaseStep?.Step?.Instruction,
                Ordinal = templatePhaseStep?.Ordinal,
            };

            return PartialView("Modal/_AddStep", model);
        }

        public async Task<IActionResult> DeleteStep(int id)
        {
            return Json(new { success = await _templatePhaseService.DeleteStep(id) });
        }

        public async Task<List<Step>> StepList(int? id)
        {
            return await _applicationTypeService.StepList(id);
        }

        public async Task<List<Step>> PhaseStepList(int templatePhaseId)
        {
            return await _applicationTypeService.PhaseStepList(templatePhaseId);
        }

        public ActionResult GetStepTypeId(int stepId)
        {
            var stepTypeId = 0;
            if (stepId != 0)
            {
                stepTypeId = _templatePhaseService.GetStepTypeId(stepId);
            }
            return Json(new { stepTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplatePhaseStep([FromBody] AddEditStepModel model)
        {
            if (ModelState.IsValid)
            {
                TemplatePhaseStep templatePhaseStep = new()
                {
                    Id = model.Id,
                    TemplatePhaseId = model.TemplatePhaseId,
                    StepId = model.StepId,
                    Ordinal = model.Ordinal,
                    Instruction = model.Instruction,
                };
                await _templatePhaseService.EditTemplatePhaseStep(templatePhaseStep);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> DeleteTemplatePhase(int templatePhaseId)
        {
            return Json(new
            {
                success = await _applicationTypeService.Delete(templatePhaseId)
            });
        }
    }
}