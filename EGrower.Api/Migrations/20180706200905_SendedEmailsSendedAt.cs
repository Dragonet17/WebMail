using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EGrower.Api.Migrations
{
    public partial class SendedEmailsSendedAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SendedEmailMessages",
                newName: "SendedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SendedAt",
                table: "SendedEmailMessages",
                newName: "CreatedAt");
        }
    }
}
