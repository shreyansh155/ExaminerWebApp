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

        public async Task<bool> Update(Step _object)
        {
            _context.Steps.Update(_object);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            Step step = await _context.Steps.Where(x => x.Id == id).FirstAsync();
            step.IsDeleted = true;
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Step> GetById(int id)
        {
            return await _context.Steps.FirstAsync(x => x.Id == id);
        }

        public async Task<List<StepType>> GetStepTypeList()
        {
            return await _context.StepTypes.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<bool> CheckIfStepExists(Step step)
        {
            return await _context.Steps.Where(x => x.PhaseId == step.PhaseId && x.Name.ToLower() == step.Name.ToLower()).AnyAsync();
        }

        public async Task<bool> CheckIfEditStepExists(Step step)
        {
            return await _context.Steps.Where(x => x.PhaseId == step.PhaseId && x.Id != step.Id && x.Name.ToLower() == step.Name.ToLower()).AnyAsync();
        }

        public async Task<bool> UpdateInstruction(int? stepId, string instruction)
        {
            Step? step = await _context.Steps.FirstOrDefaultAsync(x => x.Id == stepId);
            step.Instruction = instruction;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}