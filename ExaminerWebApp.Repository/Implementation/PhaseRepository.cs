using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Phase> Create(Phase phase)
        {
            await _context.Phases.AddAsync(phase);
            await _context.SaveChangesAsync();
            return phase;
        }

        public void Update(Phase phase)
        {
            _context.Phases.Update(phase);
            _context.SaveChanges();
        }

        public async Task<bool> Delete(int id)
        {
            Phase phase = await _context.Phases.FirstAsync(x => x.Id == id);
            phase.IsDeleted = true;
            _context.Phases.Update(phase);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Phase> GetById(int id)
        {
            return await _context.Phases.FirstAsync(x => x.Id == id); ;
        }

        public Task<bool> CheckIfPhaseExists(string phaseName)
        {
            return _context.Phases.Where(x => x.Name.ToLower() == phaseName.ToLower() && x.IsDeleted != true).AnyAsync();
        }

        public Task<bool> CheckPhaseOnUpdateExists(string phaseName, int? phaseId)
        {
            return _context.Phases.Where(x => x.Name.ToLower() == phaseName.ToLower() && x.Id != phaseId && x.IsDeleted != true).AnyAsync();
        }
    }
}
