using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class ApplicationTypeService : IApplicationTypeService
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

        public IQueryable<ApplicationTypeTemplate> GetAll(string s)
        {
            var list = _applicationTypeRepository.GetAll(s).AsQueryable();
            var obj = _mapper.ProjectTo<ApplicationTypeTemplate>(list);
            return obj;
        }

        public async Task<ApplicationTypeTemplate> GetById(int id)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = await _applicationTypeRepository.GetById(id);
            var result = _mapper.Map<ApplicationTypeTemplate>(obj);
            return result;
        }

        public async Task<ApplicationTypeTemplate> Add(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);
            await _applicationTypeRepository.Create(obj);
            return model;
        }

        public bool ApplicationTemplateExists(string applicationName)
        {
            return _applicationTypeRepository.ApplicationTemplateExists(applicationName);
        }
        public async Task<bool> EditApplicationTemplateExists(int? id, string applicationName)
        {
            return await _applicationTypeRepository.EditApplicationTemplateExists(id, applicationName);
        }

        public async Task<bool> Delete(int id)
        {
            return await _templatePhaseRepository.Delete(id);
        }
        public async Task<bool> DeleteTemplate(int id)
        {
            return await _applicationTypeRepository.Delete(id);
        }

        public bool Update(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);

            _applicationTypeRepository.Update(obj);

            return true;
        }

        public IQueryable<Phase> PhaseList(int templateId)
        {
            IQueryable<int> templatePhaseIds = _templatePhaseRepository
                .GetAllTemplates(templateId)
                .Select(tp => tp.PhaseId);

            IQueryable<Repository.DataModels.Phase> filteredPhases = _phaseRepository
                .GetAll()
                .Where(p => !templatePhaseIds.Contains(p.Id));

            IQueryable<Phase> list = _mapper.ProjectTo<Phase>(filteredPhases);

            return list;
        }

        public object GetPhaseByTemplate(int templateId)
        {
            object templatePhases = _templatePhaseRepository.TemplatePhases(templateId);
            return templatePhases;
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