using System;
using System.Collections.Generic;
using EGrower.Infrastructure.DTO.EmailAccount;

namespace EGrower.Infrastructure.DTO {
    public class UserDTO {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string Country { get; private set; }
        public DateTime LastActivity { get; private set; }
        public ICollection<EmailAccountDTO> EmailAccounts { get; set; }
    }
}