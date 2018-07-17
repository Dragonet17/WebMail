using EGrower.Infrastructure.Commands.User;
using FluentValidation;

namespace EGrower.Infrastructure.Validators.User {
    public class RestorePasswordValidator : AbstractValidator<RestorePassword> {
        public RestorePasswordValidator () {
            RuleFor (reg => reg.Email)
                .EmailAddress ()
                .MinimumLength (5)
                .MaximumLength (150)
                .NotNull ();
        }
    }
}