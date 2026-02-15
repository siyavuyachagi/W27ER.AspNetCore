using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ward27EmploymentRegistry.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
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
                        name: "FK_AspNetUsers_PhysicalAddresses_PhysicalAddressId",
                        column: x => x.PhysicalAddressId,
                        principalTable: "PhysicalAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncodedToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRefreshTokens_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "Employer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployerType = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicalAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employer_PhysicalAddresses_PhysicalAddressId",
                        column: x => x.PhysicalAddressId,
                        principalTable: "PhysicalAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CloudinaryPublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinaryUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinarySecureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinaryResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinaryFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinaryVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudinaryAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudinaryFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    AspectRatio = table.Column<double>(type: "float", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationSeconds = table.Column<double>(type: "float", nullable: true),
                    BitRate = table.Column<int>(type: "int", nullable: true),
                    VideoCodec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioCodec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrameRate = table.Column<double>(type: "float", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_AspNetUsers_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resources_AspNetUsers_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoleLinks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoleLinks", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoleLinks_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoleLinks_AspNetUserRoles_UserId_RoleId",
                        columns: x => new { x.UserId, x.RoleId },
                        principalTable: "AspNetUserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoleLinks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionsAvailable = table.Column<int>(type: "int", nullable: false),
                    PositionsFilled = table.Column<int>(type: "int", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompensationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkLocationAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumExperienceYears = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    PostedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_AspNetUsers_PostedByUserId",
                        column: x => x.PostedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Jobs_Employer_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Employer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Jobs_PhysicalAddresses_WorkLocationAddressId",
                        column: x => x.WorkLocationAddressId,
                        principalTable: "PhysicalAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Resources_Id",
                        column: x => x.Id,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Resources_Id",
                        column: x => x.Id,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobApplications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedA = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAssignments_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00edafe3-b047-5980-d0fa-da10f400c1e5"), "58e9c747-0189-4088-b3c4-d1752dd3304f", "Admin", "ADMIN" },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), "7badeb99-5b7e-479a-acea-4dccef342d03", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhysicalAddressId", "ProfileImageUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("fd6ebe91-e83e-42e1-be3e-07ba2f25236b"), 0, "34d67e91-4c5a-4875-97d0-4c096fcecf40", "Administrator", "syavuya08@outlook.com", true, "Siyavuya", 1, "Chagi", false, null, null, "SYAVUYA08@OUTLOOK.COM", "5001015009087", "AQAAAAIAAYagAAAAEI2RDy0xnXTih006/tyC+SAayEjM6xYFlOhGuwdTpwfpc3Oii6X7cJFhdyWYbU+S+w==", null, false, null, null, "d672b3c3-1d30-46b2-bde2-11175dac39fb", false, "5001015009087" });

            migrationBuilder.InsertData(
                table: "PhysicalAddresses",
                columns: new[] { "Id", "AdditionalDetails", "City", "Country", "CreatedAt", "DeletedAt", "IsDeleted", "Latitude", "Longitude", "PostalCode", "Province", "StreetName", "StreetNumber", "Suburb", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), null, "Port Elizabeth", "South Africa", new DateTime(2026, 2, 15, 13, 47, 13, 837, DateTimeKind.Utc).AddTicks(2695), null, false, -32.864600000000003, 26.545500000000001, "6759", "Eastern Cape", "Brown Street", "6068", "Berkshire", null },
                    { new Guid("86bbe442-1d6a-4f8a-9f2b-72dc6bb926e5"), null, "Rustenburg", "South Africa", new DateTime(2026, 2, 15, 13, 47, 13, 837, DateTimeKind.Utc).AddTicks(2758), null, false, -28.322500000000002, 31.060300000000002, "7795", "Eastern Cape", "Heller Street", "1315", "Buckinghamshire", null },
                    { new Guid("a0ee568b-8bf2-41a5-a3d9-a2d4d393d66e"), null, "Kimberley", "South Africa", new DateTime(2026, 2, 15, 13, 47, 13, 837, DateTimeKind.Utc).AddTicks(2623), null, false, -23.245200000000001, 25.640999999999998, "9201", "Eastern Cape", "Dlamini Street", "9448", "Berkshire", null },
                    { new Guid("c230c7d4-7c47-48bb-b319-13701cc8c52a"), null, "Pietermaritzburg", "South Africa", new DateTime(2026, 2, 15, 13, 47, 13, 837, DateTimeKind.Utc).AddTicks(2812), null, false, -27.3703, 32.398800000000001, "4910", "Eastern Cape", "Moore Street", "6747", "Buckinghamshire", null },
                    { new Guid("d1c03637-213a-4fad-bcba-a572988e57f7"), "Flat 20", "Rustenburg", "South Africa", new DateTime(2026, 2, 15, 13, 47, 13, 836, DateTimeKind.Utc).AddTicks(1925), null, false, -29.9116, 23.767099999999999, "7988", "Eastern Cape", "Mhlongo Street", null, "Bedfordshire", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("00edafe3-b047-5980-d0fa-da10f400c1e5"), new Guid("fd6ebe91-e83e-42e1-be3e-07ba2f25236b") });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhysicalAddressId", "ProfileImageUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("0d6fe597-26e5-4d4e-a198-be09dfd293c1"), 0, "963c5b6d-8ca6-4c94-940e-0a18659ad52f", "User", "Stuart.Adams@gmail.com", true, "Stuart", 1, "Adams", false, null, null, "STUART.ADAMS@GMAIL.COM", "0504106744080", "AQAAAAIAAYagAAAAEN3UA7NuvPe0O0dgE/kxSpSNWU/nqQwgVeTAAAwoiYvYFMbLixZ4T/NM/ESzEyQ0xg==", null, false, new Guid("a0ee568b-8bf2-41a5-a3d9-a2d4d393d66e"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/967.jpg", "fb020ced-bf08-4196-a021-f71970531a13", false, "0504106744080" },
                    { new Guid("224bbb28-3fac-480c-96af-93f551f2e158"), 0, "215a326e-2dea-4560-8c87-72a3acf1bf35", "User", "Robin.Barrows62@hotmail.com", true, "Robin", 1, "Barrows", false, null, "Kenneth", "ROBIN.BARROWS62@HOTMAIL.COM", "7308095251088", "AQAAAAIAAYagAAAAEH7a+8iIQMIULB1lpCZCH6svugDKeVDS43o8O4O/rewN38zYQY19nwrXVQVPvztjTw==", null, false, new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/263.jpg", "643a8956-509a-4d12-aa98-6afd8442c87b", false, "7308095251088" },
                    { new Guid("23ac6243-73e9-43f0-afd5-2b79292b9577"), 0, "c07d4769-a1ab-4127-ac9d-867e750b9c76", "User", "Erma_Schultz75@hotmail.com", true, "Erma", 1, "Schultz", false, null, "Claudia", "ERMA_SCHULTZ75@HOTMAIL.COM", "9506269295082", "AQAAAAIAAYagAAAAEL5F+hirTHeGFw+fOoZpNNbgKTNU+xIJQX/SiRdwP69MM423FKxZHI54GY4zRuLm3g==", null, false, new Guid("c230c7d4-7c47-48bb-b319-13701cc8c52a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/141.jpg", "84d7d9ae-7b12-40ad-a66e-98912e31e91d", false, "9506269295082" },
                    { new Guid("3ca4435c-2e31-46e4-a74e-1cd56a9162ac"), 0, "987eadba-2c9f-424f-98fb-69f4e942aa6a", "User", "Mark.Parnell@hotmail.com", true, "Mark", 0, "Parnell", false, null, "Ana", "MARK.PARNELL@HOTMAIL.COM", "0202272186083", "AQAAAAIAAYagAAAAEPCdq2GadGEvSX14+DTFfHtrXtyycXBm5ZKanSjWfZfBWApCpESniAdkHJc53jMFOw==", null, false, new Guid("d1c03637-213a-4fad-bcba-a572988e57f7"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/828.jpg", "8837a0b2-e2f8-40cd-a67a-d887ce2c0e57", false, "0202272186083" },
                    { new Guid("4715479e-92d3-4e75-b9fe-a1bfad398b99"), 0, "18220089-d6f2-40ce-b123-6f0aae5e94ca", "User", "Rachel16@yahoo.com", true, "Rachel", 0, "Singh", false, null, null, "RACHEL16@YAHOO.COM", "7203231414080", "AQAAAAIAAYagAAAAELVtUUpIWDSg6ry1eG44LYQucHYBq/k7zwMbRKpo92uDKlAk0GrW4d1Ycd6CodWgaA==", null, false, new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/781.jpg", "ee2f1499-8690-4d14-a88b-60a6b6904403", false, "7203231414080" },
                    { new Guid("6df22e8a-5c8a-4e93-823d-2a430ee8db23"), 0, "2ab9da13-5755-48ef-bb9b-583515f5c49c", "User", "Howard.Gibson64@hotmail.com", true, "Howard", 0, "Gibson", false, null, "Samuel", "HOWARD.GIBSON64@HOTMAIL.COM", "8308024085085", "AQAAAAIAAYagAAAAEKio0YOB0/FUo3kCDm9HlCJzKFTXVdQn8gF6wRAjBKE0eKg5eNLQoEp64UsaOSPR+A==", null, false, new Guid("c230c7d4-7c47-48bb-b319-13701cc8c52a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/543.jpg", "80267d48-1051-4d1b-8bfd-466a5bb429f6", false, "8308024085085" },
                    { new Guid("8e7a5a23-74c0-4947-bbfe-03d15315ff6d"), 0, "821069fb-5ad3-4840-ae12-f7efc2a282b6", "User", "Lindsay40@gmail.com", true, "Lindsay", 1, "Zimmerman", false, null, null, "LINDSAY40@GMAIL.COM", "0202275146084", "AQAAAAIAAYagAAAAEG2izPP3cNz4qZNa2Hde3QcI+FVrCKtFXqtpVm85vXdcjBsu5UENMs+Kc29Oo9Yp4Q==", null, false, new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/230.jpg", "4379ced4-d0c9-4952-8985-6a9c017377f6", false, "0202275146084" },
                    { new Guid("9a789550-511e-41b1-b28e-1f2df764f407"), 0, "7a21c5ac-5683-4cd9-9559-ef887f3c53d1", "User", "Terrence14@gmail.com", true, "Terrence", 0, "Nkomo", false, null, null, "TERRENCE14@GMAIL.COM", "9904270873080", "AQAAAAIAAYagAAAAEMuYYuOSeCrnMGODG/OGiAdNOstfA26cGBbiXyzkVw6mcdAlCfDtuHJPeLRxp/kfPQ==", null, false, new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/1031.jpg", "506f1e28-2ccb-4570-a350-4293525cd33d", false, "9904270873080" },
                    { new Guid("b67d2aea-d09e-41f0-a2e4-dadd9d22a18c"), 0, "e2af3ba3-7f00-4dfd-af9a-e1e98fdbcc2a", "User", "Justin.Poore@yahoo.com", true, "Justin", 1, "Poore", false, null, "Warren", "JUSTIN.POORE@YAHOO.COM", "7907065098088", "AQAAAAIAAYagAAAAEJsLwTU0qjAz+8pumylojlAFaZ6Dem7+YP5iBCpt/5arcD8uYUwOOwcAxVtpEQkUEA==", null, false, new Guid("d1c03637-213a-4fad-bcba-a572988e57f7"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/1094.jpg", "02e97d47-fbfb-40d2-a68a-122fd14ef727", false, "7907065098088" },
                    { new Guid("ca8c5f0b-9dc7-4831-9885-856cb7a35599"), 0, "c04ccb0f-71c2-4a03-b935-f8af2a73fb03", "User", "Dan_Motloung63@hotmail.com", true, "Dan", 1, "Motloung", false, null, "Megan", "DAN_MOTLOUNG63@HOTMAIL.COM", "8101077454088", "AQAAAAIAAYagAAAAEFujLqfYN5xjSDvu7HWTBLXOQB6vY/x9rLR9yB48Lb5Be3ypnWBOQQyaZYsSsDnq0Q==", null, false, new Guid("4aabe8c0-1742-46f1-9653-b9b4b9c6894a"), "https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/557.jpg", "5cfc2669-fb71-4ad0-8be4-60e242eabf0c", false, "8101077454088" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoleLinks",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("00edafe3-b047-5980-d0fa-da10f400c1e5"), new Guid("fd6ebe91-e83e-42e1-be3e-07ba2f25236b") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("0d6fe597-26e5-4d4e-a198-be09dfd293c1") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("224bbb28-3fac-480c-96af-93f551f2e158") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("23ac6243-73e9-43f0-afd5-2b79292b9577") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("3ca4435c-2e31-46e4-a74e-1cd56a9162ac") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("4715479e-92d3-4e75-b9fe-a1bfad398b99") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("6df22e8a-5c8a-4e93-823d-2a430ee8db23") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("8e7a5a23-74c0-4947-bbfe-03d15315ff6d") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("9a789550-511e-41b1-b28e-1f2df764f407") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("b67d2aea-d09e-41f0-a2e4-dadd9d22a18c") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("ca8c5f0b-9dc7-4831-9885-856cb7a35599") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoleLinks",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("0d6fe597-26e5-4d4e-a198-be09dfd293c1") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("224bbb28-3fac-480c-96af-93f551f2e158") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("23ac6243-73e9-43f0-afd5-2b79292b9577") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("3ca4435c-2e31-46e4-a74e-1cd56a9162ac") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("4715479e-92d3-4e75-b9fe-a1bfad398b99") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("6df22e8a-5c8a-4e93-823d-2a430ee8db23") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("8e7a5a23-74c0-4947-bbfe-03d15315ff6d") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("9a789550-511e-41b1-b28e-1f2df764f407") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("b67d2aea-d09e-41f0-a2e4-dadd9d22a18c") },
                    { new Guid("9dfe9b8f-4513-7c23-b3b2-b205864da075"), new Guid("ca8c5f0b-9dc7-4831-9885-856cb7a35599") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRefreshTokens_ApplicationUserId",
                table: "AspNetRefreshTokens",
                column: "ApplicationUserId");

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
                name: "IX_AspNetUserRoleLinks_RoleId",
                table: "AspNetUserRoleLinks",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhysicalAddressId",
                table: "AspNetUsers",
                column: "PhysicalAddressId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employer_PhysicalAddressId",
                table: "Employer",
                column: "PhysicalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Employer_UserId",
                table: "Employer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobId",
                table: "JobApplications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_JobId",
                table: "JobAssignments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_UserId",
                table: "JobAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_PostedByUserId",
                table: "Jobs",
                column: "PostedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_WorkLocationAddressId",
                table: "Jobs",
                column: "WorkLocationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_UploadedByUserId",
                table: "Resources",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_VerifiedByUserId",
                table: "Resources",
                column: "VerifiedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRefreshTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoleLinks");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "JobAssignments");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Employer");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PhysicalAddresses");
        }
    }
}
