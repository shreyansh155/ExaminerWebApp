using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.ViewModels
{
    public class PhaseViewModel
    {
        public int? PhaseId { get; set; }
        public int Ordinal { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
