using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace EGrower.Infrastructure.Validators.SendedEmail.Extensions {
    public class FormFileValidator : AbstractValidator<IFormFile> {
        public FormFileValidator () {
            RuleFor (reg => reg.Length)
                .LessThan (10240000)
                .WithMessage ("File size should be less than 10MB.");
        }
    }
}