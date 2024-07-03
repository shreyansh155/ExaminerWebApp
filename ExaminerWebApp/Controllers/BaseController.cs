using ExaminerWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExaminerWebApp.Controllers
{
    public class BaseController : Controller
    {
        #region ERROR MESSAGES

        protected string ModelStateErrorSerializer(ModelStateDictionary modelState)
        {
            List<string> errors = new();
            foreach (var item in modelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            return string.Join("; ", errors);
        }

        #endregion

        public IQueryable<ExaminerModel> GetExaminerModels(IQueryable<Entities.Entities.Examiner> entities)
        {
            return entities.Select(entity => new ExaminerModel
            {
                Id = entity.Id,
                Firstname = entity.FirstName,
                Lastname = entity.LastName,
                Dateofbirth = entity.DateOfBirth,
                Phone = entity.Phone,
                Email = entity.Email,
                ExaminerTypeId = entity.ExaminerId,
                ExaminerTypeName = entity.ExaminerTypeName,
                Filepath = entity.FilePath,
                Isdeleted = entity.IsDeleted,
                Status = entity.Status,
            });
        }

    }
}
