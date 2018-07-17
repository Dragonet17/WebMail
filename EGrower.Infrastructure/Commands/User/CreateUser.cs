namespace EGrower.Infrastructure.Commands.User {
    public class CreateUser {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
    }
}