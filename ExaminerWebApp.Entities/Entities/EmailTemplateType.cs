namespace ExaminerWebApp.Entities.Entities
{
    public partial class EmailTemplateType
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; } = new List<EmailTemplate>();
    }
}