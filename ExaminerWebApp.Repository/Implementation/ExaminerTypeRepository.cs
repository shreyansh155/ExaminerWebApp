using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ExaminerTypeRepository : IExaminerTypeRepository
    {
        private readonly ApplicationDbContext _context;
    
        public ExaminerTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
     
        public async Task<List<ExaminerType>> GetList()
        {
            return await _context.ExaminerTypes.ToListAsync();
        }
    }
}