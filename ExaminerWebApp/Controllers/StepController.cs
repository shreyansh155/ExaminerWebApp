using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
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
        public async Task<IActionResult> CreateStep([FromBody] Step model)
        {
            if (ModelState.IsValid)
            {
                if (!await _stepService.CheckIfStepExists(model))
                    return Json(new { success = false, errors = "Step already exists." });

                await _stepService.CreateStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
        }

        [HttpPost]
        public async Task<IActionResult> EditStep([FromBody] Step model)
        {
            if (ModelState.IsValid)
            {
                if (!await _stepService.CheckIfEditStepExists(model))
                    return Json(new { success = false, errors = "Step already exists." });

                await _stepService.UpdateStep(model);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelStateErrorSerializer(ModelState) });
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