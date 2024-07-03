using ExaminerWebApp.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExaminerWebApp.Service.Interface
{
    public interface IExaminerTypeService
    {
        List<ExaminerType> GetExaminerTypeList();
    }
}
