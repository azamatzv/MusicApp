using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace N_Tier.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangingTariffId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarifId",
                table: "PaymentHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TarifId",
                table: "PaymentHistories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
