using AutoMapper;
using ExaminerWebApp.Entities.Entities;
using ExaminerWebApp.Repository.Interface;
using ExaminerWebApp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExaminerWebApp.Service.Implementation
{
    public class StepService : IStepService
    {
        public readonly IStepRepository _stepRepository;

        public readonly IMapper _mapper;

        public StepService(IStepRepository stepRepository, IMapper mapper)
        {
            _stepRepository = stepRepository;
            _mapper = mapper;
        }

        public IQueryable<Step> GetAll(int phaseId)
        {
            IQueryable<Repository.DataModels.Step> step =
                _stepRepository.GetAllSteps(phaseId)
                .Include(x => x.StepType)
                .AsQueryable();
            IQueryable<Step> steps = _mapper.ProjectTo<Step>(step);
            return steps;
        }

        public async Task<Step> GetStepById(int id)
        {
            Repository.DataModels.Step obj = await _stepRepository.GetById(id);
            Step step = _mapper.Map<Step>(obj);
            return step;
        }

        public async Task<Step> CreateStep(Step model)
        {
            Repository.DataModels.Step step = _mapper.Map<Repository.DataModels.Step>(model);
            await _stepRepository.Create(step);
            return model;
        }

        public async Task<bool> DeleteStep(int id)
        {
            return await _stepRepository.Delete(id);
        }

        public bool UpdateStep(Step step)
        {
            Repository.DataModels.Step steps = _mapper.Map<Repository.DataModels.Step>(step);
            _stepRepository.Update(steps);
            return true;
        }

        public async Task<List<StepType>> GetStepTypeList()
        {
            List<Repository.DataModels.StepType> stepType = await _stepRepository.GetStepTypeList();
            List<StepType> list = _mapper.Map<List<StepType>>(stepType);
            return list;
        }

        public async Task<IQueryable<Step>> GetStepByPhaseId(int phaseId)
        {
            IQueryable<Repository.DataModels.Step> steps = await Task.Run(() => _stepRepository.GetAllSteps(phaseId));
            IQueryable<Step> obj = _mapper.ProjectTo<Step>(steps);
            return obj;
        }

        public async Task<bool> CheckIfStepExists(int phaseId, string stepName)
        {
            return await _stepRepository.CheckIfStepExists(phaseId, stepName);
        }
    }
}