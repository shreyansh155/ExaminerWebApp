﻿using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class ApplicationTypeTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Instruction { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ApplicationTypeTemplatePhase> ApplicationTypeTemplatePhases { get; set; } = new List<ApplicationTypeTemplatePhase>();
}
