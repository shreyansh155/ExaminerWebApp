﻿using System.ComponentModel.DataAnnotations;

namespace ExaminerWebApp.ViewHelpers
{
    public class DateNotInFuture : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? date = (DateTime?)value;
            if (date > DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}