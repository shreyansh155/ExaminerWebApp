using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class TemplatePhaseStepController : BaseController
    {
        private readonly IApplicationTypeService _applicationTypeService;
        private readonly ITemplatePhaseService _templatePhaseService;

        public TemplatePhaseStepController(IApplicationTypeService applicationTypeService, ITemplatePhaseService templatePhaseService)
        {
            _applicationTypeService = applicationTypeService;
            _templatePhaseService = templatePhaseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            return View(await _applicationTypeService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult> EditTemplate(ApplicationTypeTemplate model)
        {
            if (ModelState.IsValid)
            {
                if (await _applicationTypeService.EditApplicationTemplateExists(model.Id, model.Name))
                {
                    return Json(new { success = false, errors = "This application template already exists" });
                }
                model.ModifiedDate = DateTime.UtcNow;
                model.ModifiedBy = "2";
                await _applicationTypeService.Update(model);
            }
            return View(model);
        }

        public async Task<ActionResult> GetPhase(int templateId, [FromBody] PaginationSet<object> pager)
        {
            return Json(await _applicationTypeService.GetPhaseByTemplate(templateId, pager));
        }

        [Route("/TemplatePhaseStep/Index/TemplatePhaseStep/GetPhaseStep")]
        public async Task<ActionResult> GetPhaseStep(int templateId, int phaseId)
        {
            var phases = await _applicationTypeService.GetPhaseStepsAsync(templateId, phaseId);
            return Json(phases);
        }

        public async Task<ActionResult> GetPhaseSteps(int templateId, int phaseId)
        {
            var phases = await _applicationTypeService.GetPhaseStepsByTemplateAsync(templateId, phaseId);

            return Json(phases); ;
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

        public async Task<IQueryable<Phase>> PhaseList(int templateId)
        {
            return await Task.FromResult(_applicationTypeService.PhaseList(templateId));
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
                    StepCount = gridDataItems != null ? gridDataItems.Count : 0,
                    TemplatePhaseSteps = gridDataItems
                };
                _templatePhaseService.AddTemplatePhase(obj);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
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
            TemplatePhaseStep model = new()
            {
                TemplatePhaseId = templatePhaseId,
                Ordinal = await _templatePhaseService.GetNewStepOrdinal(templatePhaseId),
            };
            return PartialView("Modal/_AddStep", model);
        }

        public IActionResult EditStep(int id)
        {
            TemplatePhaseStep templatePhaseStep = _templatePhaseService.GetTemplatePhaseStep(id);

            return PartialView("Modal/_AddStep", templatePhaseStep);
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
        public async Task<IActionResult> AddTemplatePhaseStep([FromBody] TemplatePhaseStep model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.AddTemplatePhaseStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplatePhaseStep([FromBody] TemplatePhaseStep model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.EditTemplatePhaseStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
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