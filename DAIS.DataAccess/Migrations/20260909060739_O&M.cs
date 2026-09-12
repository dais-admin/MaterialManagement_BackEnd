using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlertPostponedDays",
                table: "MaterialMaintenances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlertStatus",
                table: "MaterialMaintenances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AlertUpdatedDate",
                table: "MaterialMaintenances",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertPostponedDays",
                table: "MaterialMaintenances");

            migrationBuilder.DropColumn(
                name: "AlertStatus",
                table: "MaterialMaintenances");

            migrationBuilder.DropColumn(
                name: "AlertUpdatedDate",
                table: "MaterialMaintenances");
        }
    }
}
