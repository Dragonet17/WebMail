using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class OnDeleteCascading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_Users_UserId",
                table: "EmailAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_Users_UserId",
                table: "EmailAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAccounts_Users_UserId",
                table: "EmailAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAccounts_Users_UserId",
                table: "EmailAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
