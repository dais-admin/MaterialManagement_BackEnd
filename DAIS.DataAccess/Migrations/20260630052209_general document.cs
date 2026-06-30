using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class generaldocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentCategory",
                table: "DesignDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentCategory",
                table: "DesignDocuments");
        }
    }
}
