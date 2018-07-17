using System;
using System.Collections.Generic;
using System.Text;

namespace EGrower.Core.Domains {
    public class Imap : EmailAccountProtocol {
        public ICollection<EmailAccount> EmailAccounts { get; private set; }
        public Imap () { }
        public Imap (int port, string host) : base (port, host) { }
        public Imap (int port, string host, string emailProvider) : base (port, host, emailProvider) { }
    }
}