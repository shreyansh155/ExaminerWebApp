using System;
using System.Collections.Generic;

namespace ExaminerWebApp.Repository.DataModels;

public partial class EmailTemplateType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<EmailTemplate> EmailTemplates { get; set; } = new List<EmailTemplate>();
}
