using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface IApplicantTypeRepository
    {
        List<ApplicantType> GetList();
    }
}