using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ApplicantTypeRepository : IApplicantTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicantTypeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<ApplicantType>> GetList()
        {
            return await _context.ApplicantTypes.OrderBy(x => x.Id).ToListAsync();
        }
    }
}