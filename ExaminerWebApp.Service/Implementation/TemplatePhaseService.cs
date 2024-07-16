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
        private readonly IPhaseStepRepository _phaseStepRepository;

        public TemplatePhaseService(ITemplatePhaseRepository templatePhaseRepository, IMapper mapper, IPhaseStepRepository phaseStepRepository)
        {
            _mapper = mapper;
            _templatePhaseRepository = templatePhaseRepository;
            _phaseStepRepository = phaseStepRepository;
        }

        public ApplicationTypeTemplatePhase AddTemplatePhaseStep(ApplicationTypeTemplatePhase model)
        {
            Repository.DataModels.ApplicationTypeTemplatePhase obj = _mapper.Map<Repository.DataModels.ApplicationTypeTemplatePhase>(model);

            List<Repository.DataModels.TemplatePhaseStep> templatePhaseStep = _mapper.Map<List<Repository.DataModels.TemplatePhaseStep>>(model.TemplatePhaseSteps);

            Repository.DataModels.ApplicationTypeTemplatePhase tempPhase = _templatePhaseRepository.AddPhaseWithOrdinal(obj);

            _phaseStepRepository.AddStepsWithOrdinal(templatePhaseStep, tempPhase);

            return model;
        }

        public async Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal)
        {
            await _templatePhaseRepository.UpdateOrdinal(templatePhaseId, ordinal);

            return true;
        }
        public TemplatePhaseStep GetTemplatePhaseStep(int id)
        {
            Repository.DataModels.TemplatePhaseStep templatePhaseStep = _phaseStepRepository.GetTemplatePhaseStep(id);
            TemplatePhaseStep obj = _mapper.Map<TemplatePhaseStep>(templatePhaseStep);
            return obj;
        }

        public int GetStepTypeId(int stepId)
        {
            return _phaseStepRepository.GetStepTypeId(stepId);
        }

        public async Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            Repository.DataModels.TemplatePhaseStep phaseStep = _mapper.Map<Repository.DataModels.TemplatePhaseStep>(templatePhaseStep);
            await _phaseStepRepository.UpdatePhaseStep(phaseStep);
            return templatePhaseStep;
        }
        
        public async Task<bool> DeleteStep(int id)
        {
            return await _phaseStepRepository.DeleteStep(id);
        }
    }
}