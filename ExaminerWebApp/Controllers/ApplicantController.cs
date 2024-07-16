using Microsoft.AspNetCore.Mvc;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.ViewModels;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.Composition.Helpers;

namespace ExaminerWebApp.Controllers
{
    public class ApplicantController : BaseController
    {
        private readonly IApplicantService _applicantService;
        private readonly IApplicantTypeService _applicantTypeService;

        public ApplicantController(IApplicantService applicantService, IApplicantTypeService applicantTypeService)
        {
            _applicantService = applicantService;
            _applicantTypeService = applicantTypeService;
        }

        public IActionResult ApplicantIndex()
        {
            return View("Grid/Grid");
        }

        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<Applicant> data = await Task.Run(() => _applicantService.GetAllApplicants());

            IQueryable<ApplicantViewModel> result = GetApplicantViewModels(data);

            return Json(await Pagination<ApplicantViewModel>.CreateAsync(result, pageNumber, pageSize));
        }

        public IActionResult ApplicantForm()
        {
            return PartialView("Modal/_ApplicationFormModal");
        }

        public List<ApplicantType> ApplicantTypeList()
        {
            return _applicantTypeService.GetApplicantTypeList();
        }

        [HttpPost]
        public async Task<ActionResult> AddApplicant(ApplicantViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_applicantService.CheckEmailIfExists(model.Email))
                {
                    return Json(new { success = false, errors = "Email already exists!" });
                }
                Applicant applicant = new()
                {
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.Dateofbirth,
                    FileName = model.FormFile?.FileName,
                    FormFile = model.FormFile ?? null,
                    ApplicantTypeId = model.Settingid,
                };

                await _applicantService.AddApplicant(applicant);
                ModelState.Clear();
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public IActionResult GetApplicant(int id)
        {
            var result = _applicantService.GetApplicantById(id);
            ApplicantViewModel obj = new()
            {
                Id = id,
                Firstname = result.FirstName,
                Lastname = result.LastName,
                Dateofbirth = result.DateOfBirth,
                Settingid = result.ApplicantTypeId,
                Email = result.Email,
                Phone = result.Phone,
                ApplicantTypeName = result.ApplicantType,
                Filepath = result.FilePath,
            };
            return PartialView("Modal/_ApplicationFormModal", obj);
        }

        [HttpPost]
        public ActionResult EditApplicant(ApplicantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Applicant obj = new()
                {
                    Id = model.Id,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    DateOfBirth = model.Dateofbirth,
                    ApplicantTypeId = model.Settingid,
                    Email = model.Email,
                    Phone = model.Phone,
                    ApplicantType = model.ApplicantTypeName ?? "",
                    FilePath = model.Filepath,
                    FormFile = model.FormFile ?? null,
                    FileName = model.FormFile?.FileName,
                };

                _applicantService.UpdateApplicant(obj);
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public async Task<ActionResult> DeleteApplicant(int id)
        {
            await _applicantService.DeleteApplicant(id);
            return Json(new { success = true });
        }
    }
}
