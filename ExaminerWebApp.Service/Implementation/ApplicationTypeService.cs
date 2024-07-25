using AutoMapper;
using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicationTypeService : BaseService, IApplicationTypeService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationTypeTemplateRepository _applicationTypeRepository;
        private readonly IPhaseRepository _phaseRepository;
        private readonly ITemplatePhaseRepository _templatePhaseRepository;
        private readonly IPhaseStepRepository _phaseStepRepository;

        public ApplicationTypeService(IMapper mapper, IApplicationTypeTemplateRepository applicationTypeRepository, IPhaseRepository phaseRepository, ITemplatePhaseRepository templatePhaseRepository, IPhaseStepRepository phaseStepRepository)
        {
            _phaseRepository = phaseRepository;
            _applicationTypeRepository = applicationTypeRepository;
            _mapper = mapper;
            _phaseStepRepository = phaseStepRepository;
            _templatePhaseRepository = templatePhaseRepository;
        }

        public async Task<PaginationSet<ApplicationTypeTemplate>> GetAll(PaginationSet<ApplicationTypeTemplate> pager)
        {
            var list = _applicationTypeRepository.GetAll().AsQueryable();

            //filtering based on search fields
            if (pager.Filter != null && pager.Filter.Filters != null && pager.Filter.Filters.Count > 0)
            {
                foreach (var filter in pager.Filter.Filters)
                {
                    if (filter.Field != "id")
                    {
                        list = ApplyFilter(list, filter);
                    }
                }
            }

            //sorting ascending descending
            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    list = ApplySorting(list, sort);
                }
            }
            var obj = _mapper.ProjectTo<ApplicationTypeTemplate>(list);

            pager.Items = await obj.Skip(pager.Skip).Take(pager.Take).ToListAsync();

            pager.TotalCount = await obj.CountAsync();

            return pager;
        }

        public async Task<ApplicationTypeTemplate> GetById(int id)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = await _applicationTypeRepository.GetById(id);
            var result = _mapper.Map<ApplicationTypeTemplate>(obj);
            return result;
        }

        public async Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model)
        {
            model.CreatedBy = "1";
            model.CreatedDate = DateTime.UtcNow;
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);
            await _applicationTypeRepository.Create(obj);
            return model;
        }
 
        public async Task<bool> CheckIfExists(int? id, string applicationName)
        {
            return await _applicationTypeRepository.CheckIfExists(id, applicationName);
        }

        public async Task<int> DeleteTemplate(int id)
        {
            return await _applicationTypeRepository.Delete(id);
        }

        public async Task<ApplicationTypeTemplate> Update(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);

            await _applicationTypeRepository.Update(obj);

            return model;
        }

        public async Task<IQueryable<Phase>> PhaseList(int templateId)
        {
            var templatePhaseIds = await _templatePhaseRepository.GetAllTemplates(templateId)
                .Select(tp => tp.PhaseId)
                .ToListAsync();

            IQueryable<Repository.DataModels.Phase> filteredPhases = _phaseRepository
                .GetAll()
                .Where(p => !templatePhaseIds.Contains(p.Id));

            return _mapper.ProjectTo<Phase>(filteredPhases);
        }

        public async Task<PaginationSet<object>> GetPhaseByTemplate(int templateId, PaginationSet<object> pager)
        {
            IQueryable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.TemplatePhases(templateId);

            IQueryable<ApplicationTypeTemplatePhase> templatePhaseList = _mapper.Map<List<ApplicationTypeTemplatePhase>>(templatePhases).AsQueryable();

            if (pager.Sort != null && pager.Sort.Count > 0)
            {
                foreach (var sort in pager.Sort)
                {
                    templatePhaseList = ApplySorting(templatePhaseList, sort);
                }
            }
            pager.Items = await templatePhaseList.Skip(pager.Skip).Take(pager.Take).ToDynamicListAsync();

            pager.TotalCount = templatePhaseList.Count();

            return pager;
        }

        public async Task<object> GetPhaseStepsByTemplateAsync(int templateId, int phaseId)
        {
            IEnumerable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.PhaseStepsByTemplateAsync(templateId, phaseId);

            var phaseSteps = await _phaseStepRepository.GetPhaseStepsByTemplateAsync(templatePhases, templateId, phaseId);

            return phaseSteps;
        }

        public async Task<object> GetPhaseStepsAsync(int templateId, int phaseId)
        {
            IEnumerable<Repository.DataModels.ApplicationTypeTemplatePhase> templatePhases = await _templatePhaseRepository.PhaseStepsAsync(templateId, phaseId);

            var phaseSteps = await _phaseStepRepository.GetPhaseStepsAsync(templatePhases, templateId, phaseId);

            return phaseSteps;
        }

        public async Task<List<Step>> StepList(int? templatePhaseStepId)
        {
            List<Repository.DataModels.Step> steps = await _phaseStepRepository.GetStepList(templatePhaseStepId);
            List<Step> step = _mapper.Map<List<Step>>(steps);
            return step;
        }

        public async Task<List<Step>> PhaseStepList(int? templatePhaseId)
        {
            List<Repository.DataModels.Step> steps = await _phaseStepRepository.GetPhaseStepList(templatePhaseId);
            List<Step> step = _mapper.Map<List<Step>>(steps);
            return step;
        }
    }
}