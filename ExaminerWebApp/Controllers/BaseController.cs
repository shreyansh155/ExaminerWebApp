using ExaminerWebApp.Entities.Entities;
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
        protected IQueryable<ApplicantViewModel> GetApplicantViewModels(IQueryable<Applicant> entities)
        {
            return entities.Select(entity => new ApplicantViewModel
            {
                Id = entity.Id,
                Firstname = entity.FirstName,
                Lastname = entity.LastName,
                Dateofbirth = entity.DateOfBirth,
                Phone = entity.Phone,
                Email = entity.Email,
                Settingid = entity.ApplicantTypeId,
                ApplicantTypeName = entity.ApplicantType,
                Filepath = entity.FilePath,
                FormFile = entity.FormFile,
                Isdeleted = entity.IsDeleted
            });
        }

        protected IQueryable<ExaminerModel> GetExaminerModels(IQueryable<Examiner> entities)
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
        protected IQueryable<ApplicationTypeTemplateModel> GetApplicationTemplates(IQueryable<ApplicationTypeTemplate> entities)
        {
            return entities.Select(entity => new ApplicationTypeTemplateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
            });
        }
        protected IQueryable<PhaseViewModel> GetPhase(IQueryable<Phase> entities)
        {
            return entities.Select(entity => new PhaseViewModel
            {
                PhaseId = entity.Id,
                Name = entity.Name,
                Description = entity.Description ?? "",
                Ordinal = entity.Ordinal,
            });
        }
        protected IQueryable<StepViewModel> GetSteps(IQueryable<Step> entities)
        {
            return entities.Select(entity => new StepViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
            });
        }
    }
}