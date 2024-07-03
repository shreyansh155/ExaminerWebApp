using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ExaminerTypeRepository : IExaminerTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ExaminerTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<ExaminerType> GetList()
        {
            return _context.ExaminerTypes.ToList();
        }
    }
}
