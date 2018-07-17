using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;
using EGrower.Infrastructure.Repositories.Interfaces;

namespace EGrower.Infrastructure.Extension.ExtensionMethods {
    public static class RepositoryExtensions {
        public static async Task<bool> UserExists (this IUserRepository repository, string email) {
            var user = await repository.GetByEmailAsync (email);
            if (user == null)
                return false;
            return true;
        }
        public static async Task<EmailMessage> GetCheckSeenOrFailAsync (this IEmailMessageRepository repository, int userId, int emailId) {
            var emailMessages = await repository.GetAllForUserAsync (userId);
            if (emailMessages == null || emailMessages.Count () == 0)
                throw new System.Exception ("This user does not have email messages with this id.");
            var emailMessage = emailMessages.SingleOrDefault (a => a.Id == emailId);
            emailMessage.MarkAsRead ();
            await repository.UpdateAsync(emailMessage);
            return emailMessage;
        }
    }
}