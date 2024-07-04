using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class PhaseController : Controller
    {
        private readonly IPhaseService _phaseService;
        public PhaseController(IPhaseService phaseService)
        {
            _phaseService = phaseService;
        }
        // GET: PhaseController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<Phase> data = await Task.Run(() => _phaseService.GetAll());
            //IQueryable<ApplicantViewModel> result = _phaseService.(data);
           return Json(await Pagination<Phase>.CreateAsync(data, pageNumber, pageSize));
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PhaseController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhaseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
