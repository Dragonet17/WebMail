using System.Threading.Tasks;
using MimeKit;

namespace EGrower.Infrastructure.Factories.Interfaces {
    public interface IEmailFactory {
        Task SendEmailAsync (MimeMessage mimeMessage);
    }
}