using EGrower.Infrastructure.Commands.SendedEmail;
using EGrower.Infrastructure.Validators.SendedEmail.Extensions;
using FluentValidation;

namespace EGrower.Infrastructure.Validators.SendedEmail {
    public class ReplyToEmailValidator : AbstractValidator<ReplyToEmail> {
        public ReplyToEmailValidator () {

            When (x => x.Attachments != null && x.Attachments.Count > 0, () => {
                RuleFor (reg => reg.Attachments)
                    .SetCollectionValidator (new FormFileValidator ());
            });
            When (x => x.Cc != null && x.Cc.Count > 0, () => {
                RuleFor (reg => reg.Cc)
                    .SetCollectionValidator (new EmailValidator ());
            });
            When (x => x.Bcc != null && x.Bcc.Count > 0, () => {
                RuleFor (reg => reg.Bcc)
                    .SetCollectionValidator (new EmailValidator ());
            });
        }
    }
}