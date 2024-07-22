namespace ExaminerWebApp.Entities.Entities
{
    public partial class Phase
    {
        public int? Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Ordinal { get; set; }

        public string? CreatedBy { get; set; } 

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}