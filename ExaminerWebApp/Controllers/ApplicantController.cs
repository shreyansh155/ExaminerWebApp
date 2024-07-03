using Microsoft.AspNetCore.Mvc;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.ViewModels;
using ExaminerWebApp.Service.Interface;
using ExaminerWebApp.Composition.Helpers;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ExaminerWebApp.Controllers
{
    public class ApplicantController : BaseController
    {
        private readonly IApplicantService _applicantService;
        private readonly IApplicantTypeService _applicantTypeService;
        private readonly INotyfService _notyf;

        #region CONSTRUCTOR
        public ApplicantController(IApplicantService applicantService, IApplicantTypeService applicantTypeService, INotyfService notyf)
        {
            _notyf = notyf;
            _applicantService = applicantService;
            _applicantTypeService = applicantTypeService;
        }
        #endregion

        public IActionResult ApplicantIndex()
        {
            return View("Grid/Grid");
        }

        #region READ ALL APPLICANTS
        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<Applicant> data = await Task.Run(() => _applicantService.GetAllApplicants());
            IQueryable<ApplicantViewModel> result = GetApplicantViewModels(data);

            return Json(await Pagination<ApplicantViewModel>.CreateAsync(result, pageNumber, pageSize));
        }
        #endregion

        #region OPEN MODAL
        public IActionResult ApplicantForm()
        {
            return PartialView("Modal/_ApplicationFormModal");
        }
        #endregion

        #region GET APPLICANT TYPE LIST
        public List<ApplicantType> ApplicantTypeList()
        {
            return _applicantTypeService.GetApplicantTypeList();
        }
        #endregion

        #region CREATE (POST)
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
                _notyf.Success("Applicant has been added successfully!");
                return Json(new { success = true });
            }
            else
            {
                // Return the partial view with the model to show validation errors
                var errors = ModelStateErrorSerializer(ModelState);
                _notyf.Error("Error occurred while adding applicant");
                return Json(new { success = false, errors });
            }
        }
        #endregion

        #region GET APPLICANT BY ID
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
        #endregion

        #region EDIT APPLICANT (POST)
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
                _notyf.Success("Applicant updated successfully!");
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }
        #endregion

        #region DELETE APPLICANT (POST)
        public async Task<ActionResult> DeleteApplicant(int id)
        {
            try
            {
                await Task.Run(() => _applicantService.DeleteApplicant(id));
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex });
            }
        }
        #endregion
    }
}
