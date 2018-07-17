using System;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Factories.Interfaces {
    public interface IUserEmailFactory {
        Task SendActivationEmailAsync (User user, Guid activationKey);
        Task SendRecoveringPasswordEmailAsync (User user, Guid token);
    }
}