using ExaminerWebApp.Repository.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IPhaseRepository : IBaseRepository<Phase>
    {
        IQueryable<Phase> GetAll();
    }
}
