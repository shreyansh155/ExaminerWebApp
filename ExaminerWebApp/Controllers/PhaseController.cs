﻿using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;
        public PhaseController(IPhaseService phaseService, INotyfService notyf)
        {
            _notyf = notyf;
            _phaseService = phaseService;
        }
        // GET: PhaseController
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
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        public IActionResult AddPhase()
        {
            return PartialView("Modal/_CreatePhase");
        }
        public IActionResult AddPhaseSteps(int phaseId)
        { 
            ViewBag.ShowGrid = true;
            return PartialView("Modal/_CreatePhase");
        }

        [HttpPost]
        public ActionResult CreatePhase(CreatePhaseModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Phase phase = new()
                    {
                        Name = model.Name,
                        Description = model.Description,
                    };
                    _phaseService.CreatePhase(phase);
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

        public ActionResult Edit(int id)
        {
            return View();
        }

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
