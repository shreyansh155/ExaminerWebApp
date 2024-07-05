using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.ViewModels
{
    public class CreatePhaseModel
    {
        [Required(ErrorMessage = "Phase name is required")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
