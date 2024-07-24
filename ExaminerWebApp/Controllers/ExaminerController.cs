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

        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        public async Task<ActionResult> GetAll([FromBody]PaginationSet<Examiner> pager) 
        {
            return Json(await _examinerService.GetAllExaminer(pager));
        }

        public async Task<IActionResult> ExaminerForm()
        {
            return await Task.FromResult(PartialView("Modal/_ExaminerFormModal"));
        }

        public async Task<List<ExaminerType>> ExaminerTypeList()
        {
            return await _examinerTypeService.GetExaminerTypeList();
        }

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

        public async Task<IActionResult> GetExaminer(int id)
        {
            Examiner result = await _examinerService.GetExaminerById(id);
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

        [HttpPost]
        public async Task<ActionResult> EditExaminer(ExaminerModel model)
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
                    FormFile = model.FormFile,
                    ModifiedBy = "1",
                    ModifiedDate = DateTime.Now,
                };
              await  _examinerService.UpdateExaminer(obj);
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelStateErrorSerializer(ModelState);
                return Json(new { success = false, errors });
            }
        }

        public async Task<ActionResult> DeleteExaminer(int id)
        {
            await _examinerService.DeleteExaminer(id);
            return Json(new { success = true });
        }
    }
}