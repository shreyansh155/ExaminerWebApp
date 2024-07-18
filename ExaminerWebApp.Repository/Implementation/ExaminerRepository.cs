using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ExaminerRepository : IExaminerRepository
    {
        private readonly ApplicationDbContext _context;

        public ExaminerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Examiner> Create(Examiner model)
        {
            await _context.Examiners.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(int id)
        {
            var applicant = await _context.Examiners.FirstAsync(x => x.Id == id);
            applicant.IsDeleted = true;
            _context.Examiners.Update(applicant);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Examiner> GetAll()
        {
            return _context.Examiners.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

        public async Task<Examiner> GetById(int id)
        {
            return await _context.Examiners.FirstAsync(x => x.Id == id);
        }

        public void Update(Examiner model)
        {
            _context.Examiners.Update(model);
            _context.SaveChanges();
        }

        public bool CheckEmail(string email)
        {
            return _context.Examiners.Where(x => x.Email == email && x.IsDeleted != true).Any();
        }
    }
}