using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class ApplicationTypeTemplatePhase
{
    public int Id { get; set; }

    public int TemplateId { get; set; }

    public int PhaseId { get; set; }

    public int Ordinal { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Phase Phase { get; set; } = null!;

    public virtual ApplicationTypeTemplate Template { get; set; } = null!;

    public virtual ICollection<TemplatePhaseStep> TemplatePhaseSteps { get; set; } = new List<TemplatePhaseStep>();
}
