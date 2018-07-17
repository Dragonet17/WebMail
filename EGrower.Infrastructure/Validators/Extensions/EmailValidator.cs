using FluentValidation;

namespace EGrower.Infrastructure.Validators.SendedEmail.Extensions {
    public class EmailValidator : AbstractValidator<string> {
        public EmailValidator () {
            RuleFor (x => x)
                .NotNull ()
                .NotEmpty ()
                .MinimumLength (5)
                .MaximumLength (150)
                .EmailAddress ();
        }
    }
}