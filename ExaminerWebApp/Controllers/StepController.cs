using AspNetCoreHero.ToastNotification.Abstractions;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class StepController : BaseController
    {
        private readonly IStepService _stepService;
        private readonly INotyfService _notyf;
        public StepController(IStepService stepService, INotyfService notyf)
        {
            _notyf = notyf;
            _stepService = stepService;
        }

        // GET: StepController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAll(int phaseId)
        {
            IQueryable<Step> data = _stepService.GetAll(phaseId);
            IQueryable<StepViewModel> result = await Task.Run(() => GetSteps(data));
            return Json(await Pagination<StepViewModel>.CreateAsync(result, 10, 1));
        }
        // GET: PhaseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PhaseController/Create
        public ActionResult Create()
        {
            return View();
        }
 
        // POST: PhaseController/Create
        [HttpPost]
        public ActionResult CreatePhase(CreatePhaseModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Step step = new();
                    _stepService.CreateStep(step);
                }
                else
                {
                    var errors = ModelStateErrorSerializer(ModelState);
                    _notyf.Error("Error occurred while adding applicant");
                    return Json(new { success = false, errors });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhaseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhaseController/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PhaseController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhaseController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
