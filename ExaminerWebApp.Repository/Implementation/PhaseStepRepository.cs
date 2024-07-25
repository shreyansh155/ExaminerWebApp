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
                .OrderBy(x => x.Ordinal)
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
            }).OrderBy(x => x.Ordinal).ToList();

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

            int ordinalNumber = 1;
            var result = allSteps.Select(s =>
            {
                var phaseStep = phaseSteps.FirstOrDefault(ps => ps.StepId == s.Id);
                return new
                {
                    Step = s,
                    IsInTemplatePhaseSteps = phaseStep != null,
                    Ordinal = ordinalNumber++,
                    TemplatePhaseId = templatePhase?.Id,
                    TemplatePhaseStepId = phaseStep?.Id
                };
            }).OrderBy(x => x.Ordinal).ToList();

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

        public async Task<List<Step>> GetStepList(int? templatePhaseStepId)
        {
            TemplatePhaseStep templatePhaseStep = await _context.TemplatePhaseSteps.FirstAsync(x => x.Id == templatePhaseStepId);

            Step step = await _context.Steps.FirstAsync(x => x.Id == templatePhaseStep.StepId);

            return await _context.Steps.Where(x => x.PhaseId == step.PhaseId && x.IsDeleted != true).OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<List<Step>> GetPhaseStepList(int? templatePhaseId)
        {

            var templatePhase = await _context.ApplicationTypeTemplatePhases
                                              .FirstOrDefaultAsync(x => x.Id == templatePhaseId);

            var phaseId = templatePhase?.PhaseId;

            var stepsInPhase = await _context.Steps
                                             .Where(s => s.PhaseId == phaseId && (s.IsDeleted == null || s.IsDeleted == false))
                                             .ToListAsync();

            // Fetch TemplatePhaseStep records to exclude from the steps
            var templatePhaseStepIds = await _context.TemplatePhaseSteps
                                                     .Where(tps => tps.TemplatePhaseId == templatePhaseId && tps.IsDeleted != true)
                                                     .Select(tps => tps.StepId)
                                                     .ToListAsync();

            // Exclude steps that are already in TemplatePhaseStep
            var stepsNotInTemplatePhaseStep = stepsInPhase
                                              .Where(s => !templatePhaseStepIds.Contains(s.Id))
                                              .ToList();

            return stepsNotInTemplatePhaseStep;
        }

        public async Task<TemplatePhaseStep> GetTemplatePhaseStep(int id)
        {
            return await _context.TemplatePhaseSteps.Include(tps => tps.Step).FirstAsync(x => x.Id == id);
        }

        public async Task<int> GetStepTypeId(int stepId)
        {
            Step step = await _context.Steps.FirstOrDefaultAsync(x => x.Id == stepId);
            return step.StepTypeId;
        }

        public async Task<TemplatePhaseStep> AddPhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            UpdateOrdinal(templatePhaseStep);

            ApplicationTypeTemplatePhase? templatePhase = await _context.ApplicationTypeTemplatePhases
                .FirstOrDefaultAsync(x => x.Id == templatePhaseStep.TemplatePhaseId);

            templatePhase.StepCount++;

            await _context.TemplatePhaseSteps.AddAsync(templatePhaseStep);

            await _context.SaveChangesAsync();

            return templatePhaseStep;
        }
        public async Task<TemplatePhaseStep> UpdatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            UpdateOrdinal(templatePhaseStep);
            _context.TemplatePhaseSteps.Update(templatePhaseStep);
            await _context.SaveChangesAsync();
            return templatePhaseStep;
        }

        public async Task<int> DeleteStep(int id)
        {
            TemplatePhaseStep templatePhaseStep = await _context.TemplatePhaseSteps.FirstAsync(x => x.Id == id);

            templatePhaseStep.IsDeleted = true;

            ApplicationTypeTemplatePhase? templatePhase = _context.ApplicationTypeTemplatePhases.FirstOrDefault(x => x.Id == templatePhaseStep.TemplatePhaseId);

            templatePhase.StepCount--;

            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<int> GetNewPhaseOrdinal(int templateId)
        {
            ApplicationTypeTemplatePhase? templatePhases = await _context.ApplicationTypeTemplatePhases.Where(x => x.TemplateId == templateId && x.IsDeleted != true).OrderBy(x => x.Ordinal).LastOrDefaultAsync();
            if (templatePhases == null)
            {
                return 1;
            }
            else
            {
                int ordinal = templatePhases.Ordinal + 1;
                return ordinal;
            }
        }

        public async Task<int> GetNewStepOrdinal(int templatePhaseId)
        {
            TemplatePhaseStep? templatePhasesStep = await _context.TemplatePhaseSteps
                .Where(x => x.TemplatePhaseId == templatePhaseId && x.IsDeleted != true)
                .OrderBy(x => x.Ordinal)
                .LastOrDefaultAsync();
            if (templatePhasesStep == null)
            {
                return 1;
            }
            else
            {
                int ordinal = templatePhasesStep.Ordinal + 1;
                return ordinal;
            }
        }

        public void UpdateOrdinal(TemplatePhaseStep templatePhaseStep)
        {
            var allTemplatePhaseSteps = _context.TemplatePhaseSteps
              .Where(x => x.TemplatePhaseId == templatePhaseStep.TemplatePhaseId && x.Id != templatePhaseStep.Id)
              .OrderBy(x => x.Ordinal)
              .ToList();

            var upperPhaseStep = allTemplatePhaseSteps
                .Where(tp => tp.Ordinal >= templatePhaseStep.Ordinal && tp.StepId != templatePhaseStep.StepId).ToList();

            var lowerPhaseStep = allTemplatePhaseSteps
                .Where(tp => tp.Ordinal < templatePhaseStep.Ordinal && tp.StepId != templatePhaseStep.StepId).ToList();

            for (int i = 0; i < upperPhaseStep.Count; i++)
            {
                if (i == 0 && upperPhaseStep[0].Ordinal > templatePhaseStep.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperPhaseStep[i].Ordinal++;
                    }
                    else if (i > 0 && upperPhaseStep[i - 1].Ordinal == upperPhaseStep[i].Ordinal)
                    {
                        upperPhaseStep[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = lowerPhaseStep.Count; i >= 0; i--)
            {
                if (i == lowerPhaseStep.Count - 1 && lowerPhaseStep[i].Ordinal < templatePhaseStep.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == lowerPhaseStep.Count - 1)
                    {
                        lowerPhaseStep[i].Ordinal--;
                    }
                    else if (i < lowerPhaseStep.Count - 1 && lowerPhaseStep[i - 1].Ordinal == lowerPhaseStep[i].Ordinal)
                    {
                        lowerPhaseStep[i].Ordinal--;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}