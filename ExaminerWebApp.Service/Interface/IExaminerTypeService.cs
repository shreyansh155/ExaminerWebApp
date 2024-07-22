using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface IExaminerTypeService
    {
        Task<List<ExaminerType>> GetExaminerTypeList();
    }
}