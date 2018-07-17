using EGrower.Core.Domains;
using Microsoft.EntityFrameworkCore;

namespace EGrower.Infrastructure.Data {
    public class EGrowerContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivation> UsersActivations { get; set; }
        public DbSet<EmailAccountProtocol> EmailAccountProtocols { get; set; }
        public DbSet<Imap> Imaps { get; set; }
        public DbSet<Smtp> Smtp { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<Atachment> Atachments { get; set; }
        public DbSet<SendedEmailMessage> SendedEmailMessages { get; set; }
        public DbSet<SendedAtachment> SendedAtachments { get; set; }

        public EGrowerContext (DbContextOptions<EGrowerContext> options) : base (options) { }
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<User> ()
                .HasOne (a => a.UserActivation)
                .WithOne (b => b.User)
                .HasForeignKey<UserActivation> (b => b.UserId)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<User> ()
                .HasOne (a => a.UserRestoringPassword)
                .WithOne (b => b.User)
                .HasForeignKey<UserRestoringPassword> (b => b.UserId)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<User> ()
                .HasMany (u => u.EmailAccounts)
                .WithOne (e => e.User)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<Imap> ()
                .HasMany (s => s.EmailAccounts)
                .WithOne (em => em.Imap);
            modelBuilder.Entity<Smtp> ()
                .HasMany (s => s.EmailAccounts)
                .WithOne (em => em.Smtp);
            modelBuilder.Entity<EmailAccount> ()
                .HasMany (s => s.EmailMessages)
                .WithOne (em => em.EmailAccount)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<EmailAccount> ()
                .HasMany (s => s.SendedEmailMessages)
                .WithOne (em => em.EmailAccount)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<EmailMessage> ()
                .HasMany (em => em.Atachments)
                .WithOne (a => a.EmailMessage)
                .HasForeignKey (a => a.EmailMessageId)
                .OnDelete (DeleteBehavior.Cascade);
            modelBuilder.Entity<SendedEmailMessage> ()
                .HasMany (em => em.SendedAtachments)
                .WithOne (a => a.SendedEmailMessage)
                .HasForeignKey (a => a.SendedEmailMessageId)
                .OnDelete (DeleteBehavior.Cascade);
        }
    }
}