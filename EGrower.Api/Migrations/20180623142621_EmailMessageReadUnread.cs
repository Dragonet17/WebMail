using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class EmailMessageReadUnread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastSeen",
                table: "EmailMessages",
                newName: "LastActivity");

            migrationBuilder.RenameColumn(
                name: "IsSeen",
                table: "EmailMessages",
                newName: "IsRead");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastActivity",
                table: "EmailMessages",
                newName: "LastSeen");

            migrationBuilder.RenameColumn(
                name: "IsRead",
                table: "EmailMessages",
                newName: "IsSeen");
        }
    }
}
