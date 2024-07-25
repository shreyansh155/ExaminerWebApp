using AutoMapper;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using ExaminerWebApp.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ExaminerWebApp.Composition.Helpers;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicantService : BaseService, IApplicantService
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
            Repository.DataModels.Applicant result = await _applicantRepository.GetById(id);
            return _mapper.Map<Entities.Entities.Applicant>(result);
        }

        public async Task<PaginationSet<Entities.Entities.Applicant>> GetAllApplicants(PaginationSet<Entities.Entities.Applicant> pager)
        {
            IQueryable<Repository.DataModels.Applicant> list = _applicantRepository.GetAll();

            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    list = ApplyFilter<Repository.DataModels.Applicant>(list, filter);
                }
            }

            //sorting ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    list = ApplySorting<Repository.DataModels.Applicant>(list, sort);
                }
            }

            var obj = _mapper.ProjectTo<Entities.Entities.Applicant>(list);

            pager.Items = await obj.Skip(pager.Skip).Take(pager.Take).ToListAsync();

            pager.TotalCount = obj.Count();

            return pager;
        }

        public async Task<Entities.Entities.Applicant> AddApplicant(Entities.Entities.Applicant applicant)
        {

            var obj = _mapper.Map<Repository.DataModels.Applicant>(applicant);
            if (applicant.FormFile != null && applicant.FormFile.Length != 0)
            {
                obj.FilePath = SaveFile(applicant.FormFile);
            }
            await _applicantRepository.Create(obj);
            return applicant;
        }

        public async Task<bool> CheckEmailIfExists(string email)
        {
            return await _applicantRepository.CheckEmail(email);
        }

        public async Task<int> DeleteApplicant(int id)
        {
            return await _applicantRepository.Delete(id);
        }

        public async Task<Entities.Entities.Applicant> UpdateApplicant(Entities.Entities.Applicant applicant)
        {
            if (applicant.FormFile != null && applicant.FormFile.Length > 0)
            {
                applicant.FilePath = SaveFile(applicant.FormFile);
            }
            var result = _mapper.Map<Repository.DataModels.Applicant>(applicant);
            return _mapper.Map<Entities.Entities.Applicant>(await _applicantRepository.Update(result));
        }

        private string? SaveFile(IFormFile? formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }
            var rootPath = "UploadedFiles";
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