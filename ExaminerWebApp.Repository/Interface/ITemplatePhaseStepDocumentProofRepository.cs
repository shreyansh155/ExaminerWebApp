using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface ITemplatePhaseStepDocumentProofRepository : IBaseRepository<TemplatePhaseStepDocumentProof>
    {
        IQueryable<TemplatePhaseStepDocumentProof> GetAll(int? tpsId);
    }
}
