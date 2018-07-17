using EGrower.Infrastructure.Commands.User;
using FluentValidation;

namespace EGrower.Infrastructure.Validators {
    public class SignInUserValidator : AbstractValidator<SignInUser> {
        public SignInUserValidator () {
            RuleFor (reg => reg.Email)
                .NotNull ()
                .EmailAddress ()
                .MinimumLength (5);
            RuleFor (reg => reg.Password)
                .NotNull ()
                .Must (u => !u.Contains (" "))
                .WithMessage ("Password should not contain space")
                .Matches (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$")
                .WithMessage ("Passoword must consist of 8-50 characters and at least: one number, one upper case, one lower case  and one unique character such as '!#$%&?' ");
        }
    }
}