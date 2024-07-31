using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class TemplatePhaseStepAttachment
{
    public int Id { get; set; }

    public int TemplatePhaseStepId { get; set; }

    public string Title { get; set; } = null!;

    public int AttachmentTypeId { get; set; }

    public int Ordinal { get; set; }

    public string FilePath { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual DocumentFileType AttachmentType { get; set; } = null!;
}
