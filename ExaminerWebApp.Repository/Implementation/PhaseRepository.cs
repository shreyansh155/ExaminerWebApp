using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;

namespace ExaminerWebApp.Repository.Implementation
{
    public class PhaseRepository : IPhaseRepository
    {
        private readonly ApplicationDbContext _context;
        public PhaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Phase> GetAll()
        {
            return _context.Phases.Where(x => x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }
        public async Task<Phase> Create(Phase model)
        {
            await _context.Phases.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }
        public void Update(Phase model) { }
        public void Delete(int id)
        {
            Phase phase = _context.Phases.First(x => x.Id == id);
            phase.IsDeleted = true;
            _context.Phases.Update(phase);
            _context.SaveChanges();
        }
        public Phase GetById(int id)
        {
            return _context.Phases.First(x => x.Id == id); ;
        }
    }
}
