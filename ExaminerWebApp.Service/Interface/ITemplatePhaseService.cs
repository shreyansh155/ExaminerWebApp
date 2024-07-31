using ExaminerWebApp.Composition.Helpers;
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.Service.Interface
{
    public interface ITemplatePhaseService
    {
        Task<ApplicationTypeTemplatePhase> AddTemplatePhase(ApplicationTypeTemplatePhase model);

        Task<bool> UpdateOrdinal(int templatePhaseId, int ordinal);

        Task<TemplatePhaseStep> GetTemplatePhaseStep(int id);

        Task<int?> GetStepTypeId(int stepId);

        Task<TemplatePhaseStep> AddTemplatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<TemplatePhaseStep> EditTemplatePhaseStep(TemplatePhaseStep templatePhaseStep);

        Task<int?> DeleteStep(int id);

        Task<int?> DeletePhase(int id);

        Task<int> GetNewPhaseOrdinal(int templateId);

        Task<int> GetNewStepOrdinal(int templatePhaseId);

        Task<PaginationSet<TemplatePhaseStepAttachment>> GetAttachments(int? tpsId, PaginationSet<TemplatePhaseStepAttachment> pager);

        Task<PaginationSet<TemplatePhaseStepDocumentProof>> GetDocumentProof(int? tpsId, PaginationSet<TemplatePhaseStepDocumentProof> pager);

        Task<List<DocumentFileType>> GetDocumentFileTypes();

        Task<TemplatePhaseStepDocumentProof> CreateDocumentProof(TemplatePhaseStepDocumentProof model);

        Task<TemplatePhaseStepDocumentProof> EditDocumentProof(TemplatePhaseStepDocumentProof model);


        Task<int> DeleteDocumentProof(int id);

        Task<TemplatePhaseStepAttachment> CreateAttachment(TemplatePhaseStepAttachment model);

        Task<TemplatePhaseStepAttachment> EditAttachment(TemplatePhaseStepAttachment model);

        Task<int> DeleteAttachment(int id);
    }
}