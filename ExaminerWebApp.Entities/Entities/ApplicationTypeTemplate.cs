﻿using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.Entities.Entities
{
    public partial class ApplicationTypeTemplate
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Please enter application template name")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Instruction { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}