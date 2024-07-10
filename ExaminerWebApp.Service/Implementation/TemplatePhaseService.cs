using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;

namespace ExaminerWebApp.Service.Implementation
{
    public class TemplatePhaseService : ITemplatePhaseService
    {
        private readonly IMapper _mapper;
        private readonly ITemplatePhaseRepository _templatePhaseRepository;
        public TemplatePhaseService(ITemplatePhaseRepository templatePhaseRepository, IMapper mapper)
        {
            _mapper = mapper;
            _templatePhaseRepository = templatePhaseRepository;
        }
        public ApplicationTypeTemplatePhase AddTemplatePhase(ApplicationTypeTemplatePhase model)
        {
            Repository.DataModels.ApplicationTypeTemplatePhase obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplatePhase>(model);
            _templatePhaseRepository.AddPhaseWithOrdinal(obj);
            return model;
        }
        public bool UpdateOrdinal(int templateId, int phaseId, int ordinal)
        {
            _templatePhaseRepository.UpdateOrdinal(templateId, phaseId, ordinal);
            return true;
        }
    }
}