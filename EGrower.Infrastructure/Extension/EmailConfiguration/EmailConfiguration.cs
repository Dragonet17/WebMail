using EGrower.Infrastructure.Extension.Interfaces;

namespace EGrower.Infrastructure.Extension.EmailConfiguration {
    public class EmailConfiguration:IEmailConfiguration {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string Name { get; set; }
    }
}