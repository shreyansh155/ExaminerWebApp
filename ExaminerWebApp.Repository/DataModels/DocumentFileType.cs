using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class DocumentFileType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TemplatePhaseStepAttachment> TemplatePhaseStepAttachments { get; set; } = new List<TemplatePhaseStepAttachment>();

    public virtual ICollection<TemplatePhaseStepDocumentProof> TemplatePhaseStepDocumentProofs { get; set; } = new List<TemplatePhaseStepDocumentProof>();
}
