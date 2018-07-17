using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class SendedMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "SendedEmailMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EmailAccountId",
                table: "SendedEmailMessages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAttachment",
                table: "SendedEmailMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "SendedEmailMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SendedEmailMessages_EmailAccountId",
                table: "SendedEmailMessages",
                column: "EmailAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages",
                column: "EmailAccountId",
                principalTable: "EmailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SendedEmailMessages_EmailAccounts_EmailAccountId",
                table: "SendedEmailMessages",
                column: "EmailAccountId",
                principalTable: "EmailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SendedEmailMessages_EmailAccounts_EmailAccountId",
                table: "SendedEmailMessages");

            migrationBuilder.DropIndex(
                name: "IX_SendedEmailMessages_EmailAccountId",
                table: "SendedEmailMessages");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "SendedEmailMessages");

            migrationBuilder.DropColumn(
                name: "EmailAccountId",
                table: "SendedEmailMessages");

            migrationBuilder.DropColumn(
                name: "HasAttachment",
                table: "SendedEmailMessages");

            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "SendedEmailMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages",
                column: "EmailAccountId",
                principalTable: "EmailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
