using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class materials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMaterial",
                table: "Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMaterial",
                table: "Materials");
        }
    }
}
