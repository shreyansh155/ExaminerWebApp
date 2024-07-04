using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.ViewModels
{
    public class ApplicationTypeTemplateModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage ="Please enter application template name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Instruction { get; set; }
    }
}