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

        public IActionResult AddPhaseSteps(int phaseId)
        {
            Phase phase = _phaseService.GetPhaseById(phaseId);
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
        public ActionResult CreatePhase(CreatePhaseModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!_phaseService.CheckIfPhaseExists(model.Name))
                    {
                        Phase phase = new()
                        {
                            Name = model.Name,
                            Description = model.Description,
                        };
                        _phaseService.CreatePhase(phase);
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, errors = "Entered phase name already exists." });
                    }
                }
                else
                {
                    var errors = ModelStateErrorSerializer(ModelState);
                    return Json(new { success = false, errors });
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                Phase phase = _phaseService.GetPhaseById(id);
                CreatePhaseModel model = new()
                {
                    PhaseId = phase.Id,
                    Name = phase.Name,
                    Description = phase.Description,
                };
                return PartialView("Modal/_CreatePhase", model);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditPhase(CreatePhaseModel model)
        {
            if (ModelState.IsValid)
            {
                Phase phase = new()
                {
                    Id = (int)model.PhaseId,
                    Name = model.Name,
                    Description = model.Description,
                    ModifiedBy = "2",
                    ModifiedDate = DateTime.UtcNow,
                };
                _phaseService.UpdatePhase(phase);
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await Task.Run(() => _phaseService.DeletePhase(id));
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex });
            }
        }
    }
}
