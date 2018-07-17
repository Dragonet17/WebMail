using System;
using System.Collections.Generic;
using System.Text;
using EGrower.Infrastructure.Commands.EmailAccount;
using FluentValidation;

namespace EGrower.Infrastructure.Validators.EmailAccount {
    public class CreateEmailAccountValidator : AbstractValidator<CreateEmailAccount> {
        public CreateEmailAccountValidator () {
            RuleFor (reg => reg.Email)
                .NotNull ()
                .EmailAddress ()
                .MinimumLength (5);
            RuleFor (reg => reg.Password)
                .NotNull ()
                .NotEmpty ()
                .MinimumLength (5);
            RuleFor (eg => eg.ImapHost)
                .NotEmpty ()
                .WithMessage ("Host cannot be empty.")
                .NotNull ()
                .WithMessage ("Host cannot be null.");
            RuleFor (eg => eg.ImapPort)
                .NotEmpty ()
                .WithMessage ("Port cannot be empty.")
                .NotNull ()
                .WithMessage ("Port cannot be null.");
            RuleFor (eg => eg.SmtpHost)
                .NotEmpty ()
                .WithMessage ("Host cannot be empty.")
                .NotNull ()
                .WithMessage ("Host cannot be null.");
            RuleFor (eg => eg.SmtpPort)
                .NotEmpty ()
                .WithMessage ("Port cannot be empty.")
                .NotNull ()
                .WithMessage ("Port cannot be null.");
        }
    }
}