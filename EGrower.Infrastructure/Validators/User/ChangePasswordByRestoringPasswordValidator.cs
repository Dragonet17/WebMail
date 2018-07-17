using EGrower.Infrastructure.Commands.User;
using FluentValidation;

namespace EGrower.Infrastructure.Validators.User {
    public class ChangePasswordByRestoringPasswordValidator : AbstractValidator<ChangePasswordByRestoringPassword> {
        public ChangePasswordByRestoringPasswordValidator () {

            RuleFor (reg => reg.Email)
                .EmailAddress ()
                .MinimumLength (5)
                .MaximumLength (150)
                .NotNull ();
            RuleFor (reg => reg.NewPassword)
                .NotNull ()
                .NotEmpty ()
                .MaximumLength (50)
                .Must (u => !string.IsNullOrWhiteSpace (u) && !u.Contains (" "))
                .WithMessage ("Password should not contain space")
                .Matches (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$")
                .WithMessage ("Passoword must consist of 8-50 characters and at least: one number, one upper case, one lower case  and one unique character such as '!#$%&?' ");
        }
    }
}