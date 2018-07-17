using System;

namespace EGrower.Core.Domains {
    public class UserActivation {
        public int Id { get; private set; }
        public Guid ActivationKey { get; private set; }
        public bool Inactive { get; private set; }
        public DateTime ActivatedAt { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }

        protected UserActivation () {
            ActivationKey = Guid.NewGuid ();
            Inactive = false;
        }
        public UserActivation (Guid activationKey) {
            ActivationKey = activationKey;
            Inactive = false;
        }
        public void Activate () {
            ActivatedAt = DateTime.UtcNow;
            Inactive = true;
        }
    }
}