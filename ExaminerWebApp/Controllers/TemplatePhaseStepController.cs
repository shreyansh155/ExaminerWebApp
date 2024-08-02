using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Implementation;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Kendo.Mvc.UI;
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
                if (await _applicationTypeService.CheckIfExists(model.Id, model.Name))
                {
                    return Json(new { success = false, errors = "This application template already exists" });
                }
                model.ModifiedDate = DateTime.UtcNow;
                model.ModifiedBy = "2";
                await _applicationTypeService.Update(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
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
            ApplicationTypeTemplatePhase model = new()
            {
                TemplateId = templateId,
                Ordinal = await _templatePhaseService.GetNewPhaseOrdinal(templateId),
            };
            return PartialView("Modal/_AddPhase", model);
        }

        public async Task<IQueryable<Phase>> PhaseList(int templateId)
        {
            return await _applicationTypeService.PhaseList(templateId);
        }

        [HttpPost]
        public async Task<IActionResult> AddTemplatePhase([FromBody] ApplicationTypeTemplatePhase model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.AddTemplatePhase(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        public async Task<IActionResult> EditPhase(int templatePhaseId, int ordinal, string phaseName)
        {
            ApplicationTypeTemplatePhase model = new()
            {
                Id = templatePhaseId,
                Ordinal = ordinal,
                PhaseName = phaseName
            };

            return await Task.FromResult(PartialView("Modal/_EditPhase", model));
        }

        public async Task<IActionResult> EditTemplatePhase([FromBody] ApplicationTypeTemplatePhase model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.UpdateOrdinal(model.Id, model.Ordinal);
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
            return await Task.FromResult(PartialView("Modal/_AddStep", model));
        }

        public async Task<IActionResult> EditStep(int id)
        {
            TemplatePhaseStep templatePhaseStep = await _templatePhaseService.GetTemplatePhaseStep(id);

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

        public async Task<ActionResult> GetStepTypeId(int stepId)
        {
            int? stepTypeId = 0;
            if (stepId != 0)
            {
                stepTypeId = await _templatePhaseService.GetStepTypeId(stepId);
            }
            return Json(new { stepTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> AddTemplatePhaseStep(TemplatePhaseStep model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.AddTemplatePhaseStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplatePhaseStep(TemplatePhaseStep model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.EditTemplatePhaseStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplatePhase(int id)
        {
            return Json(new
            {
                success = await _templatePhaseService.DeletePhase(id)
            });
        }

        public async Task<ActionResult> GetTemplatePhaseStepAttachment(int? tpsId, [FromBody] PaginationSet<TemplatePhaseStepAttachment> pager)
        {
            return Json(await _templatePhaseService.GetAttachments(tpsId, pager));
        }

        public async Task<ActionResult> GetTemplatePhaseStepDocumentProof(int tpsId, [FromBody] PaginationSet<TemplatePhaseStepDocumentProof> pager)
        {
            return Json(await _templatePhaseService.GetDocumentProof(tpsId, pager));
        }

        public async Task<ActionResult> GetDocumentTypeList()
        {
            var obj = Json(await _templatePhaseService.GetDocumentFileTypes());
            return obj;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplatePhaseStepDocumentProof([FromBody] TemplatePhaseStepDocumentProof model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.CreateDocumentProof(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplatePhaseStepDocumentProof([FromBody] TemplatePhaseStepDocumentProof model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.EditDocumentProof(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplatePhaseStepDocumentProof(int id)
        {
            await _templatePhaseService.DeleteDocumentProof(id);
            return Json(new { success = true });
        }

        public async Task<IActionResult> OpenAttachmentModal(int tpsId)
        {
            TemplatePhaseStepAttachment model = new()
            {
                TemplatePhaseStepId = tpsId,
            };
            return await Task.FromResult(PartialView("Modal/_AttachmentModal", model));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttachment(TemplatePhaseStepAttachment model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.CreateAttachment(model);
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<IActionResult> EditTemplatePhaseStepAttachment([FromBody] TemplatePhaseStepAttachment model)
        {
            if (ModelState.IsValid)
            {
                await _templatePhaseService.EditAttachment(model);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTemplatePhaseStepAttachment(int id)
        {
            await _templatePhaseService.DeleteAttachment(id);
            return Json(new { success = true });
        }
    }
}