using System;
using System.Collections.Generic;

namespace EGrower.Core.Domains {
    public class User {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string Country { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime LastActivity { get; private set; }
        public bool Deleted { get; private set; }
        public bool Activated { get; private set; }
        public UserActivation UserActivation { get; private set; }
        public ICollection<EmailAccount> EmailAccounts { get; private set; }
        public UserRestoringPassword UserRestoringPassword { get; private set; }
        protected User () { }
        public User (string email, string password, string name, string surname, string country) {
            Email = email;
            Name = name;
            Surname = surname;
            Country = country;
            Role = "user";
            CreatePasswordHash (password);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            LastActivity = DateTime.UtcNow;
            Deleted = false;
            Activated = false;
        }
        public void Update (string name, string surname, string country) {
            Name = name;
            Surname = surname;
            Country = country;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePassword (string newPassword) {
            CreatePasswordHash (newPassword);
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate (UserActivation activationUser) {
            if (!activationUser.Inactive) {
                Activated = true;
                UpdatedAt = DateTime.UtcNow;
                activationUser.Activate ();
            }
        }
        public void ChangeUserRestoringPassword (Guid token) {
            if (UserRestoringPassword != null)
                UserRestoringPassword.ResetState (token);
        }
        public void AddUserRestoringPassword (UserRestoringPassword userRestoringPassword) {
            if (userRestoringPassword != null)
                UserRestoringPassword = userRestoringPassword;
        }
        public void Delete () {
            if (Deleted == false) {
                Deleted = true;
                UpdatedAt = DateTime.UtcNow;
            }
        }
        public void Restore () {
            if (Deleted == true) {
                Deleted = false;
                UpdatedAt = DateTime.UtcNow;
            }
        }
        private void CreatePasswordHash (string password) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }
        public void AddUserActivation (UserActivation userActivation) {
            UserActivation = userActivation;
        }
        public void AddEmailAccount (EmailAccount emailAccount) {
            EmailAccounts.Add (emailAccount);
        }
    }
}