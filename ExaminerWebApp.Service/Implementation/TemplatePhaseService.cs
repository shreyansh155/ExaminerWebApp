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
        private readonly IStepRepository _stepRepository;

        public TemplatePhaseService(ITemplatePhaseRepository templatePhaseRepository, IMapper mapper, IPhaseStepRepository phaseStepRepository, IStepRepository stepRepository)
        {
            _mapper = mapper;
            _templatePhaseRepository = templatePhaseRepository;
            _phaseStepRepository = phaseStepRepository;
            _stepRepository = stepRepository;
        }

        public async Task<ApplicationTypeTemplatePhase> AddTemplatePhase(ApplicationTypeTemplatePhase model)
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

        public async Task<TemplatePhaseStep> GetTemplatePhaseStep(int id)
        {
            Repository.DataModels.TemplatePhaseStep templatePhaseStep = await _phaseStepRepository.GetTemplatePhaseStep(id);
            return _mapper.Map<TemplatePhaseStep>(templatePhaseStep);
        }

        public async Task<int> GetStepTypeId(int stepId)
        {
            return await _phaseStepRepository.GetStepTypeId(stepId);
        }

        public async Task<TemplatePhaseStep> AddTemplatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            Repository.DataModels.TemplatePhaseStep phaseStep = _mapper.Map<Repository.DataModels.TemplatePhaseStep>(templatePhaseStep);
            await _phaseStepRepository.AddPhaseStep(phaseStep);
            if (templatePhaseStep.Instruction != "")
            {
                await _stepRepository.UpdateInstruction(templatePhaseStep.StepId, templatePhaseStep.Instruction ?? "");
            }
            return templatePhaseStep;
        }

        public async Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep)
        {
            Repository.DataModels.TemplatePhaseStep phaseStep = _mapper.Map<Repository.DataModels.TemplatePhaseStep>(templatePhaseStep);
            await _phaseStepRepository.UpdatePhaseStep(phaseStep);
            if (templatePhaseStep.Instruction != "")
            {
                await _stepRepository.UpdateInstruction(templatePhaseStep.StepId, templatePhaseStep.Instruction ?? "");
            }
            return templatePhaseStep;
        }

        public async Task<bool> DeleteStep(int id)
        {
            return await _phaseStepRepository.DeleteStep(id);
        }
        public async Task<int> GetNewPhaseOrdinal(int templateId)
        {
            int ordinal = await _phaseStepRepository.GetNewPhaseOrdinal(templateId);
            return ordinal;
        }

        public async Task<int> GetNewStepOrdinal(int templatePhaseId)
        {
            int ordinal = await _phaseStepRepository.GetNewStepOrdinal(templatePhaseId);
            return ordinal;
        }
    }
}