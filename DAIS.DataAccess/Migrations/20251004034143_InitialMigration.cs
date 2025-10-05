using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAIS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AgencyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppBackupDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackupType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BackupStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackupSize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBackupDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BulkUploadDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfRecords = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionRequiredByUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkUploadDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DivisionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExcelReaderMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    FileException = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelReaderMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MeasurementCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubDivisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubDivisionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisions_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleFeatures_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleFeatures_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInitialLogin = table.Column<bool>(type: "bit", nullable: false),
                    ReportingTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TypeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkPackageName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkPackageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    System = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LetterOfAcceptanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommencementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractPackageDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPackages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CategoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_DesignDocuments_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationOperationName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LocationOperationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    System = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationOperations_SubDivisions_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationOperations_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractorName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContractorAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractors_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contractors_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufacturerName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ManufacturerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportantDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufacturers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Manufacturers_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SupplierAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductsDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceProviderDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContractorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceProviders_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceProviders_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    System = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialQty = table.Column<int>(type: "int", nullable: false),
                    LocationRFId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfSupply = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommissioningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearOfInstallation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DesignLifeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndPeriodLifeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialStatus = table.Column<int>(type: "int", nullable: false),
                    CurrentApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManufacturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContractorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuilkUploadDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BulkUploadDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRehabilitation = table.Column<bool>(type: "bit", nullable: true),
                    RehabilitationMaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_BulkUploadDetails_BulkUploadDetailId",
                        column: x => x.BulkUploadDetailId,
                        principalTable: "BulkUploadDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_LocationOperations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_MaterialMeasurements_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "MaterialMeasurements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_MaterialTypes_MaterialTypeId",
                        column: x => x.MaterialTypeId,
                        principalTable: "MaterialTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Materials_WorkPackages_WorkPackageId",
                        column: x => x.WorkPackageId,
                        principalTable: "WorkPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalStatusHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusChangeBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionRequiredByUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalStatusHistory_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DivisionLocationMaterialTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieveLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionLocationMaterialTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransfers_Divisions_IssueDivisionId",
                        column: x => x.IssueDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransfers_Divisions_OnBoardedDivisionId",
                        column: x => x.OnBoardedDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransfers_LocationOperations_RecieveLocationId",
                        column: x => x.RecieveLocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransfers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionMaterialTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieveDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionMaterialTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransfers_Divisions_IssueDivisionId",
                        column: x => x.IssueDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransfers_Divisions_OnBoardedDivisionId",
                        column: x => x.OnBoardedDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransfers_Divisions_RecieveDivisionId",
                        column: x => x.RecieveDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransfers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DivisionToSubDivisionMaterialTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionToSubDivisionMaterialTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransfers_Divisions_IssueDivisionId",
                        column: x => x.IssueDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransfers_Divisions_OnBoardedDivisionId",
                        column: x => x.OnBoardedDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransfers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransfers_SubDivisions_TargetSubDivisionId",
                        column: x => x.TargetSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    ReviewedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmitterComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerPreviousComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproverComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproverPreviousComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SubmitterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReveiwerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialApprovals_AspNetUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialApprovals_AspNetUsers_ReveiwerId",
                        column: x => x.ReveiwerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialApprovals_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialApprovals_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDocuments_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialDocuments_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialHardwares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HarwareName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    Chipset = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfManufacturer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetworkDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiskDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BiosDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    HardwareDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManufacturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialHardwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialHardwares_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialHardwares_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialHardwares_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialIssueRecieveVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieveLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialIssueRecieveVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialIssueRecieveVouchers_LocationOperations_IssueLocationId",
                        column: x => x.IssueLocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueRecieveVouchers_LocationOperations_OnBoardedLocationId",
                        column: x => x.OnBoardedLocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueRecieveVouchers_LocationOperations_RecieveLocationId",
                        column: x => x.RecieveLocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialIssueRecieveVouchers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaintenanceStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaintenanceEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgencyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialServiceProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialMaintenances_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialMaintenances_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialMaintenances_ServiceProviders_MaterialServiceProviderId",
                        column: x => x.MaterialServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialSoftwares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoftwareName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoftwareDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoftwareDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialSoftwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialSoftwares_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialSoftwares_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialWarranties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarrantyStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarrantyEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DLPStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DLPEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastRenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExtended = table.Column<bool>(type: "bit", nullable: false),
                    NoOfMonths = table.Column<int>(type: "int", nullable: false),
                    WarrantyDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialWarranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialWarranties_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialWarranties_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionMaterialTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieveSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionMaterialTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransfers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransfers_SubDivisions_IssueSubDivisionId",
                        column: x => x.IssueSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransfers_SubDivisions_OnBoardedSubDivisionId",
                        column: x => x.OnBoardedSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransfers_SubDivisions_RecieveSubDivisionId",
                        column: x => x.RecieveSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionToDivisionMaterialTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    OnBoardedQuantity = table.Column<int>(type: "int", nullable: false),
                    IssueSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieveDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnBoardedSubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionToDivisionMaterialTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransfers_Divisions_RecieveDivisionId",
                        column: x => x.RecieveDivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransfers_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransfers_SubDivisions_IssueSubDivisionId",
                        column: x => x.IssueSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransfers_SubDivisions_OnBoardedSubDivisionId",
                        column: x => x.OnBoardedSubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionLocationMaterialTransferApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionLocationMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionLocationMaterialTransferApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferApprovals_DivisionLocationMaterialTransfers_DivisionLocationMaterialTransferId",
                        column: x => x.DivisionLocationMaterialTransferId,
                        principalTable: "DivisionLocationMaterialTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionLocationMaterialTransferTrancations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionLocationMaterialTransferTrancations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferTrancations_DivisionLocationMaterialTransfers_DivisionMaterialTransferId",
                        column: x => x.DivisionMaterialTransferId,
                        principalTable: "DivisionLocationMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferTrancations_LocationOperations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionLocationMaterialTransferTrancations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionMaterialTransferApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionMaterialTransferApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferApprovals_DivisionMaterialTransfers_DivisionMaterialTransferId",
                        column: x => x.DivisionMaterialTransferId,
                        principalTable: "DivisionMaterialTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionMaterialTransferTrancations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionMaterialTransferTrancations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferTrancations_DivisionMaterialTransfers_DivisionMaterialTransferId",
                        column: x => x.DivisionMaterialTransferId,
                        principalTable: "DivisionMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferTrancations_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionMaterialTransferTrancations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DivisionToSubDivisionMaterialTransferApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionToSubDivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionToSubDivisionMaterialTransferApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferApprovals_DivisionToSubDivisionMaterialTransfers_DivisionToSubDivisionMaterialTransferId",
                        column: x => x.DivisionToSubDivisionMaterialTransferId,
                        principalTable: "DivisionToSubDivisionMaterialTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DivisionToSubDivisionMaterialTransferTrancations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DivisionToSubDivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionToSubDivisionMaterialTransferTrancations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferTrancations_DivisionToSubDivisionMaterialTransfers_DivisionToSubDivisionMaterialTransfe~",
                        column: x => x.DivisionToSubDivisionMaterialTransferId,
                        principalTable: "DivisionToSubDivisionMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferTrancations_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferTrancations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DivisionToSubDivisionMaterialTransferTrancations_SubDivisions_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialVoucherTrancationApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialIssueRecieveVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialVoucherTrancationApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancationApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancationApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancationApprovals_MaterialIssueRecieveVouchers_MaterialIssueRecieveVoucherId",
                        column: x => x.MaterialIssueRecieveVoucherId,
                        principalTable: "MaterialIssueRecieveVouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialVoucherTrancations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MaterialIssueRecieveVoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialVoucherTrancations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancations_LocationOperations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancations_MaterialIssueRecieveVouchers_MaterialIssueRecieveVoucherId",
                        column: x => x.MaterialIssueRecieveVoucherId,
                        principalTable: "MaterialIssueRecieveVouchers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialVoucherTrancations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionMaterialTransferApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionMaterialTransferApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferApprovals_SubDivisionMaterialTransfers_SubDivisionMaterialTransferId",
                        column: x => x.SubDivisionMaterialTransferId,
                        principalTable: "SubDivisionMaterialTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionMaterialTransferTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubDivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionMaterialTransferTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferTransactions_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferTransactions_SubDivisionMaterialTransfers_SubDivisionMaterialTransferId",
                        column: x => x.SubDivisionMaterialTransferId,
                        principalTable: "SubDivisionMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionMaterialTransferTransactions_SubDivisions_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionToDivisionMaterialTransferApprovals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivToDivMaterialTransferApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecieverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionToDivisionMaterialTransferApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferApprovals_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferApprovals_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferApprovals_SubDivisionToDivisionMaterialTransfers_SubDivToDivMaterialTransferApprovalId",
                        column: x => x.SubDivToDivMaterialTransferApprovalId,
                        principalTable: "SubDivisionToDivisionMaterialTransfers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubDivisionToDivisionMaterialTransferTrancations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubDivToDivMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IssuedQuantity = table.Column<int>(type: "int", nullable: false),
                    RecievedQuantity = table.Column<int>(type: "int", nullable: false),
                    SubDivisionToDivisionMaterialTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDivisionToDivisionMaterialTransferTrancations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferTrancations_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferTrancations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferTrancations_SubDivisionToDivisionMaterialTransfers_SubDivToDivMaterialTransferId",
                        column: x => x.SubDivToDivMaterialTransferId,
                        principalTable: "SubDivisionToDivisionMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferTrancations_SubDivisionToDivisionMaterialTransfers_SubDivisionToDivisionMaterialTransfe~",
                        column: x => x.SubDivisionToDivisionMaterialTransferId,
                        principalTable: "SubDivisionToDivisionMaterialTransfers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubDivisionToDivisionMaterialTransferTrancations_SubDivisions_SubDivisionId",
                        column: x => x.SubDivisionId,
                        principalTable: "SubDivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_AgencyName",
                table: "Agencies",
                column: "AgencyName",
                unique: true,
                filter: "[AgencyName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalStatusHistory_MaterialId",
                table: "ApprovalStatusHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjectId",
                table: "AspNetUsers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_DateTime",
                table: "AuditLogs",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TableName",
                table: "AuditLogs",
                column: "TableName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName_ProjectId",
                table: "Categories",
                columns: new[] { "CategoryName", "ProjectId" },
                unique: true,
                filter: "[CategoryName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MaterialTypeId",
                table: "Categories",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_CategoryId",
                table: "Contractors",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_ContractorName",
                table: "Contractors",
                column: "ContractorName",
                unique: true,
                filter: "[ContractorName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_MaterialTypeId",
                table: "Contractors",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignDocuments_WorkPackageId",
                table: "DesignDocuments",
                column: "WorkPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferApprovals_DivisionLocationMaterialTransferId",
                table: "DivisionLocationMaterialTransferApprovals",
                column: "DivisionLocationMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferApprovals_IssuerId",
                table: "DivisionLocationMaterialTransferApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferApprovals_RecieverId",
                table: "DivisionLocationMaterialTransferApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransfers_IssueDivisionId",
                table: "DivisionLocationMaterialTransfers",
                column: "IssueDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransfers_MaterialId",
                table: "DivisionLocationMaterialTransfers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransfers_OnBoardedDivisionId",
                table: "DivisionLocationMaterialTransfers",
                column: "OnBoardedDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransfers_RecieveLocationId",
                table: "DivisionLocationMaterialTransfers",
                column: "RecieveLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferTrancations_DivisionMaterialTransferId",
                table: "DivisionLocationMaterialTransferTrancations",
                column: "DivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferTrancations_LocationId",
                table: "DivisionLocationMaterialTransferTrancations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionLocationMaterialTransferTrancations_MaterialId",
                table: "DivisionLocationMaterialTransferTrancations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferApprovals_DivisionMaterialTransferId",
                table: "DivisionMaterialTransferApprovals",
                column: "DivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferApprovals_IssuerId",
                table: "DivisionMaterialTransferApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferApprovals_RecieverId",
                table: "DivisionMaterialTransferApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransfers_IssueDivisionId",
                table: "DivisionMaterialTransfers",
                column: "IssueDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransfers_MaterialId",
                table: "DivisionMaterialTransfers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransfers_OnBoardedDivisionId",
                table: "DivisionMaterialTransfers",
                column: "OnBoardedDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransfers_RecieveDivisionId",
                table: "DivisionMaterialTransfers",
                column: "RecieveDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferTrancations_DivisionId",
                table: "DivisionMaterialTransferTrancations",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferTrancations_DivisionMaterialTransferId",
                table: "DivisionMaterialTransferTrancations",
                column: "DivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionMaterialTransferTrancations_MaterialId",
                table: "DivisionMaterialTransferTrancations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_DivisionName",
                table: "Divisions",
                column: "DivisionName",
                unique: true,
                filter: "[DivisionName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferApprovals_DivisionToSubDivisionMaterialTransferId",
                table: "DivisionToSubDivisionMaterialTransferApprovals",
                column: "DivisionToSubDivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferApprovals_IssuerId",
                table: "DivisionToSubDivisionMaterialTransferApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferApprovals_RecieverId",
                table: "DivisionToSubDivisionMaterialTransferApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransfers_IssueDivisionId",
                table: "DivisionToSubDivisionMaterialTransfers",
                column: "IssueDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransfers_MaterialId",
                table: "DivisionToSubDivisionMaterialTransfers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransfers_OnBoardedDivisionId",
                table: "DivisionToSubDivisionMaterialTransfers",
                column: "OnBoardedDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransfers_TargetSubDivisionId",
                table: "DivisionToSubDivisionMaterialTransfers",
                column: "TargetSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferTrancations_DivisionId",
                table: "DivisionToSubDivisionMaterialTransferTrancations",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferTrancations_DivisionToSubDivisionMaterialTransferId",
                table: "DivisionToSubDivisionMaterialTransferTrancations",
                column: "DivisionToSubDivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferTrancations_MaterialId",
                table: "DivisionToSubDivisionMaterialTransferTrancations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionToSubDivisionMaterialTransferTrancations_SubDivisionId",
                table: "DivisionToSubDivisionMaterialTransferTrancations",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_DocumentName",
                table: "DocumentTypes",
                column: "DocumentName",
                unique: true,
                filter: "[DocumentName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LocationOperations_LocationOperationName_System",
                table: "LocationOperations",
                columns: new[] { "LocationOperationName", "System" },
                unique: true,
                filter: "[LocationOperationName] IS NOT NULL AND [System] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LocationOperations_SubDivisionId",
                table: "LocationOperations",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationOperations_WorkPackageId",
                table: "LocationOperations",
                column: "WorkPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_CategoryId",
                table: "Manufacturers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_ManufacturerName",
                table: "Manufacturers",
                column: "ManufacturerName",
                unique: true,
                filter: "[ManufacturerName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_MaterialTypeId",
                table: "Manufacturers",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialApprovals_ApproverId",
                table: "MaterialApprovals",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialApprovals_MaterialId",
                table: "MaterialApprovals",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialApprovals_ReveiwerId",
                table: "MaterialApprovals",
                column: "ReveiwerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialApprovals_SubmitterId",
                table: "MaterialApprovals",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDocuments_DocumentTypeId",
                table: "MaterialDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDocuments_MaterialId",
                table: "MaterialDocuments",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialHardwares_ManufacturerId",
                table: "MaterialHardwares",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialHardwares_MaterialId",
                table: "MaterialHardwares",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialHardwares_SupplierId",
                table: "MaterialHardwares",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueRecieveVouchers_IssueLocationId",
                table: "MaterialIssueRecieveVouchers",
                column: "IssueLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueRecieveVouchers_MaterialId",
                table: "MaterialIssueRecieveVouchers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueRecieveVouchers_OnBoardedLocationId",
                table: "MaterialIssueRecieveVouchers",
                column: "OnBoardedLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialIssueRecieveVouchers_RecieveLocationId",
                table: "MaterialIssueRecieveVouchers",
                column: "RecieveLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaintenances_AgencyId",
                table: "MaterialMaintenances",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaintenances_MaterialId",
                table: "MaterialMaintenances",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaintenances_MaterialServiceProviderId",
                table: "MaterialMaintenances",
                column: "MaterialServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMeasurements_MeasurementName",
                table: "MaterialMeasurements",
                column: "MeasurementName",
                unique: true,
                filter: "[MeasurementName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_BulkUploadDetailId",
                table: "Materials",
                column: "BulkUploadDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CategoryId",
                table: "Materials",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ContractorId",
                table: "Materials",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_DivisionId",
                table: "Materials",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LocationId",
                table: "Materials",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ManufacturerId",
                table: "Materials",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialTypeId",
                table: "Materials",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MeasurementId",
                table: "Materials",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_RegionId",
                table: "Materials",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_SupplierId",
                table: "Materials",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_WorkPackageId",
                table: "Materials",
                column: "WorkPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSoftwares_MaterialId",
                table: "MaterialSoftwares",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialSoftwares_SupplierId",
                table: "MaterialSoftwares",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_ProjectId",
                table: "MaterialTypes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_TypeName_ProjectId",
                table: "MaterialTypes",
                columns: new[] { "TypeName", "ProjectId" },
                unique: true,
                filter: "[TypeName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancationApprovals_IssuerId",
                table: "MaterialVoucherTrancationApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancationApprovals_MaterialIssueRecieveVoucherId",
                table: "MaterialVoucherTrancationApprovals",
                column: "MaterialIssueRecieveVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancationApprovals_RecieverId",
                table: "MaterialVoucherTrancationApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancations_LocationId",
                table: "MaterialVoucherTrancations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancations_MaterialId",
                table: "MaterialVoucherTrancations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialVoucherTrancations_MaterialIssueRecieveVoucherId",
                table: "MaterialVoucherTrancations",
                column: "MaterialIssueRecieveVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialWarranties_ManufacturerId",
                table: "MaterialWarranties",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialWarranties_MaterialId",
                table: "MaterialWarranties",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AgencyId",
                table: "Projects",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectName",
                table: "Projects",
                column: "ProjectName",
                unique: true,
                filter: "[ProjectName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFeatures_FeatureId",
                table: "RoleFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFeatures_PermissionId",
                table: "RoleFeatures",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFeatures_RoleId",
                table: "RoleFeatures",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviders_ContractorId",
                table: "ServiceProviders",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviders_ManufacturerId",
                table: "ServiceProviders",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferApprovals_IssuerId",
                table: "SubDivisionMaterialTransferApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferApprovals_RecieverId",
                table: "SubDivisionMaterialTransferApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferApprovals_SubDivisionMaterialTransferId",
                table: "SubDivisionMaterialTransferApprovals",
                column: "SubDivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransfers_IssueSubDivisionId",
                table: "SubDivisionMaterialTransfers",
                column: "IssueSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransfers_MaterialId",
                table: "SubDivisionMaterialTransfers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransfers_OnBoardedSubDivisionId",
                table: "SubDivisionMaterialTransfers",
                column: "OnBoardedSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransfers_RecieveSubDivisionId",
                table: "SubDivisionMaterialTransfers",
                column: "RecieveSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferTransactions_MaterialId",
                table: "SubDivisionMaterialTransferTransactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferTransactions_SubDivisionId",
                table: "SubDivisionMaterialTransferTransactions",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionMaterialTransferTransactions_SubDivisionMaterialTransferId",
                table: "SubDivisionMaterialTransferTransactions",
                column: "SubDivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisions_DivisionId",
                table: "SubDivisions",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisions_SubDivisionName_DivisionId",
                table: "SubDivisions",
                columns: new[] { "SubDivisionName", "DivisionId" },
                unique: true,
                filter: "[SubDivisionName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferApprovals_IssuerId",
                table: "SubDivisionToDivisionMaterialTransferApprovals",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferApprovals_RecieverId",
                table: "SubDivisionToDivisionMaterialTransferApprovals",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferApprovals_SubDivToDivMaterialTransferApprovalId",
                table: "SubDivisionToDivisionMaterialTransferApprovals",
                column: "SubDivToDivMaterialTransferApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransfers_IssueSubDivisionId",
                table: "SubDivisionToDivisionMaterialTransfers",
                column: "IssueSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransfers_MaterialId",
                table: "SubDivisionToDivisionMaterialTransfers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransfers_OnBoardedSubDivisionId",
                table: "SubDivisionToDivisionMaterialTransfers",
                column: "OnBoardedSubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransfers_RecieveDivisionId",
                table: "SubDivisionToDivisionMaterialTransfers",
                column: "RecieveDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferTrancations_DivisionId",
                table: "SubDivisionToDivisionMaterialTransferTrancations",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferTrancations_MaterialId",
                table: "SubDivisionToDivisionMaterialTransferTrancations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferTrancations_SubDivisionId",
                table: "SubDivisionToDivisionMaterialTransferTrancations",
                column: "SubDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferTrancations_SubDivisionToDivisionMaterialTransferId",
                table: "SubDivisionToDivisionMaterialTransferTrancations",
                column: "SubDivisionToDivisionMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_SubDivisionToDivisionMaterialTransferTrancations_SubDivToDivMaterialTransferId",
                table: "SubDivisionToDivisionMaterialTransferTrancations",
                column: "SubDivToDivMaterialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CategoryId",
                table: "Suppliers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MaterialTypeId",
                table: "Suppliers",
                column: "MaterialTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierName",
                table: "Suppliers",
                column: "SupplierName",
                unique: true,
                filter: "[SupplierName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPackages_ProjectId",
                table: "WorkPackages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPackages_WorkPackageName",
                table: "WorkPackages",
                column: "WorkPackageName",
                unique: true,
                filter: "[WorkPackageName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppBackupDetails");

            migrationBuilder.DropTable(
                name: "ApprovalStatusHistory");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "DesignDocuments");

            migrationBuilder.DropTable(
                name: "DivisionLocationMaterialTransferApprovals");

            migrationBuilder.DropTable(
                name: "DivisionLocationMaterialTransferTrancations");

            migrationBuilder.DropTable(
                name: "DivisionMaterialTransferApprovals");

            migrationBuilder.DropTable(
                name: "DivisionMaterialTransferTrancations");

            migrationBuilder.DropTable(
                name: "DivisionToSubDivisionMaterialTransferApprovals");

            migrationBuilder.DropTable(
                name: "DivisionToSubDivisionMaterialTransferTrancations");

            migrationBuilder.DropTable(
                name: "ExcelReaderMetadata");

            migrationBuilder.DropTable(
                name: "MaterialApprovals");

            migrationBuilder.DropTable(
                name: "MaterialDocuments");

            migrationBuilder.DropTable(
                name: "MaterialHardwares");

            migrationBuilder.DropTable(
                name: "MaterialMaintenances");

            migrationBuilder.DropTable(
                name: "MaterialSoftwares");

            migrationBuilder.DropTable(
                name: "MaterialVoucherTrancationApprovals");

            migrationBuilder.DropTable(
                name: "MaterialVoucherTrancations");

            migrationBuilder.DropTable(
                name: "MaterialWarranties");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RoleFeatures");

            migrationBuilder.DropTable(
                name: "SubDivisionMaterialTransferApprovals");

            migrationBuilder.DropTable(
                name: "SubDivisionMaterialTransferTransactions");

            migrationBuilder.DropTable(
                name: "SubDivisionToDivisionMaterialTransferApprovals");

            migrationBuilder.DropTable(
                name: "SubDivisionToDivisionMaterialTransferTrancations");

            migrationBuilder.DropTable(
                name: "DivisionLocationMaterialTransfers");

            migrationBuilder.DropTable(
                name: "DivisionMaterialTransfers");

            migrationBuilder.DropTable(
                name: "DivisionToSubDivisionMaterialTransfers");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "ServiceProviders");

            migrationBuilder.DropTable(
                name: "MaterialIssueRecieveVouchers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "SubDivisionMaterialTransfers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubDivisionToDivisionMaterialTransfers");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "BulkUploadDetails");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "LocationOperations");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "MaterialMeasurements");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "SubDivisions");

            migrationBuilder.DropTable(
                name: "WorkPackages");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "MaterialTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Agencies");
        }
    }
}
