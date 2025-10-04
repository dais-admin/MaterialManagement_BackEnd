using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBulkUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApprovalStatus",
                table: "BulkUploadDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActionRequiredByUserEmail",
                table: "BulkUploadDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DesignDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignDocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignDocuments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignDocuments_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesignDocuments_ProjectId",
                table: "DesignDocuments",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignDocuments_WorkPackageId",
                table: "DesignDocuments",
                column: "WorkPackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignDocuments");

            migrationBuilder.DropColumn(
                name: "ActionRequiredByUserEmail",
                table: "BulkUploadDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalStatus",
                table: "BulkUploadDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
