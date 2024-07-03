using Microsoft.AspNetCore.Mvc;

namespace ExaminerWebApp.Controllers
{
    public class PhaseController : Controller
    {
        // GET: PhaseController
        public ActionResult Index()
        {
            return View();
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
