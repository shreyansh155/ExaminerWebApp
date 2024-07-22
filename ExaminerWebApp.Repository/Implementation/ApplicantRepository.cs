using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Repository.DataContext;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Applicant> Create(Applicant model)
        {
            await _context.Applicants.AddAsync(model);
            _context.SaveChanges();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var applicant = await _context.Applicants.FirstAsync(x => x.Id == id);
            applicant.IsDeleted = true;

            _context.Applicants.Update(applicant);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Applicant> GetAll()
        {
            return _context.Applicants.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

        public async Task<Applicant> GetById(int id)
        {
            return await _context.Applicants.FirstAsync(x => x.Id == id);
        }

        public async Task<bool> Update(Applicant model)
        {
            var applicant = _context.Applicants.First(x => x.Id == model.Id);

            applicant.FirstName = model.FirstName;
            applicant.LastName = model.LastName;
            applicant.Email = model.Email;
            applicant.Phone = model.Phone;
            applicant.DateOfBirth = model.DateOfBirth;
            applicant.FilePath = model.FilePath ?? applicant.FilePath;
            applicant.FileName = applicant.FileName;
            applicant.ApplicantTypeId = model.ApplicantTypeId;

            _context.Applicants.Update(applicant);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool CheckEmail(string email)
        {
            return _context.Applicants.Where(x => x.Email == email && x.IsDeleted != true).Any();
        }
    }
}