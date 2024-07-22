using ExaminerWebApp.Composition.Helpers;
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

        public async Task<ActionResult> GetAll([FromQuery] int phaseId, [FromBody] PaginationSet<Step> pager)
        {
            return Json(await _stepService.GetAll(phaseId, pager));
        }

        [HttpPost]
        public async Task<IActionResult> CreateStep([FromBody] StepViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Name != null && await _stepService.CheckIfStepExists(model.PhaseId, model.Name) != true)
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
                    await _stepService.CreateStep(step);

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "Step already exists." });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditStep([FromBody] StepViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _stepService.CheckIfEditStepExists(model.PhaseId, model.Id, model.Name))
                {
                    return Json(new { success = false, errors = "Step already exists." });
                }
                Step step = new()
                {
                    Id = model.Id,
                    PhaseId = model.PhaseId,
                    Name = model.Name ?? "",
                    Description = model.Description,
                    Instruction = model.Instruction,
                    StepTypeId = model.TypeId,
                    ModifiedDate = DateTime.UtcNow,
                    ModifiedBy = "2",
                };
                _stepService.UpdateStep(step);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
            }
        }

        public async Task<List<StepType>> StepTypeList()
        {
            return await _stepService.GetStepTypeList();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            return Json(new { success = await _stepService.DeleteStep(id) });
        }
    }
}