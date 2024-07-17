﻿using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IStepRepository : IBaseRepository<Step>
    {
        IQueryable<Step> GetAllSteps(int phaseId);
     
        Task<List<StepType>> GetStepTypeList();
        
        Task<bool> CheckIfStepExists(int phaseId, string stepName);
    }
}