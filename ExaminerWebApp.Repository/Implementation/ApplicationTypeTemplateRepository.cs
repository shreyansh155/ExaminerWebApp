using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class ApplicationTypeTemplateRepository : IApplicationTypeTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationTypeTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<ApplicationTypeTemplate> GetAll()
        {
            return _context.ApplicationTypeTemplates.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }
    }
}
