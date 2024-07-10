using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class StepController : BaseController
    {
        private readonly IStepService _stepService;
        public StepController(IStepService stepService)
        {
            _stepService = stepService;
        }

        public async Task<ActionResult> GetAll(int phaseId)
        {
            IQueryable<Step> data = _stepService.GetAll(phaseId);
            IQueryable<StepViewModel> result = await Task.Run(() => GetSteps(data));
            return Json(result);
        }

        public async Task<ActionResult> GetAllSteps(int phaseId)
        {
            IQueryable<Step> data = _stepService.GetAll(phaseId);
            IQueryable<StepsList> result = await Task.Run(() => GetAllSteps(data));
            return Json(result);
        }

        [HttpPost]
        public IActionResult CreateStep([FromBody] StepViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_stepService.CheckIfStepExists(model.PhaseId, model.Name))
                {
                    Step step = new()
                    {
                        PhaseId = model.PhaseId,
                        Name = model.Name,
                        Description = model.Description,
                        Instruction = model.Instruction,
                        StepTypeId = model.TypeId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "1",
                    };
                    _stepService.CreateStep(step);

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "This phase already contains the entered step." });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult EditStep([FromBody] StepViewModel model)
        {
            if (ModelState.IsValid)
            {
                Step step = new()
                {
                    Id = (int)model.Id,
                    PhaseId = model.PhaseId,
                    Name = model.Name,
                    Description = model.Description,
                    Instruction = model.Instruction,
                    StepTypeId = model.TypeId,
                    ModifiedDate = DateTime.UtcNow,
                    ModifiedBy = "2",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "1",
                };
                _stepService.UpdateStep(step);

                return Json(new { success = true });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public List<StepType> StepTypeList()
        {
            return _stepService.GetStepTypeList();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _stepService.DeleteStep(id);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
