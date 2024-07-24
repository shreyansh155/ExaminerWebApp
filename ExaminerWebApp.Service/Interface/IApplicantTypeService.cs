using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface 
{ 
    public interface IApplicantTypeService
    {
        Task<List<ApplicantType>> GetApplicantTypeList();
    }
}