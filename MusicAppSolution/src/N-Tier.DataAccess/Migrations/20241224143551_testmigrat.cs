﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_Tier.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class testmigrat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Userss",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Userss");
        }
    }
}
