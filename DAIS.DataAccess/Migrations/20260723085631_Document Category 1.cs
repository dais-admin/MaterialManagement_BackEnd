using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DocumentCategory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentCategory",
                table: "DesignDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentCategoryId",
                table: "DesignDocuments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignDocuments_DocumentCategoryId",
                table: "DesignDocuments",
                column: "DocumentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DesignDocuments_DocumentCategories_DocumentCategoryId",
                table: "DesignDocuments",
                column: "DocumentCategoryId",
                principalTable: "DocumentCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignDocuments_DocumentCategories_DocumentCategoryId",
                table: "DesignDocuments");

            migrationBuilder.DropIndex(
                name: "IX_DesignDocuments_DocumentCategoryId",
                table: "DesignDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentCategoryId",
                table: "DesignDocuments");

            migrationBuilder.AddColumn<string>(
                name: "DocumentCategory",
                table: "DesignDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
