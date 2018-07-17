﻿// <auto-generated />
using EGrower.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace EGrower.Api.Migrations
{
    [DbContext(typeof(EGrowerContext))]
    [Migration("20180508095538_DataBaseWithRelations")]
    partial class DataBaseWithRelations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EGrower.Core.Domains.Atachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<byte[]>("Data");

                    b.Property<int>("EmailMessageId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("EmailMessageId");

                    b.ToTable("Atachments");
                });

            modelBuilder.Entity("EGrower.Core.Domains.EmailAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Activated");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<int?>("SettingsId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.HasIndex("UserId");

                    b.ToTable("EmailAccounts");
                });

            modelBuilder.Entity("EGrower.Core.Domains.EmailMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("EmailAccountsId");

                    b.Property<string>("From");

                    b.Property<string>("Subject");

                    b.Property<string>("TextHTMLBody");

                    b.Property<string>("To");

                    b.HasKey("Id");

                    b.HasIndex("EmailAccountsId");

                    b.ToTable("EmailMessages");
                });

            modelBuilder.Entity("EGrower.Core.Domains.SendedAtachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<byte[]>("Data");

                    b.Property<string>("Name");

                    b.Property<int>("SendedEmailMessageId");

                    b.HasKey("Id");

                    b.HasIndex("SendedEmailMessageId");

                    b.ToTable("SendedAtachments");
                });

            modelBuilder.Entity("EGrower.Core.Domains.SendedEmailMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddedAt");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("From");

                    b.Property<string>("Subject");

                    b.Property<string>("TextHTMLBody");

                    b.Property<string>("To");

                    b.HasKey("Id");

                    b.ToTable("SendedEmailMessages");
                });

            modelBuilder.Entity("EGrower.Core.Domains.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("EmailProvider");

                    b.Property<string>("Host");

                    b.Property<int>("Port");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Settings");
                });

            modelBuilder.Entity("EGrower.Core.Domains.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Activated");

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Email");

                    b.Property<DateTime>("LastActivity");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Role");

                    b.Property<string>("Surname");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EGrower.Core.Domains.UserActivation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActivatedAt");

                    b.Property<Guid>("ActivationKey");

                    b.Property<bool>("Inactive");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UsersActivations");
                });

            modelBuilder.Entity("EGrower.Core.Domains.IMap", b =>
                {
                    b.HasBaseType("EGrower.Core.Domains.Settings");


                    b.ToTable("IMap");

                    b.HasDiscriminator().HasValue("IMap");
                });

            modelBuilder.Entity("EGrower.Core.Domains.Smtp", b =>
                {
                    b.HasBaseType("EGrower.Core.Domains.Settings");


                    b.ToTable("Smtp");

                    b.HasDiscriminator().HasValue("Smtp");
                });

            modelBuilder.Entity("EGrower.Core.Domains.Atachment", b =>
                {
                    b.HasOne("EGrower.Core.Domains.EmailMessage", "EmailMessage")
                        .WithMany("Atachments")
                        .HasForeignKey("EmailMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EGrower.Core.Domains.EmailAccount", b =>
                {
                    b.HasOne("EGrower.Core.Domains.Settings", "Settings")
                        .WithMany("EmailAccounts")
                        .HasForeignKey("SettingsId");

                    b.HasOne("EGrower.Core.Domains.User", "User")
                        .WithMany("EmailAccounts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EGrower.Core.Domains.EmailMessage", b =>
                {
                    b.HasOne("EGrower.Core.Domains.EmailAccount", "EmailAccounts")
                        .WithMany("EmailMessages")
                        .HasForeignKey("EmailAccountsId");
                });

            modelBuilder.Entity("EGrower.Core.Domains.SendedAtachment", b =>
                {
                    b.HasOne("EGrower.Core.Domains.SendedEmailMessage", "SendedEmailMessage")
                        .WithMany("SendedAtachments")
                        .HasForeignKey("SendedEmailMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EGrower.Core.Domains.UserActivation", b =>
                {
                    b.HasOne("EGrower.Core.Domains.User", "User")
                        .WithOne("UserActivation")
                        .HasForeignKey("EGrower.Core.Domains.UserActivation", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}