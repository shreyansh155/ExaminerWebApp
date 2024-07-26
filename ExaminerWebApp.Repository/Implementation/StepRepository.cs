using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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
            return _context.Steps.Where(x => x.PhaseId == phaseId && x.IsDeleted != true).OrderBy(x => x.Id).AsQueryable();
        }

        public async Task<Step> Create(Step _object)
        {
            await _context.Steps.AddAsync(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<Step> Update(Step _object)
        {
            _context.Steps.Update(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public async Task<int> Delete(int id)
        {
            Step step = await _context.Steps.Where(x => x.Id == id).FirstAsync();
            step.IsDeleted = true;
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<Step?> GetById(int id)
        {
            Step step = await _context.Steps.FirstAsync(x => x.Id == id);

            if (step.IsDeleted == true)
                return null;

            return step.IsDeleted == true ? null : step;
        }

        public async Task<List<StepType>> GetStepTypeList()
        {
            return await _context.StepTypes.OrderBy(x => x.Id).ToListAsync();
        }

        public Task<bool> CheckIfExists(int? id, string name)
        {
            if (id != null)
            {
                return _context.Steps.Where(x => x.Name.ToLower() == name.ToLower() && x.Id != id && x.IsDeleted != true).AnyAsync();
            }
            return _context.Steps.Where(x => x.Name.ToLower() == name.ToLower() && x.IsDeleted != true).AnyAsync();
        }

        public async Task<bool> UpdateInstruction(int? stepId, string instruction)
        {
            Step step = await _context.Steps.FirstAsync(x => x.Id == stepId);
            step.Instruction = instruction;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}