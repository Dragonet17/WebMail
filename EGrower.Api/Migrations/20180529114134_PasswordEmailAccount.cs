using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class PasswordEmailAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "SendedEmailMessages");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "EmailMessages",
                newName: "DeliveredAt");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "EmailMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "EmailAccounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "EmailAccounts");

            migrationBuilder.RenameColumn(
                name: "DeliveredAt",
                table: "EmailMessages",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "SendedEmailMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
