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
            return _object;
        }
        public void Update(Step _object)
        {
            _context.Steps.Update(_object);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Step step = _context.Steps.Where(x => x.Id == id).First();
            step.IsDeleted = true;
            _context.Update(id);
            _context.SaveChanges();
        }
        public Step GetById(int id)
        {
            return _context.Steps.First(x => x.Id == id);
        }
    }
}
