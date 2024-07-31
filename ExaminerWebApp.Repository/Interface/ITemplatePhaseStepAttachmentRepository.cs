using ExaminerWebApp.Repository.DataModels;

namespace ExaminerWebApp.Repository.Interface
{
    public interface ITemplatePhaseStepAttachmentRepository : IBaseRepository<TemplatePhaseStepAttachment>
    {
        IQueryable<TemplatePhaseStepAttachment> GetAll(int? tpsId);

        Task<List<DocumentFileType>> GetDocumentTypes();
    }
}