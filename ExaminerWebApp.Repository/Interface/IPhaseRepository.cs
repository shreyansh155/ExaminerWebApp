﻿using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseRepository : IBaseRepository<Phase>
    {
        IQueryable<Phase> GetAll();

        Task<bool> CheckIfExists(int? id, string name);
    }
}