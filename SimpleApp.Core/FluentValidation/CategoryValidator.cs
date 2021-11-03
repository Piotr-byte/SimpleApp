﻿using FluentValidation;
using SimpleApp.Core.Models;

namespace SimpleApp.Core.FluentValidation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("This field cannot be empty")
                .MinimumLength(3).WithMessage("Minimum length of {MinLength} char allowed")
                .MaximumLength(20).WithMessage("Maximum legth of {MaxLength} char is allowed");
            
        }
   
    }
}