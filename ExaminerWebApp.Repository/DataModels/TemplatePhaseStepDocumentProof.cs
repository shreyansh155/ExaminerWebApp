using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class TemplatePhaseStepDocumentProof
{
    public int Id { get; set; }

    public int TemplatePhaseStepId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int DocumentFileType { get; set; }

    public bool IsRequired { get; set; }

    public int Ordinal { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual DocumentFileType DocumentFileTypeNavigation { get; set; } = null!;
}
