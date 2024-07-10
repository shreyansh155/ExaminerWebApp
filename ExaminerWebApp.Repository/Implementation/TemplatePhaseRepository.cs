using ExaminerWebApp.Repository.DataContext;
using ExaminerWebApp.Repository.DataModels;
using ExaminerWebApp.Repository.Interface;
using static System.Net.WebRequestMethods;

namespace ExaminerWebApp.Repository.Implementation
{
    public class TemplatePhaseRepository : ITemplatePhaseRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatePhaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Object TemplatePhases(int templateId)
        {
            using (_context)
            {
                var phasesWithStepCounts = _context.ApplicationTypeTemplatePhases
                    .Where(attp => attp.TemplateId == templateId && attp.IsDeleted != true)
                    .Select(attp => new
                    {
                        attp.Ordinal,
                        attp.Phase,
                        StepCount = attp.Phase.Steps.Count(),
                    })
                    .OrderBy(x => x.Ordinal)
                    .ToList();

                return phasesWithStepCounts;
            }
        }
        public Object PhaseSteps(int templateId, int phaseId)
        {
            using (_context)
            {
                var phasesWithStepCounts = _context.ApplicationTypeTemplatePhases
                    .Where(attp => attp.TemplateId == templateId && attp.PhaseId == phaseId && attp.IsDeleted != true)
                    .Select(attp => new
                    {
                        attp.Ordinal,
                        attp.Phase,
                        StepCount = attp.Phase.Steps.Count(),
                    })
                    .OrderBy(x => x.Ordinal)
                    .ToList();

                return phasesWithStepCounts;
            }
        }

        public async Task<ApplicationTypeTemplatePhase> Create(ApplicationTypeTemplatePhase _object)
        {
            await _context.ApplicationTypeTemplatePhases.AddAsync(_object);
            await _context.SaveChangesAsync();
            return _object;
        }

        public void Update(ApplicationTypeTemplatePhase _object) { }

        public void Delete(int id) { }

        public ApplicationTypeTemplatePhase GetById(int id)
        {
            return _context.ApplicationTypeTemplatePhases.Where(x => x.Id == id).First();
        }

        public IQueryable<ApplicationTypeTemplatePhase> GetAllTemplates(int templateId)
        {
            return _context.ApplicationTypeTemplatePhases.Where(x => x.TemplateId == templateId).AsQueryable();
        }

        public void AddPhaseWithOrdinal(ApplicationTypeTemplatePhase model)
        {
            var allPhases = GetAllTemplates(model.TemplateId)
                .OrderBy(x => x.Ordinal)
                .ToList();

            var upperPhase = allPhases
                .Where(tp => tp.Ordinal >= model.Ordinal)
                .ToList();

            var lowerPhase = allPhases
                .Where(tp => tp.Ordinal < model.Ordinal)
                .ToList();

            for (int i = 0; i < upperPhase.Count; i++)
            {
                if (i == 0 && upperPhase[0].Ordinal > model.Ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperPhase[i - 1].Ordinal == upperPhase[i].Ordinal)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            //for (int i = lowerPhase.Count; i >= 0; i--)
            //{
            //    if (lowerPhase[lowerPhase.Count - 1].Ordinal < model.Ordinal)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        if (i > 0 && lowerPhase[i + 1].Ordinal++ == lowerPhase[i].Ordinal)
            //        {
            //            lowerPhase[i].Ordinal++;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}

            _context.SaveChanges();

            var newPhase = new ApplicationTypeTemplatePhase
            {
                TemplateId = model.TemplateId,
                PhaseId = model.PhaseId,
                Ordinal = model.Ordinal,
                IsDeleted = false
            };

            _context.Add(newPhase);
            _context.SaveChanges();
        }
        public bool UpdateOrdinal(int templateId, int phaseId, int ordinal)
        {
            var allPhases = GetAllTemplates(templateId).OrderBy(x => x.Ordinal).ToList();

            var currentPhase = allPhases.First(tp => tp.PhaseId == phaseId);
            currentPhase.Ordinal = ordinal;

            var upperPhase = allPhases.Where(tp => tp.Ordinal >= currentPhase.Ordinal && tp.PhaseId != phaseId).ToList();
            var lowerPhase = allPhases.Where(tp => tp.Ordinal < currentPhase.Ordinal && tp.PhaseId != phaseId).ToList();

            for (int i = 0; i < upperPhase.Count; i++)
            {
                if (i == 0 && upperPhase[0].Ordinal > ordinal)
                {
                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else if (i > 0 && upperPhase[i - 1].Ordinal == upperPhase[i].Ordinal)
                    {
                        upperPhase[i].Ordinal++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = lowerPhase.Count; i >= 0; i--)
            {
                if (i == lowerPhase.Count - 1 && lowerPhase[i].Ordinal < ordinal)
                {
                    break;
                }
                else
                {
                    if (i == lowerPhase.Count - 1)
                    {
                        lowerPhase[i].Ordinal--;
                    }
                    else if (i < lowerPhase.Count - 1 && lowerPhase[i - 1].Ordinal == lowerPhase[i].Ordinal)
                    {
                        lowerPhase[i].Ordinal--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            _context.SaveChanges();
            return true;
        }
    }
}

