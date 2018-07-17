using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGrower.Core.Domains {
    public abstract class EmailAccountProtocol {
        public int Id { get; private set; }
        public int Port { get; private set; }
        public string Host { get; private set; }
        public string EmailProvider { get; private set; }
        protected EmailAccountProtocol () { }

        public EmailAccountProtocol (int port, string host) {
            Port = port;
            Host = host;
        }

        public EmailAccountProtocol (int port, string host, string emailProvider) {
            Port = port;
            Host = host;
            EmailProvider = emailProvider;
        }
        public void Update (int port, string host, string emailProvider) {
            Port = port;
            Host = host;
            EmailProvider = emailProvider;
        }
    }
}