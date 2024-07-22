using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class PhaseController : BaseController
    {
        private readonly IPhaseService _phaseService;

        public PhaseController(IPhaseService phaseService)
        {
            _phaseService = phaseService;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public async Task<ActionResult> GetAll([FromBody] PaginationSet<Phase> pager)
        {
            return Json(await _phaseService.GetAll(pager));
        }

        public async Task<IActionResult> AddPhase()
        {
            return await Task.FromResult(PartialView("Modal/_CreatePhase"));
        }

        public async Task<IActionResult> AddPhaseSteps(int phaseId)
        {
            Phase phase = await _phaseService.GetPhaseById(phaseId);
            CreatePhaseModel model = new()
            {
                Name = phase.Name,
                Description = phase.Description ?? "",
                PhaseId = phaseId,
            };
            ViewBag.ShowGrid = true;
            return PartialView("Modal/_CreatePhase", phase);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePhase(Phase model)
        {
            if (ModelState.IsValid)
            {
                if (await _phaseService.CheckIfPhaseExists(model.Name) != true)
                {
                    await _phaseService.CreatePhase(model);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "Phase already exists." });
                }
            }
            else
            {

                return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Phase phase = await _phaseService.GetPhaseById(id);
            return PartialView("Modal/_CreatePhase", phase);
        }

        [HttpPost]
        public async Task<ActionResult> EditPhase(Phase model)
        {
            if (ModelState.IsValid)
            {
                if (await _phaseService.CheckPhaseOnUpdateExists(model.Name, model.Id) != true)
                {
                    await _phaseService.UpdatePhase(model);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "Phase already exists." });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (id != 0)
            {
                await Task.Run(() => _phaseService.DeletePhase(id));
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}