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

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<Phase> data = _phaseService.GetAll();
            IQueryable<PhaseViewModel> result = GetPhase(data);
            return Json(await Pagination<PhaseViewModel>.CreateAsync(result, pageNumber, pageSize));
        }

        public IActionResult AddPhase()
        {
            return PartialView("Modal/_CreatePhase");
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
            return PartialView("Modal/_CreatePhase", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePhase(CreatePhaseModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _phaseService.CheckIfPhaseExists(model.Name) != true)
                {
                    Phase phase = new()
                    {
                        Name = model.Name,
                        Description = model.Description,
                    };
                    await _phaseService.CreatePhase(phase);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "Phase already exists." });
                }
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            Phase phase = await _phaseService.GetPhaseById(id);
            CreatePhaseModel model = new()
            {
                PhaseId = phase.Id,
                Name = phase.Name,
                Description = phase.Description,
            };
            return PartialView("Modal/_CreatePhase", model);
        }

        [HttpPost]
        public async Task<ActionResult> EditPhase(CreatePhaseModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _phaseService.CheckPhaseOnUpdateExists(model.Name, model.PhaseId) != true)
                {
                    Phase phase = new()
                    {
                        Id = model.PhaseId,
                        Name = model.Name,
                        Description = model.Description,
                        ModifiedBy = "2",
                        ModifiedDate = DateTime.UtcNow,
                    };
                    await _phaseService.UpdatePhase(phase);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "Phase already exists." });
                }
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
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
