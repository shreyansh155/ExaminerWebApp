using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.ViewModels
{
    public class AddEditStepModel
    {
        public int? Id { get; set; }
      
        public int? StepId { get; set; }
      
        public int? TemplatePhaseId { get; set; }
     
        public int? StepTypeId { get; set; }

        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "Please enter a valid ordinal number")]
        public int? Ordinal { get; set; }
    
        public string? Instruction { get; set; }
    }
}
