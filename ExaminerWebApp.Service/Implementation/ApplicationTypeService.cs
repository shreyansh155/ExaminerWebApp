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

        public bool Delete(int id)
        {
            _applicationTypeRepository.Delete(id);
            return true;
        }

        public bool Update(ApplicationTypeTemplate model)
        {
            Repository.DataModels.ApplicationTypeTemplate obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplate>(model);
            try
            {
                _applicationTypeRepository.Update(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
    }
}
