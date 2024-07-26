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

        public async Task<int> Delete(int id)
        {
            var applicant = await _context.Examiners.FirstAsync(x => x.Id == id);
            applicant.IsDeleted = true;
            _context.Examiners.Update(applicant);
            await _context.SaveChangesAsync();
            return id;
        }

        public IQueryable<Examiner> GetAll()
        {
            return _context.Examiners.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

        public async Task<Examiner?> GetById(int id)
        {
            Examiner examiner = await _context.Examiners.FirstAsync(x => x.Id == id);
            return examiner.IsDeleted == true ? null : examiner;
        }

        public async Task<Examiner> Update(Examiner model)
        {
            _context.Examiners.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public bool CheckEmail(string email)
        {
            return _context.Examiners.Where(x => x.Email == email && x.IsDeleted != true).Any();
        }
    }
}