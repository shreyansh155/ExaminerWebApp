using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Repository.Implementation
{
    public class PhaseStepRepository : IPhaseStepRepository
    {
        private readonly ApplicationDbContext _context;
        public PhaseStepRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<object> GetPhaseStepsAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId)
        {
            ApplicationTypeTemplatePhase? templatePhase = templatePhases
                .FirstOrDefault(x => x.PhaseId == phaseId && x.TemplateId == templateId);

            var allSteps = await _context.Steps
                .Where(s => s.IsDeleted != true && s.PhaseId == phaseId)
                .ToListAsync();

            var phaseSteps = await _context.TemplatePhaseSteps
                .Where(tps => tps.TemplatePhase.TemplateId == templateId &&
                              tps.TemplatePhase.PhaseId == phaseId &&
                              tps.IsDeleted != true)
                .Select(tps => new
                {
                    tps.Id,
                    tps.StepId,
                    tps.Ordinal,
                    tps.TemplatePhaseId
                })
                .ToListAsync();

            // Filter allSteps to include only those whose ID is in the phaseSteps list
            var filteredSteps = allSteps.Where(s => phaseSteps.Any(ps => ps.StepId == s.Id)).ToList();

            var result = filteredSteps.Select(s =>
            {
                var phaseStep = phaseSteps.FirstOrDefault(ps => ps.StepId == s.Id);
                return new
                {
                    Step = s,
                    IsInTemplatePhaseSteps = phaseStep != null,
                    phaseStep?.Ordinal,
                    TemplatePhaseId = templatePhase?.Id,
                    TemplatePhaseStepId = phaseStep?.Id
                };
            }).ToList();

            return result;
        }

        public async Task<object> GetPhaseStepsByTemplateAsync(IEnumerable<ApplicationTypeTemplatePhase> templatePhases, int templateId, int phaseId)
        {
            ApplicationTypeTemplatePhase? templatePhase = templatePhases
                .FirstOrDefault(x => x.PhaseId == phaseId && x.TemplateId == templateId);

            var allSteps = await _context.Steps
                .Where(s => s.IsDeleted != true && s.PhaseId == phaseId)
                .ToListAsync();

            var phaseSteps = await _context.TemplatePhaseSteps
                .Where(tps => tps.TemplatePhase.TemplateId == templateId &&
                              tps.TemplatePhase.PhaseId == phaseId &&
                              tps.IsDeleted != true)
                .Select(tps => new
                {
                    tps.Id,
                    tps.StepId,
                    tps.Ordinal,
                    tps.TemplatePhaseId
                })
                .ToListAsync();

            var result = allSteps.Select(s =>
            {
                var phaseStep = phaseSteps.FirstOrDefault(ps => ps.StepId == s.Id);
                return new
                {
                    Step = s,
                    IsInTemplatePhaseSteps = phaseStep != null,
                    phaseStep?.Ordinal,
                    TemplatePhaseId = templatePhase?.Id,
                    TemplatePhaseStepId = phaseStep?.Id
                };
            }).ToList();

            return result;
        }

        public List<TemplatePhaseStep> AddStepsWithOrdinal(List<TemplatePhaseStep> model, ApplicationTypeTemplatePhase tempPhase)
        {
            foreach (var item in model)
            {
                item.TemplatePhaseId = tempPhase.Id;
                _context.TemplatePhaseSteps.Add(item);
            }
            _context.SaveChanges();
            return model;
        }
    }
}
