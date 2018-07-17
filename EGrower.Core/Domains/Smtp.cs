using System;
using System.Collections.Generic;
using System.Text;

namespace EGrower.Core.Domains {
    public class Smtp : EmailAccountProtocol {
        public ICollection<EmailAccount> EmailAccounts { get; private set; }
        public Smtp () { }
        public Smtp (int port, string host) : base (port, host) { }
        public Smtp (int port, string host, string emailProvider) : base (port, host, emailProvider) { }
    }
}