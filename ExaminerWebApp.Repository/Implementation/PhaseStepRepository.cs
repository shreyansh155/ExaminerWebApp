using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.Interface;
using Kendo.Mvc.UI;

namespace ExaminerWebApp.Repository.Implementation
{
    public class PhaseStepRepository : IPhaseStepRepository
    {
        private readonly ApplicationDbContext _context;
        public PhaseStepRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public object PhaseStep(object _obj)
        {

            using (_context)
            {
                var steps = _context.Steps.Where(_ => _.IsDeleted != true).ToList();
                var phaseSteps = _context.TemplatePhaseSteps.Where(x => x.IsDeleted != true).Select(
                    x => new
                    {
                        x.Step,
                        x.Id,
                    }).ToList();

                 
                return phaseSteps;
            }

        }
    }
}
