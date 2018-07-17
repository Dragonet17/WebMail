using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class ContentTypeInsideAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountsId",
                table: "EmailMessages");

            migrationBuilder.RenameColumn(
                name: "EmailAccountsId",
                table: "EmailMessages",
                newName: "EmailAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailMessages_EmailAccountsId",
                table: "EmailMessages",
                newName: "IX_EmailMessages_EmailAccountId");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "SendedAtachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Atachments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages",
                column: "EmailAccountId",
                principalTable: "EmailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "SendedAtachments");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Atachments");

            migrationBuilder.RenameColumn(
                name: "EmailAccountId",
                table: "EmailMessages",
                newName: "EmailAccountsId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailMessages_EmailAccountId",
                table: "EmailMessages",
                newName: "IX_EmailMessages_EmailAccountsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_EmailAccounts_EmailAccountsId",
                table: "EmailMessages",
                column: "EmailAccountsId",
                principalTable: "EmailAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
