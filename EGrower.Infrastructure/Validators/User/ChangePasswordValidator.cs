using EGrower.Infrastructure.Commands.User;
using FluentValidation;

namespace EGrower.Infrastructure.Validators {
    public class ChangePasswordValidator : AbstractValidator<ChangePassword> {
        public ChangePasswordValidator () {
            RuleFor (reg => reg.OldPassword)
                .NotNull ()
                .Must (u => !u.Contains (" "))
                .WithMessage ("Password should not contain space")
                .Matches (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$")
                .WithMessage ("Passoword must consist of 8-50 characters and at least: one number, one upper case, one lower case  and one unique character such as '!#$%&?' ");
            RuleFor (reg => reg.NewPassword)
                .NotNull ()
                .Must (u => !u.Contains (" "))
                .WithMessage ("Password should not contain space")
                .Matches (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$")
                .WithMessage ("Passoword must consist of 8-50 characters and at least: one number, one upper case, one lower case  and one unique character such as '!#$%&?' ")
                .NotEqual (reg => reg.OldPassword)
                .WithMessage ("New password should not be equal to old password.");
        }

    }
}