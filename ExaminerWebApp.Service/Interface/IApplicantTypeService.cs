using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface 
{ 
    public interface IApplicantTypeService
    {
        List<ApplicantType> GetApplicantTypeList();
    }
}