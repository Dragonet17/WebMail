using System;

namespace EGrower.Core.Domains {
    public class UserRestoringPassword {
        public int Id { get; set; }
        public DateTime RestoredAt { get; private set; }
        public Guid Token { get; private set; }
        public bool Restored { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        protected UserRestoringPassword () { }
        public UserRestoringPassword (Guid token) {
            RestoredAt = default (DateTime);
            Token = token;
            Restored = false;
        }
        public void PasswordRestoring () {
            Restored = true;
            RestoredAt = DateTime.UtcNow;
            Token = Guid.NewGuid ();
        }
        public void ResetState (Guid token) {
            RestoredAt = default (DateTime);
            Token = token;
            Restored = false;
        }
    }
}