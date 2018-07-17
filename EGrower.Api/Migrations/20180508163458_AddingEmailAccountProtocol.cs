using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class AddingEmailAccountProtocol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_Settings_SettingsId",
                table: "EmailAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settings",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Activated",
                table: "EmailAccounts");

            migrationBuilder.RenameTable(
                name: "Settings",
                newName: "EmailAccountProtocols");

            migrationBuilder.RenameColumn(
                name: "SettingsId",
                table: "EmailAccounts",
                newName: "SmtpId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailAccounts_SettingsId",
                table: "EmailAccounts",
                newName: "IX_EmailAccounts_SmtpId");

            migrationBuilder.AddColumn<int>(
                name: "ImapId",
                table: "EmailAccounts",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailAccountProtocols",
                table: "EmailAccountProtocols",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAccounts_ImapId",
                table: "EmailAccounts",
                column: "ImapId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_EmailAccountProtocols_ImapId",
                table: "EmailAccounts",
                column: "ImapId",
                principalTable: "EmailAccountProtocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_EmailAccountProtocols_SmtpId",
                table: "EmailAccounts",
                column: "SmtpId",
                principalTable: "EmailAccountProtocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_EmailAccountProtocols_ImapId",
                table: "EmailAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_EmailAccountProtocols_SmtpId",
                table: "EmailAccounts");

            migrationBuilder.DropIndex(
                name: "IX_EmailAccounts_ImapId",
                table: "EmailAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailAccountProtocols",
                table: "EmailAccountProtocols");

            migrationBuilder.DropColumn(
                name: "ImapId",
                table: "EmailAccounts");

            migrationBuilder.RenameTable(
                name: "EmailAccountProtocols",
                newName: "Settings");

            migrationBuilder.RenameColumn(
                name: "SmtpId",
                table: "EmailAccounts",
                newName: "SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailAccounts_SmtpId",
                table: "EmailAccounts",
                newName: "IX_EmailAccounts_SettingsId");

            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "EmailAccounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settings",
                table: "Settings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_Settings_SettingsId",
                table: "EmailAccounts",
                column: "SettingsId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
