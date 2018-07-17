using System;

namespace EGrower.Infrastructure.Commands.User {
    public class ChangePasswordByRestoringPassword {
        public Guid Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}