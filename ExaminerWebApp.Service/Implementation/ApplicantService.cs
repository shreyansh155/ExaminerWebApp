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

        public ApplicantService(IApplicantRepository applicant, IMapper mapper, IWebHostEnvironment environment)
        {
            _environment = environment;
            _applicantRepository = applicant;
            _mapper = mapper;
        }

        public async Task<Entities.Entities.Applicant> GetApplicantById(int id)
        {
            ExaminerWebApp.Repository.DataModels.Applicant result = await _applicantRepository.GetById(id);
            return _mapper.Map<Entities.Entities.Applicant>(result);
        }

        public IQueryable<Entities.Entities.Applicant> GetAllApplicants()
        {
            var list = _applicantRepository.GetAll().Include(x => x.ApplicantType).AsQueryable();
            var obj = _mapper.ProjectTo<Entities.Entities.Applicant>(list);
            return obj;
        }

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

        public bool CheckEmailIfExists(string email)
        {
            return _applicantRepository.CheckEmail(email);
        }

        public async Task<bool> DeleteApplicant(int id)
        {
            await _applicantRepository.Delete(id);
            return true;
        }

        public async Task<bool> UpdateApplicant(Entities.Entities.Applicant model)
        {
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                model.FilePath = SaveFile(model.FormFile);
            }
            var result = _mapper.Map<Repository.DataModels.Applicant>(model);
            await _applicantRepository.Update(result);
            return true;
        }

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
                formFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}