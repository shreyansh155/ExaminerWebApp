using AutoMapper;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using ExaminerWebApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        #region Constructor
        public ApplicantService(IApplicantRepository applicant, IMapper mapper, IWebHostEnvironment environment)
        {
            _environment = environment;
            _applicantRepository = applicant;
            _mapper = mapper;
        }
        #endregion

        #region Interface Methods

        #region READ SPECIFIC APPLICANT
        public Entities.Entities.Applicant GetApplicantById(int id)
        {
            return ExecuteWithTryCatch(() =>
            {
                var result = _applicantRepository.GetById(id);
                return _mapper.Map<Entities.Entities.Applicant>(result);
            });
        }
        #endregion

        #region READ ALL APPLICANTS
        public IQueryable<Entities.Entities.Applicant> GetAllApplicants()
        {
            return ExecuteWithTryCatch(() =>
            {
                var list = _applicantRepository.GetAll().Include(x => x.ApplicantType).AsQueryable();
                var obj = _mapper.ProjectTo<Entities.Entities.Applicant>(list);
                return obj;
            });
        }
        #endregion

        #region ADD APPLICANT
        public async Task<Entities.Entities.Applicant> AddApplicant(Entities.Entities.Applicant model)
        {

            var obj = _mapper.Map<Repository.DataModels.Applicant>(model);
            if (model.FormFile != null && model.FormFile.Length != 0)
            {
                obj.FilePath = SaveFile(model.FormFile);
            }
            await _applicantRepository.Create(obj);
            return model;
        }
        #endregion

        #region CHECK EMAIL IF EXISTS
        public bool CheckEmailIfExists(string email)
        {
            return _applicantRepository.CheckEmail(email);
        }

        #endregion

        #region DELETE APPLICANT
        public bool DeleteApplicant(int id)
        {
            return ExecuteWithTryCatch(() =>
            {
                _applicantRepository.Delete(id);
                return true;
            });
        }
        #endregion

        #region EDIT APPLICANT DETAILS
        public bool UpdateApplicant(Entities.Entities.Applicant model)
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.FilePath = SaveFile(model.FormFile);
            }
            var result = _mapper.Map<Repository.DataModels.Applicant>(model);
            _applicantRepository.Update(result);
            return true;
        }
        #endregion

        #region Helper Methods
        private string? SaveFile(IFormFile? formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }
            var rootPath = "PracticeApp/PracticeWebAppSLN/PracticeWebApp/wwwroot/UploadedFiles";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, rootPath);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                try
                {

                    formFile.CopyTo(fileStream);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }

            return uniqueFileName;
        }

        private T ExecuteWithTryCatch<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.", ex);
            }
        }

        private async Task<T> ExecuteWithTryCatchAsync<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing your request.", ex);
            }
        }
        #endregion
        #endregion

    }
}
