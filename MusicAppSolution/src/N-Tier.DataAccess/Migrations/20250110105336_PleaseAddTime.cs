using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_Tier.DataAccess.Migrations
{
    public partial class PleaseAddTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Avval eski ustunni o'chiramiz
            migrationBuilder.DropColumn(
                name: "Expire_Date",
                table: "Cards");

            // Keyin yangi DateTime ustunni qo'shamiz
            migrationBuilder.AddColumn<DateTime>(
                name: "Expire_Date",
                table: "Cards",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: DateTime.UtcNow);  // default qiymat beramiz
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expire_Date",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "Expire_Date",
                table: "Cards",
                type: "text",
                nullable: false);
        }
    }
}