namespace EGrower.Infrastructure.Commands.User {
    public class ChangePassword {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}