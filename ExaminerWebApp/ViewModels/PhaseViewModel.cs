
using ExaminerWebApp.Entities.Entities;

namespace ExaminerWebApp.ViewModels
{
    public class PhaseViewModel
    {
        public ApplicationTypeTemplatePhase? TemplatePhase { get; set; }
        public int? PhaseId { get; set; }
      
        public int Ordinal { get; set; }
        
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public int TemplateId { get; set; }
        
        public List<GridDataItem>? GridData { get; set; }
    }

    public class GridDataItem
    {
        public int Id { get; set; }
        
        public int? TemplatePhaseId { get; set; }
        
        public int? TemplatePhaseStepId { get; set; }
        
        public string Name { get; set; }
        
        public int? Ordinal { get; set; }
        
        public bool IsInTemplatePhaseSteps { get; set; }
    }
}