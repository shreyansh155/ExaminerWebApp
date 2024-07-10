using ExaminerWebApp.Composition.Helpers;
using Microsoft.AspNetCore.Mvc;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.ViewModels;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Controllers
{
    public class ExaminerController : BaseController
    {
        private readonly IExaminerService _examinerService;
        private readonly IExaminerTypeService _examinerTypeService;
        public ExaminerController(IExaminerService examinerService, IExaminerTypeService examinerTypeService)
        {
            _examinerService = examinerService;
            _examinerTypeService = examinerTypeService;
        }

        public IActionResult ExaminerIndex()
        {
            return View("Grid/Grid");
        }

        public async Task<ActionResult> GetAll(int pageSize, int pageNumber)
        {
            IQueryable<Examiner> data = await Task.Run(() => _examinerService.GetAllExaminer());
            var result = GetExaminerModels(data);

            return Json(await Pagination<ExaminerModel>.CreateAsync(result, pageNumber, pageSize));
        }

        #region OPEN MODAL
        public IActionResult ExaminerForm()
        {
            return PartialView("Modal/_ExaminerFormModal");
        }
        #endregion

        #region GET EXAMINER TYPE LIST
        public List<ExaminerType> ExaminerTypeList()
        {
            return _examinerTypeService.GetExaminerTypeList();
        }
        #endregion

        #region CREATE (POST)
        [HttpPost]
        public async Task<ActionResult> AddExaminer(ExaminerModel model)
        {
            if (ModelState.IsValid)
            {
                if (_examinerService.CheckEmailIfExists(model.Email))
                {
                    return Json(new { success = false, errors = "Email already exists!" });
                }

                Examiner applicant = new()
                {
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.Dateofbirth,
                    ExaminerId = model.ExaminerTypeId,
                    FilePath = model.Filepath,
                    FormFile = model.FormFile,
                };
                await _examinerService.AddExaminer(applicant);
                ModelState.Clear();
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }
        #endregion

        #region GET EXAMINER BY ID
        public IActionResult GetExaminer(int id)
        {
            Examiner result = _examinerService.GetExaminerById(id);
            ExaminerModel obj = new()
            {
                Id = id,
                Firstname = result.FirstName,
                Lastname = result.LastName,
                Dateofbirth = result.DateOfBirth,
                ExaminerTypeId = result.ExaminerId,
                Email = result.Email,
                Phone = result.Phone,
                ExaminerTypeName = result.ExaminerTypeName,
                Filepath = result.FilePath,
            };
            return PartialView("Modal/_ExaminerFormModal", obj);
        }
        #endregion

        #region EDIT EXAMINER (POST)
        [HttpPost]
        public ActionResult EditExaminer(ExaminerModel model)
        {
            if (ModelState.IsValid)
            {
                Examiner obj = new()
                {
                    Id = model.Id,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    DateOfBirth = model.Dateofbirth,
                    ExaminerId = model.ExaminerTypeId,
                    Email = model.Email,
                    Phone = model.Phone,
                    ExaminerTypeName = model.ExaminerTypeName ?? "",
                    FilePath = model.Filepath,
                    FormFile = model.FormFile ,
                    ModifiedBy = "1",
                    ModifiedDate = DateTime.Now,
                };
                _examinerService.UpdateExaminer(obj);
                // _notyf.Success("Examiner has been updated successfully");
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }
        #endregion

        #region DELETE EXAMINER (POST)
        public async Task<ActionResult> DeleteExaminer(int id)
        {
            try
            {
                await Task.Run(() => _examinerService.DeleteExaminer(id));
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
