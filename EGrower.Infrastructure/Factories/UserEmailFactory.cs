using System;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Extension.Interfaces;
using EGrower.Infrastructure.Factories.Interfaces;
using MimeKit;

namespace EGrower.Infrastructure.Factories {
    public class UserEmailFactory : IUserEmailFactory {
        private readonly IEmailFactory _emailFactory;
        private readonly IEmailConfiguration _emailConfiguration;
        public UserEmailFactory (IEmailFactory emailFactory, IEmailConfiguration emailConfiguration) {
            _emailFactory = emailFactory;
            _emailConfiguration = emailConfiguration;
        }
        public async Task SendActivationEmailAsync (User user, Guid activationKey) {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (_emailConfiguration.Name, _emailConfiguration.SmtpUsername));
            message.To.Add (new MailboxAddress (user.Name, user.Email));
            message.Subject = "Aktywacja do systemu E-Hodowca";
            message.Body = new TextPart ("html") { Text = $"Oto mail wygenerowany automatycznie, potwierdzający Twoją rejestrację w aplikacji <b>E-Hodowca</b><br/> Kliknij w <a href=\"http://localhost:5000/api/auth/activation/{activationKey}\">link aktywacyjny</a>, dzięki czemu aktywujesz swoje konto w serwisie." };
            await _emailFactory.SendEmailAsync (message);
        }
        public async Task SendRecoveringPasswordEmailAsync (User user, Guid token) {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (_emailConfiguration.Name, _emailConfiguration.SmtpUsername));
            message.To.Add (new MailboxAddress (user.Name.ToString (), user.Email.ToString ()));
            message.Subject = "Przywracanie hasla w systemie E-Hodowca";
            message.Body = new TextPart ("html") { Text = $"Witaj, {user.Name}.Ten mail został wygenerowany automatycznie.</b><br/> Kliknij w <a href=\"http://localhost:5000/api/auth/recoveringPassword/{token}\">link </a>, aby zmienić swoje hasło." };
            await _emailFactory.SendEmailAsync (message);
        }
    }
}