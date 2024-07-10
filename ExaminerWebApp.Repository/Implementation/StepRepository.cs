using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class StepRepository : IStepRepository
    {
        private readonly ApplicationDbContext _context;
        public StepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Step> GetAllSteps(int phaseId)
        {
            return _context.Steps.Where(x => x.PhaseId == phaseId && x.IsDeleted != true).AsQueryable();
        }
        public async Task<Step> Create(Step _object)
        {
            await _context.Steps.AddAsync(_object);
            _context.SaveChanges();
            return _object;
        }
        public void Update(Step _object)
        {
            var a = _context.Steps.Update(_object);
            var obj = _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Step step = _context.Steps.Where(x => x.Id == id).First();
            step.IsDeleted = true;
            _context.Steps.Update(step);
            _context.SaveChanges();
        }
        public Step GetById(int id)
        {
            return _context.Steps.First(x => x.Id == id);
        }
        public List<StepType> GetStepTypeList()
        {
            return _context.StepTypes.OrderBy(x => x.Id).ToList();
        }

        public bool CheckIfStepExists(int phaseId, string stepName)
        {
            return _context.Steps.Where(x => x.PhaseId == phaseId && x.Name.ToLower() == stepName.ToLower()).Any();
        }
    }
}
