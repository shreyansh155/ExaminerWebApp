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

        public async Task<Phase> Update(Phase phase)
        {
            _context.Phases.Update(phase);
            await _context.SaveChangesAsync();
            return phase;
        }

        public async Task<int> Delete(int id)
        {
            Phase phase = await _context.Phases.FirstAsync(x => x.Id == id);
            phase.IsDeleted = true;
            _context.Phases.Update(phase);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Phase?> GetById(int id)
        {
            Phase phase = await _context.Phases.FirstAsync(x => x.Id == id);
            return phase.IsDeleted == true ? null : phase;
        }

        public Task<bool> CheckIfExists(int? id, string name)
        {
            if (id != null)
            {
                return _context.Phases.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id && x.IsDeleted != true).AnyAsync();
            }
            return _context.Phases.Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted != true).AnyAsync();
        }
    }
}