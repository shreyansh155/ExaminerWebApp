using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ApplicantTypeRepository : IApplicantTypeRepository
    {
        private readonly ApplicationDbContext _context;
      
        public ApplicantTypeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public List<ApplicantType> GetList()
        {
            return _context.ApplicantTypes.OrderBy(x => x.Id).ToList();
        }
    }
}
